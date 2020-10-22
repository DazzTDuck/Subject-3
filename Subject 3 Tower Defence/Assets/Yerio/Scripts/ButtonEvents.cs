using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IPointerExitHandler
{
#pragma warning disable 0649
    [SerializeField] bool clickSound;
    [SerializeField] bool hoverSound;

    [Header("---Show Tooltip---")]
    [SerializeField, Tooltip("Turn this bool on if you want to display a tooltip when this button is hovered over")] bool showHoverTooltip;
    [SerializeField, Tooltip("This text will be displayed when tooltip is activated"), TextArea(1, 4)] string tooltipText;

    AudioManager manager;

    // Start is called before the first frame update
    void Awake()
    {
        manager = FindObjectOfType<AudioManager>();
    }

    public void PlayClickEvent()
    {
        if (clickSound)
            manager.PlaySound("ButtonClick");

        if (showHoverTooltip)
            TooltipUI.RemoveTooltip();
    }

    public void PlayHoverEvent()
    {
        if (hoverSound)
            manager.PlaySound("ButtonHover");

        if (showHoverTooltip)
            TooltipUI.DisplayTooltip(tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData) { TooltipUI.RemoveTooltip(); }
}
