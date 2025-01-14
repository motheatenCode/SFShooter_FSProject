using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Globalization;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class Settings : MonoBehaviour
{
    [Header("--- Gameplay Components ---")]
    [SerializeField] TMP_Text sensTextValue;
    [SerializeField] Slider sensSlider;
    [SerializeField] Toggle invertYToggle;

    // gameplay defaults
    int defaultSensitivity;

    [Header("--- Volume Components ---")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] TMP_Text volumeTextvalue;
    [SerializeField] TMP_Text volumeSFXTextvalue;

    // volume defaults
    float defaultVolume;
    float defaultSFXVolume;

    public void Start()
    {
        // === default values ===
        defaultSensitivity = 100;
        defaultVolume = 1.0f;
        defaultSFXVolume = 1.0f;
        

        // loading gameplay
        if(PlayerPrefs.HasKey("MasterSens") || PlayerPrefs.HasKey("masterInvert"))
        {
            // only calls loadgameplay() is either MasterSens or masterInvert exists
            LoadGameplay();
        }
        else
        {
            // if those keys dont exist, load defaults
            gameplayDefaults();
        }

        // loading volume
        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("SFXVolume"))
        {
            // only calls loadvolume() if either musicVolume of SFXVolume exists
            LoadVolume();
        }
        else
        {
            // if those keys dont exist, load defaults
            volumeDefaults();
        }
    }

    // ===== GAMEPLAY FUNCTIONS =====
    public void SetSensititvity()
    {
        int sensValue = Mathf.RoundToInt(sensSlider.value);
        PlayerPrefs.SetInt("MasterSens", sensValue);
        sensTextValue.text = sensValue.ToString("0");
    }

    public void SetInvertY()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvert", 1);
            //invert Y
        }
        else
        {
            PlayerPrefs.SetInt("masterInvert", 0);
            //Dont invert Y
        }
    }

    public void LoadGameplay()
    {
        // grabs value from playerprefs
        // sensitivity
        int sensValue = PlayerPrefs.GetInt("MasterSens");
        sensSlider.value = sensValue;
        sensTextValue.text = sensValue.ToString("0");

        // invert y
        int isInverted = PlayerPrefs.GetInt("masterInvert");
        if(isInverted == 1)
        {
            invertYToggle.isOn = true;
        }
        else
        {
            invertYToggle.isOn = false;
        }
    }

    public void gameplayDefaults()
    {
        PlayerPrefs.SetInt("MasterSens", defaultSensitivity);
        PlayerPrefs.SetInt("masterInvert", 0); // default is false

        // load the values
        LoadGameplay();
    }

    // ===== VOLUMES FUNCTIONS =====
    public void SetMusicVolume()
    {
        // grabs value from music slider
        float volume = soundSlider.value;
        // converts for audio mixer
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        // saves it in playerprefs
        PlayerPrefs.SetFloat("musicVolume", volume);
        // update text value in settings
        volumeTextvalue.text = volume.ToString("p0", CultureInfo.InvariantCulture);
    }

    public void SetSFXVolume()
    {
        // grabes value from sfx slider
        float SFXvolume = SFXSlider.value;
        // converts for audio mixer
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXvolume) * 20);
        // saves it in playerprefs
        PlayerPrefs.SetFloat("SFXVolume", SFXvolume);
        // update text in settings
        volumeSFXTextvalue.text = SFXvolume.ToString("p0", CultureInfo.InvariantCulture);
    }

    public void LoadVolume()
    {
        // grabs value from playerprefs
        // music volume
        float musicVol = PlayerPrefs.GetFloat("musicVolume");
        audioMixer.SetFloat("Music", Mathf.Log10(musicVol) * 20);
        soundSlider.value = musicVol;
        volumeTextvalue.text = musicVol.ToString("p0", CultureInfo.InvariantCulture);

        // sfx volume
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume");
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxVol) * 20);
        SFXSlider.value = sfxVol;
        volumeSFXTextvalue.text = sfxVol.ToString("p0", CultureInfo.InvariantCulture);
    }

    public void volumeDefaults()
    {
        // sets playerprefs to default values
        PlayerPrefs.SetFloat("musicVolume", defaultVolume);
        PlayerPrefs.SetFloat("SFXVolume", defaultSFXVolume);

        // loads the default values
        LoadVolume();
    }
  
}       