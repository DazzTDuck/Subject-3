using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Assertions.Must;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer masterMixer;

    [Header("Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;

    [Header("Sliders")]
    public Slider master;
    public Slider sfx;
    public Slider music;

    [Header("Toggles")]
    public Toggle fullscreenToggle;
    public Toggle motionBlurToggle;
    public Toggle bloomToggle;

    [Header("PostFX Profile")]
    public VolumeProfile postFx;

    MotionBlur motionBlur;
    Bloom bloom;

    //values
    public static float masterVolume = 1f;
    public static float sfxVolume = 1f;
    public static float musicVolume = 1f;

    public static bool fullscreen = true;
    public static bool bloomOn = true;
    public static bool motionBlurOn = true;

    public Resolution[] resolutions;
    public static int currentResIndex;

    public static int currentQualityIndex = 2;

    Animator settingsAnimator;

    private void Start()
    {
        if (resolutionDropdown)
            GetResolutions();

        GetAllPlayerPrefsValues();

        postFx.TryGet(out bloom);
        postFx.TryGet(out motionBlur);

        SetMasterVolume(masterVolume);
        if(master)
        master.value = masterVolume;

        SetSFXVolume(sfxVolume);
        if(sfx)
        sfx.value = sfxVolume;

        SetMusicVolume(musicVolume);
        if(music)
        music.value = musicVolume;

        if (qualityDropdown)
        {
            SetQuality(currentQualityIndex);
            qualityDropdown.value = currentQualityIndex;
        }

        if (resolutionDropdown)
        {
            SetResolution(currentResIndex);
            resolutionDropdown.value = currentResIndex;
        }
        if (fullscreenToggle)
        {
            SetFullscreen(fullscreen);
            fullscreenToggle.isOn = fullscreen;
        }
        if (bloomToggle)
        {
            SetBloom(bloomOn);
            bloomToggle.isOn = bloomOn;
        }
        if (motionBlurToggle)
        {
            SetMotionBlur(motionBlurOn);
            motionBlurToggle.isOn = motionBlurOn;
        }

        settingsAnimator = GetComponentInChildren<Animator>();
    }

    public void SetAllPlayerPrefsValues()
    {
        //this function sets all the current settings values to the player prefs

        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        PlayerPrefs.SetInt("ResIndex", currentResIndex);
        PlayerPrefs.SetInt("QualityIndex", currentQualityIndex);

        PlayerPrefs.SetInt("FullscreenBool", fullscreen ? 1 : 0);
        PlayerPrefs.SetInt("BloomBool", bloomOn ? 1 : 0);
        PlayerPrefs.SetInt("MotionBlurBool", motionBlurOn ? 1 : 0);

        PlayerPrefs.Save();

        //--set a bool--
        //PlayerPrefs.SetInt("Name", yourBool ? 1 : 0);
        //----
    }
    public void GetAllPlayerPrefsValues()
    {
        //this function gets all the player prefs and sets the static values

        masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

        currentResIndex = PlayerPrefs.GetInt("ResIndex");
        currentQualityIndex = PlayerPrefs.GetInt("QualityIndex");

        fullscreen = PlayerPrefs.GetInt("FullscreenBool") != 0;
        bloomOn =  PlayerPrefs.GetInt("BloomBool") != 0;
        motionBlurOn = PlayerPrefs.GetInt("MotionBlurBool") != 0;

        //--get a bool--
        //yourBool = (PlayerPrefs.GetInt("Name") != 0);
        //----
    }

    public void GetResolutions()
    {
        //get and set the resolutions for the dropdown
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            if (!options.Contains(option))
            {
                options.Add(option);               
            }       

            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SettingsTrigger(string triggerName)
    {
        settingsAnimator.SetTrigger(triggerName);
    }

    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        currentResIndex = resIndex;
        SetAllPlayerPrefsValues();
    }

    //control the different volume sliders
    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterValue", volume);
        masterVolume = volume;

        SetAllPlayerPrefsValues();
    }
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicValue", volume);
        musicVolume = volume;

        SetAllPlayerPrefsValues();
    }
    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFXValue", volume);
        sfxVolume = volume;

        SetAllPlayerPrefsValues();
    }

    //set the quality of the game
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        SettingsMenu.currentQualityIndex = qualityIndex;

        SetAllPlayerPrefsValues();
    }

    //set the fullscreen of the game
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        fullscreen = isFullscreen;

        SetAllPlayerPrefsValues();
    }
    public void SetBloom(bool isBloom)
    {
        bloom.active = isBloom;
        bloomOn = isBloom;


        SetAllPlayerPrefsValues();
    }
    public void SetMotionBlur(bool isMB)
    {
        motionBlur.active = isMB;
        motionBlurOn = isMB;

        SetAllPlayerPrefsValues();
    }

}
