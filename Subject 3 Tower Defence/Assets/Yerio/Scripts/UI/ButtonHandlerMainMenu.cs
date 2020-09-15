using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandlerMainMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UIAnimations animations;
    AudioManager audioManager;

    public float animationOffset = 10f;
    public float pressScaleOffset = 5f;

    Vector3 originalPos;
    private void Awake()
    {
        originalPos = transform.position;
        animations = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIAnimations>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animations.EnterButtonAnimation(transform, animationOffset);
        audioManager.PlaySound("ButtonHover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animations.LeaveButtonAnimation(transform, originalPos);
    }

    public void ButtonClick()
    {
        StartCoroutine(animations.ButtonPressAnimation(transform, pressScaleOffset));
        audioManager.PlaySound("ButtonClick");
    }
}
