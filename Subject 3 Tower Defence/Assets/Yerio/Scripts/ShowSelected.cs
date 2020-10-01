using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShowSelected : MonoBehaviour, IPointerClickHandler
{
#pragma warning disable 0649
    [SerializeField] Color selectedColor;
    [SerializeField] Color deselectedColor;

    public bool startSelectedButton;
    
    static TMP_Text lastTextSelected;

    private void Start()
    {
        if (startSelectedButton)
        {
            var text = GetComponentInChildren<TMP_Text>();
            lastTextSelected = text;
            lastTextSelected.color = selectedColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var clickedObject = eventData.pointerCurrentRaycast.gameObject;

        if (clickedObject.GetComponent<TMP_Text>() && lastTextSelected)
        {
            lastTextSelected.color = deselectedColor;
            lastTextSelected = null;
        }

        if (clickedObject.GetComponent<TMP_Text>())
        {
            lastTextSelected = clickedObject.GetComponent<TMP_Text>();
            lastTextSelected.color = selectedColor;
        }
    }
}
