using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ScrapCollector : BaseTower
{
    [Header("---Scrap Collector---")]
    [Tooltip("This is the minimum distance that has to be between the collector and scrap for it to be actually collected," +
        " so that the currency gets added to the player at the right time")]
    public float minDistanceBetweenScrapAndTower;

    List<Scrap> scrapThatIsCollecting = new List<Scrap>();
    Scrap targetScrap;

    protected override void Start()
    {
        base.Start();
    }

    protected override void ShootToTarget()
    {
        if(onTarget && canShoot)
        {
            if(!scrapThatIsCollecting.Contains(targetScrap))
                scrapThatIsCollecting.Add(targetScrap);

            for (int i = 0; i < scrapThatIsCollecting.Count; i++)
            {
                scrapThatIsCollecting[i].canBePickedUp = false;
                scrapThatIsCollecting[i].PickupScrap(transform.position, currentShootSpeed);
            }
        }

        if (canShoot)
        {
            if (scrapThatIsCollecting.Count > 0)
            {
                if(scrapThatIsCollecting[0] != null)
                {
                    if (Vector3.Distance(scrapThatIsCollecting[0].transform.position, transform.position) < minDistanceBetweenScrapAndTower)
                    {
                        scrapThatIsCollecting[0].CollectScrap(buyingHandler.currencyManager, 2, scrapThatIsCollecting);
                    }
                }
                else
                {
                    scrapThatIsCollecting.RemoveAt(0);
                }

            }
        }       
    }

    protected override void TargetDetection()
    {
        //get all scrap on the ground
        var allScrapOnGround = FindObjectsOfType<Scrap>();

        var minDistance = Mathf.Infinity;
        Scrap targetScrap = null;
        float distance;

        foreach (var scrap in allScrapOnGround)
        {
            if (scrap.canBePickedUp)
            {
                distance = Vector3.Distance(scrap.transform.position, transform.position);

                if (distance < currentDetectionDistance)
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        targetScrap = scrap;
                    }
                }
            }     
        }

        onTarget = targetScrap;
        this.targetScrap = targetScrap;

        if (onTarget)
        {
            if (shootingPoint)
                Debug.DrawRay(shootingPoint.position, this.targetScrap.transform.position - transform.position, Color.red);
            //Debug.Log(distance);
        }
    }

    protected override void RotateHeadToEnemy()
    {
        
    }
}
