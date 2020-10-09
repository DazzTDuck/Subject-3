using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    public float amount = 0;
    public float pickupTime = 5f;
    [Space]
    public bool stayOnScreen = false;

    Timer scrapPickupTimer;
    readonly float lerpTime = 2.5f;
    float collectionSpeed = 1f;
    //[HideInInspector]
    public bool canBePickedUp = true;
    SignWave signWave;
    bool lerpDown;
    bool pickupLerp;

    Vector3 lerpPos;
    Vector3 pickupPos;

    private void Start()
    {
        lerpPos = transform.position;
        signWave = GetComponent<SignWave>();
        scrapPickupTimer = gameObject.AddComponent<Timer>();
        if (!stayOnScreen)
        {
            scrapPickupTimer.SetTimer(pickupTime, () => { StartCoroutine(OnTimerEnd()); signWave.DeactivateSignWave(); canBePickedUp = false; });
        }
    }
    private void Update()
    {
        if (lerpDown)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up, lerpTime * Time.deltaTime);
        }
        if (pickupLerp)
        {
            transform.position = Vector3.MoveTowards(transform.position, pickupPos, collectionSpeed * Time.deltaTime);
        }
    }
    IEnumerator OnTimerEnd()
    {
        if (!canBePickedUp)
        {
            lerpDown = true;

            yield return new WaitForSeconds(2);

            Destroy(gameObject);
        }
    }

    public void PickupScrap(Vector3 posistion, float collectionSpeed)
    {
        this.collectionSpeed = collectionSpeed;
        pickupPos = posistion;
        canBePickedUp = false;
        pickupLerp = true;
    }

    public void CollectScrap(CurrencyManager currency, float multiplier = 1f, List<Scrap> listToRemoveFrom = null)
    {
        if (scrapPickupTimer.IsTimerActive() || stayOnScreen)
        {
            currency.AddCurrency(amount * multiplier);

            if (listToRemoveFrom != null)
            {
                var index = listToRemoveFrom.IndexOf(this);
                listToRemoveFrom.RemoveAt(index);
            }

            //play pickup effect
            Destroy(gameObject);
        }
    }
}
