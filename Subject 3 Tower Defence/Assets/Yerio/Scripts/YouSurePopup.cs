using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class YouSurePopup : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] Button yesButton;

    Animator animator;
    LevelLoader loader;

    [HideInInspector] public bool isActive = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        loader = FindObjectOfType<LevelLoader>();
    }

    public void PopupToMainMenu()
    {
        animator.SetTrigger("Open");
        SetActive(true);

        yesButton.onClick.AddListener(() => {
            Time.timeScale = 1f;
            loader.LoadNewScene(0);
            SetActive(false);
            animator.SetTrigger("Close"); 
        });
    }

    public void SetActive(bool state)
    {
        isActive = state;
    }
}
