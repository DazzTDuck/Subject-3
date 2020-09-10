using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    UIAnimations animations;
    CanvasGroup canvas;

    public void DontDestroyOnload()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        DontDestroyOnload();
        animations = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIAnimations>();
        canvas = GetComponent<CanvasGroup>();
    }

    public void LoadNewScene()
    {
        StartCoroutine(animations.SceneTransistion(1, canvas, true));
    }
}
