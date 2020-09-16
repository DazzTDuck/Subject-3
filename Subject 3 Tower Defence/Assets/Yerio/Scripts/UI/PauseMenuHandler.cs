using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    Animator pauseMenuAnimator;

    bool pausemenuAnimationStart = false;
    bool pausemenuAnimationOpen = false;

    Button[] buttons;

    private void Awake()
    {
        pauseMenuAnimator = GetComponent<Animator>();
        buttons = GetComponentsInChildren<Button>();
    }

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
                PauseTime();
                return;
            }

            if (!pausemenuAnimationStart && pausemenuAnimationOpen)
                StartCloseMenu();
        }
    }

    public void StartCloseMenu()
    {
        StartCoroutine(CloseMenu());
    }

    public IEnumerator CloseMenu()
    {
        ResumeTime();
        pauseMenuAnimator.SetTrigger("Close");

        yield return new WaitForSeconds(0.25f);

        foreach (var button in buttons)
        {
            if (EventSystem.current.currentSelectedGameObject == button.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        StopCoroutine(CloseMenu());
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

    public void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1;
    }
}
