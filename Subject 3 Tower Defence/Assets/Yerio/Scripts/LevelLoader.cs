using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class LevelLoader : MonoBehaviour
{
    public float transistionWaitTime = 1f;
    public float buttonPressTime = 0.3f;

    public static LevelLoader instance;

    Animator animator;

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
        //DontDestroyOnload();
        animator = GetComponent<Animator>();
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadNewScene(int levelIndex)
    {
        StartCoroutine(LoadScene(levelIndex));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        yield return new WaitForSeconds(buttonPressTime);

        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transistionWaitTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
