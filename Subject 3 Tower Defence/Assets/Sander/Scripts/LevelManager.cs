using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{

    //De levels die zijn gecomplete worden opgeslagen in de int "completedlevels" in de playerPrefs
    //Deze int geeft de laatste (hoogste getal) index aan van een level dat behaald is en dus elk level er voor 


    [SerializeField] LevelLoader levelLoader;

    [SerializeField] Button[] levelSelectButtons;
    [SerializeField] TMP_Text[] levelStateText;

    //geef in elk level hier aan wat de index van dat level is
    public int lvlindex = 0;

    int indexB;
    public string levelStateUnlockedString;
    public string levelStateCompletionString;
    public string levelStateLockedString;


    void Awake()
    {
        if (levelSelectButtons != null)
        {
            UpdateLvlSelectUi();
            print("not empty");
        }
        print("Empty");
    }

    private void UpdateLvlSelectUi()
    {
        foreach (Button button in levelSelectButtons)
        {
            if (indexB < PlayerPrefs.GetInt("completedLevels"))
            {
                button.interactable = true;
                levelStateText[indexB].text = levelStateCompletionString;
            }
            else if (indexB == PlayerPrefs.GetInt("completedLevels"))
            {
                button.interactable = true;
                levelStateText[indexB].text = levelStateUnlockedString;
            }
            else
            {
                button.interactable = false;
                levelStateText[indexB].text = levelStateLockedString;
            }


            indexB++;
        }
        indexB = 0;
    }

    public void SelectLvl(int lvlIndex)
    {
        if (PlayerPrefs.GetInt("completedLevels", 0) >= lvlIndex - 1)
        {
            levelLoader.LoadNewScene(lvlIndex + SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            print("not compleded last lvl");
        }
    }

    public void LvlCompleted()
    {
        if (lvlindex > PlayerPrefs.GetInt("completedLevels"))
        {
            PlayerPrefs.SetInt("completedLevels", lvlindex);
            UpdateLvlSelectUi();
        }
    }

    public void ResetCompletion()
    {
        PlayerPrefs.SetInt("completedLevels", 0);
        UpdateLvlSelectUi();

        print("RESET " + PlayerPrefs.GetInt("completedLevels") + " <- should be 0");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

   
}
