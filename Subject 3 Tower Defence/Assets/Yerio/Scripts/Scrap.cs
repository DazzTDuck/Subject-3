using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    public float amount = 0;
    public float pickupTime = 5f;

    Timer scrapPickupTimer;
    readonly float lerpTime = 2.5f;
    bool canBePickedUp = true;

    SignWave signWave;

    bool lerpDown;

    private void Start()
    {
        signWave = GetComponent<SignWave>();
        scrapPickupTimer = gameObject.AddComponent<Timer>();
        scrapPickupTimer.SetTimer(pickupTime, () => { StartCoroutine(OnTimerEnd()); signWave.DeactivateSignWave(); canBePickedUp = false; });
    }
    private void Update()
    {
        if (lerpDown)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up, lerpTime * Time.deltaTime);
        }
    }
    IEnumerator OnTimerEnd()
    {
        lerpDown = true;

        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }

    public void PickupScrap(CurrencyManager currency)
    {
        if (canBePickedUp)
        {
            currency.AddCurrency(amount);
            //play pickup effect
            Destroy(gameObject);
        }
    }
}
