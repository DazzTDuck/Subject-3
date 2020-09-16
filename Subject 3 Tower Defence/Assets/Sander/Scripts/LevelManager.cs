using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // wordt gebruikt om aan tegeven welk lvls de player al gecomplete heeft bijv. 2 voor als je lvl 2 (en dus 1) gedaan hebt en 13 voor als je lvl 13 (en dus alle er voor) gedaan hebt
    // wordt opgeslagen in PlayerPref Int "completedLevels"
     private int completedIndex;


    public void SelectLvl(int lvlIndex)
    {
        completedIndex = PlayerPrefs.GetInt("completedLevels", 0);
        if (completedIndex >= lvlIndex - 1)
        {
            print("load " + lvlIndex);
            SceneManager.LoadScene(lvlIndex + SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            print("no " + completedIndex);
        }
    }

    public void LvlCompleted(int lvlindex)
    {
        if (lvlindex > PlayerPrefs.GetInt("completedLevels"))
        {
            PlayerPrefs.SetInt("completedLevels", lvlindex);
            print("completed ");
        }
    }

    public void ResetCompletion()
    {
        PlayerPrefs.SetInt("completedLevels", 0);
        completedIndex = 0;

        print("RESET " + completedIndex + " <- should be 0");
    }
}
