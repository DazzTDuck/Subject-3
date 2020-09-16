using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundEvents : MonoBehaviour
{
    AudioManager manager;

    // Start is called before the first frame update
    void Awake()
    {
        manager = FindObjectOfType<AudioManager>();
    }

    public void PlayClickEvent()
    {
        manager.PlaySound("ButtonClick");
    }

    public void PlayHoverEvent()
    {
        manager.PlaySound("ButtonHover");
    }
}
