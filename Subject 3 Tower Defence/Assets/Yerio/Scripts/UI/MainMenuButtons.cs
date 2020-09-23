using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("quit game");
        Application.Quit();
    }
}
