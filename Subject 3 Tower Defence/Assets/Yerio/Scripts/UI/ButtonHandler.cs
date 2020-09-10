using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UIAnimations animations;
    public float animationOffset = 10f;
    public float pressScaleOffset = 5f;

    Vector3 originalPos;
    private void Awake()
    {
        originalPos = transform.position;
        animations = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIAnimations>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animations.EnterButtonAnimation(transform, animationOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animations.LeaveButtonAnimation(transform, originalPos);
    }

    public void ButtonClick()
    {
        StartCoroutine(animations.ButtonPressAnimation(transform, pressScaleOffset));
    }
}
