using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] Animator pauseMenuAnimator;

    [SerializeField] bool pausemenuAnimationStart = false;
    [SerializeField] bool pausemenuAnimationOpen = false;


    // Update is called once per frame
    void Update()
    {
        ShowPauseMenu();
    }

    void ShowPauseMenu()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!pausemenuAnimationStart && !pausemenuAnimationOpen)
            {
                pauseMenuAnimator.SetTrigger("Open");
                return;
            }

            if(!pausemenuAnimationStart && pausemenuAnimationOpen)
            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        pauseMenuAnimator.SetTrigger("Close");
    }

    public void PauseMenuAnimationStart()
    {
        pausemenuAnimationStart = true;
    }

    public void PauseMenuAnimationOpenDone()
    {
        pausemenuAnimationStart = false;
        pausemenuAnimationOpen = true;
    }

    public void PauseMenuAnimationCloseDone()
    {
        pausemenuAnimationStart = false;
        pausemenuAnimationOpen = false;
    }
}
