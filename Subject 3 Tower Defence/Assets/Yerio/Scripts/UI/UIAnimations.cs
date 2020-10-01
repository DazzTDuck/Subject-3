using System.Collections;
using TMPro;
using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    [SerializeField,Header("---Popup Text---")]
    float popupLeanSpeed = 1.5f;
    public float popupDelayTime = 2.2f;

    public static UIAnimations instance;

    private void Awake()
    {
        DontDestroyOnload();
    }

    public void DontDestroyOnload()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator PopupText(TMP_Text _popupText, string _text)
    {
        _popupText.text = _text;

        LeanTween.scale(_popupText.gameObject, new Vector3(1.2f, 1.2f, 1.2f), popupLeanSpeed).setEaseInOutExpo();

        yield return new WaitForSeconds(popupDelayTime);

        LeanTween.scale(_popupText.gameObject, new Vector3(0f, 0f, 0f), popupLeanSpeed).setEaseInOutExpo();

        StopCoroutine("PopupText");
    }
}
