using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEvents : MonoBehaviour
{
    [SerializeField] bool clickSound;
    [SerializeField] bool hoverSound;

    AudioManager manager;

    // Start is called before the first frame update
    void Awake()
    {
        manager = FindObjectOfType<AudioManager>();
    }

    public void PlayClickEvent()
    {
        if(clickSound)
        manager.PlaySound("ButtonClick");
    }

    public void PlayHoverEvent()
    {
        if(hoverSound)
        manager.PlaySound("ButtonHover");
    }
}
