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

        LeanTween.scale(_popupText.gameObject, new Vector3(0.001f, 0.001f, 0.001f), popupLeanSpeed).setEaseInOutExpo();

        StopCoroutine("PopupText");
    }

    [SerializeField, Header("---Decend Text---")]
    float decendTweenSpeed = 2f;
    [SerializeField]
    float decendValule = 50f;

    public IEnumerator DecendTextDown(GameObject _text)
    {
        //Debug.Log(_text.transform.localPosition);

        LeanTween.moveLocalY(_text, _text.transform.localPosition.y - decendValule, decendTweenSpeed).setEaseInOutBack();

        yield return new WaitForSeconds(decendTweenSpeed + 0.1f);

        StopCoroutine("DecendTextDown");
    }
    public IEnumerator DecendTextUp(GameObject _text, Vector3 originalPos)
    {
        LeanTween.moveLocalY(_text, _text.transform.localPosition.y + decendValule, decendTweenSpeed).setEaseInOutBack();

        yield return new WaitForSeconds(decendTweenSpeed + 0.1f);

        _text.transform.position = originalPos;

        StopCoroutine("DecendTextUp");
    }

    [Header("---Button animations---")]
    [SerializeField]
    float moveTweenSpeed = 0.15f;

    public void EnterButtonAnimation(Transform button, float offset)
    {
        var newPos = new Vector3(button.position.x + offset, button.position.y, button.position.z);
        LeanTween.move(button.gameObject, newPos, moveTweenSpeed);
    }
    public void LeaveButtonAnimation(Transform button, Vector3 OriginalPos)
    {
        LeanTween.move(button.gameObject, OriginalPos, moveTweenSpeed);
    }

    [SerializeField]
    float pressWaitTime = 0.3f;
    [SerializeField]
    float pressTweenSpeed = 0.2f;
    public IEnumerator ButtonPressAnimation(Transform button, float scaleOffset)
    {
        var originalScale = button.localScale;

        LeanTween.scale(button.gameObject, originalScale * scaleOffset, pressTweenSpeed).setEaseInOutBack();

        yield return new WaitForSeconds(pressWaitTime);

        LeanTween.scale(button.gameObject, originalScale, pressTweenSpeed).setEaseInOutBack();

        StopCoroutine("ButtonPressAnimation");
    }
}
