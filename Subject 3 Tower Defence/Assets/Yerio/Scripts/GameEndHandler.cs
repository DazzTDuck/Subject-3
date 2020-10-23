using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEndHandler : MonoBehaviour
{
#pragma warning disable
    [SerializeField] LevelManager levelManager;

    [SerializeField] Sprite winImage;
    [SerializeField] Sprite loseImage;

    [SerializeField] Image gameEndBG;

    [SerializeField] Button button1;

    [HideInInspector] public bool levelEnded = false;

    Animator animator;
    TMP_Text buttonText;
    LevelLoader levelLoader;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        buttonText = button1.GetComponentInChildren<TMP_Text>();
        levelLoader = FindObjectOfType<LevelLoader>();
        levelEnded = false;
    }

    private void Start()
    {
        //GameEndSetup(false);
    }

    public void GameEndSetup(bool winTrigger)
    {
        levelEnded = true;
        string text;
        Sprite sprite;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        button1.onClick.AddListener(() => { Time.timeScale = 1f; animator.SetTrigger("Close"); });

        if (winTrigger)
        {
            levelManager.LvlCompleted();
            sprite = winImage;
            text = "Level Selector";
            //set button OnClick to restart the level
            button1.onClick.AddListener(() => levelLoader.LoadNewScene(1));
        }
        else
        {
            sprite = loseImage;
            text = "Restart Level";
            //set button OnClick to load the next level 
            button1.onClick.AddListener(() => levelLoader.LoadNewScene(SceneManager.GetActiveScene().buildIndex));
        }

        gameEndBG.sprite = sprite;
        buttonText.text = text;
        animator.SetTrigger("Open");
    }

}
