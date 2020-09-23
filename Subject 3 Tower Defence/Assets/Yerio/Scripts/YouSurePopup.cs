using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class YouSurePopup : MonoBehaviour
{
    [SerializeField] Button yesButton;

    Animator animator;
    LevelLoader loader;

    [HideInInspector] public bool isActive = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        loader = FindObjectOfType<LevelLoader>();
    }

    /// <summary>
    /// This function calls the "Are you sure?" menu before continueing with the action that is given
    /// </summary>
    /// <param name="action">event that takes place when the Yes button is pressed</param>
    public void Popup(UnityAction action)
    {
        animator.SetTrigger("Open");
        yesButton.onClick.AddListener(action);
        yesButton.onClick.AddListener(delegate () { Time.timeScale = 1f; });
    }

    public void PopupToMainMenu()
    {
        animator.SetTrigger("Open");
        SetActive(true);
        yesButton.onClick.AddListener(delegate () { Time.timeScale = 1f; });
        yesButton.onClick.AddListener(delegate () { loader.LoadNewScene(0); });
        yesButton.onClick.AddListener(delegate () { SetActive(false); });
        yesButton.onClick.AddListener(delegate () { animator.SetTrigger("Close"); });
    }

    public void SetActive(bool state)
    {
        isActive = state;
    }
}
