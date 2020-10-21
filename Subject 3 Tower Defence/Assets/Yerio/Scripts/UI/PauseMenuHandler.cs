﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    public YouSurePopup popup;

    Animator pauseMenuAnimator;

    bool pausemenuAnimationStart = false;
    bool pausemenuAnimationOpen = false;

    Button[] buttons;
    GameEndHandler endHandler;

    bool settingsOpen = false;

    private void Awake()
    {
        pauseMenuAnimator = GetComponent<Animator>();
        buttons = GetComponentsInChildren<Button>();
        endHandler = FindObjectOfType<GameEndHandler>();
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
            if (!pausemenuAnimationStart && !pausemenuAnimationOpen && !endHandler.levelEnded)
            {
                pauseMenuAnimator.SetTrigger("Open");
                PauseTime();
                return;
            }

            if (!pausemenuAnimationStart && pausemenuAnimationOpen && !popup.isActive && !settingsOpen)
                StartCloseMenu();
        }
    }

    public void StartCloseMenu()
    {
        StartCoroutine(CloseMenu());
    }

    public void SettingsOpen() { settingsOpen = true; }
    public void SettingsClose() { settingsOpen = false; }

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
