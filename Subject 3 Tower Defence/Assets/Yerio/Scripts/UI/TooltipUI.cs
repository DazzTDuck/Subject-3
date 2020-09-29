using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] RectTransform backgroundRectTransform;
    [SerializeField] TextMeshProUGUI textMeshPro;

    RectTransform rectTransform;
    Animator animator;

    bool isShown;

    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        //SetText("Testing Text");
        RemoveTooltip();
    }

    private void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if(anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            //Tooltip has left the screen on right side
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            //Tooltip has left the screen on top side
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }

    void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);

        textMeshPro.ForceMeshUpdate();
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(20, 20);

        backgroundRectTransform.sizeDelta = textSize + padding;
    }

    void Hide()
    {
        animator.SetTrigger("Hide");
    }

    void Show(string tooltipText)
    {
        if (!Instance.isShown)
        {
            Instance.isShown = true;
            animator.SetTrigger("Show");
            SetText(tooltipText);
        }      
    }

    public static void DisplayTooltip(string tooltipText)
    {
        Instance.Show(tooltipText);
    }
    public static void RemoveTooltip()
    {
        if(Instance.isShown)
        Instance.Hide();
    }

    public void CanShowAgain() { isShown = false; }

}
