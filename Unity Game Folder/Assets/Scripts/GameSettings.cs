using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Application = UnityEngine.Application;

public class GameSettings : MonoBehaviour
{
    [HideInInspector]
    public bool loadTutorial;
    [HideInInspector]
    public int vfxVolume;
    [HideInInspector]
    public int musicVolume;
    [HideInInspector]
    public float sensitivity;

    public AudioMixer masterMixer;
    public AudioSource vfxTestNoise;

    [HideInInspector]
    public int qualitySetting;
    [HideInInspector]
    public int displayMode;

    public int currLevel = 1;

    public static GameSettings Instance;

    private const string DEFAULT_VALUE = "masterVolume=10,musicVolume=-10,vfxVolume=-10,sensitivity=3,loadTutorial=False,quality=2,displayMode=0,lastLevel=1";
    private void Start()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }
    private void SetDefaultSettings()
    {
        File.WriteAllText(Application.dataPath + "/Resources/options.txt", DEFAULT_VALUE);
    }

    public void ResetLevel()
    {
        currLevel = 1;
    }
    private void LoadSettings()
    {
        string saveString;
        // Get file as String
        try
        {
            saveString = File.ReadAllText(Application.dataPath + "/Resources/options.txt");
        }
        catch
        {
            SetDefaultSettings();
            LoadSettings();
            return;
        }
        // Remove Whitespace
        saveString = Regex.Replace(saveString, @"\s+", "");

        Debug.Log("Save String: " + saveString);
        char[] listOfChar = saveString.ToCharArray();

        if (listOfChar.Length <= 1 || listOfChar == null || listOfChar.Length == 0)
        {
            SetDefaultSettings();
            LoadSettings();
            return;
        }

        List<string> values = new List<string>() ;
        string tempString = "";
        foreach (char character in listOfChar)
        {
            if (character == ',' || character == '=')
            {
                values.Add(tempString);
                tempString = "";
            }
            else
                tempString = tempString + character;
        }
        values.Add(tempString);

        // Master Volume
        if (values[0] == "masterVolume")
            masterMixer.SetFloat("MasterVolume", int.Parse(values[1]));
        else
            Debug.Log("MasterVolume not found in Game Settings.");

        if (values[2] == "musicVolume")
        {
            musicVolume = int.Parse(values[3]);
            masterMixer.SetFloat("MusicVolume", musicVolume);
        }
        else
            Debug.Log("musicVolume not found in Game Settings.");
        if (values[4] == "vfxVolume")
        {
            vfxVolume = int.Parse(values[5]);
            if (!masterMixer.SetFloat("VFXVolume", vfxVolume))
                Debug.Log("why");
        }
        else
            Debug.Log("VFXVolume not found in Game Settings.");
        if (values[6] == "sensitivity")
            sensitivity = float.Parse(values[7]);
        else
        {
            Debug.Log("Sensitivity not found in Game Settings.");
            sensitivity = 3;
        }
        if (values[8] == "loadTutorial")
            loadTutorial = bool.Parse(values[9]);
        else
        {
            Debug.Log("Load Tutorial not found in Game Settings.");
            loadTutorial = false;
        }
        if (values[10] == "quality")
            qualitySetting = int.Parse(values[11]);
        else
        {
            Debug.Log("Quality Setting not found in Game Settings.");
            qualitySetting = 0;

        }
        if (values[12] == "displayMode")
            displayMode = int.Parse(values[13]);
        else
        {
            Debug.Log("Display mode not found in Game Settings.");
            displayMode = 0;
        }
        if (values[14] == "lastLevel")
            // TODO: Remove when we add a continue button.
            currLevel = 1;
            //currLevel = int.Parse(values[15]);
        else
            Debug.Log("Current Level not found in Game Settings.");

        ResetUI();
    }

    private void SaveSettings()
    {
        bool result;
        float vfxVol;
        float musicVol;
        result = masterMixer.GetFloat("VFXVolume", out vfxVol);
        if (!result)
            return;
        result = masterMixer.GetFloat("MusicVolume", out musicVol);
        if (!result)
            return;
        musicVolume = (int)musicVol;
        vfxVolume = (int)vfxVol;
        string text = "masterVolume=" + "10" + ",musicVolume=" + musicVolume + ",vfxVolume=" + vfxVolume + ",sensitivity=" + sensitivity + ",loadTutorial=" + loadTutorial +",quality=" + qualitySetting + ",displayMode=" + displayMode + ",lastLevel=" + currLevel;

        Debug.Log(text);
        File.WriteAllText(Application.dataPath + "/Resources/options.txt", text);
    }
    public void ResetUI()
    {
        BookMenu[] book = Resources.FindObjectsOfTypeAll<BookMenu>() as BookMenu[];
        book[0].Reset(vfxVolume, musicVolume, qualitySetting, displayMode);
    }

/*    void ResetSliders()
    {
        List<String> names = new List<String> { "VFX Slider",
            "Music Slider",
            "Mouse Sensitivity Slider"
        };
        List<float> values = new List<float> { vfxVolume,
            musicVolume,
            sensitivity,
        };
        Slider[] sliders = Resources.FindObjectsOfTypeAll<Slider>() as Slider[];
        foreach (var slider in sliders)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (slider.gameObject.name == names[i])
                {
                    slider.gameObject.SetActive(false);
                    slider.value = values[i];
                    slider.gameObject.SetActive(true);
                    *//*                Debug.Log("Found");*//*
                    return;
                }
            }
        }
    }
    void ResetDropdowns()
    {
        List<String> names = new List<String> { "Quality Settings Dropdown",
            "Display Mode Dropdown"
        };
        List<int> values = new List<int> { qualitySetting,
            displayMode
        };
        foreach (var dropdown in dropdowns)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (dropdown.gameObject.name == names[i])
                {
                    dropdown.gameObject.SetActive(false);
                    dropdown.value = values[i];
                    dropdown.gameObject.SetActive(true);
                    *//*                Debug.Log("Found");*//*
                    return;
                }
            }
        }
    }*/
    public void SetVFXVolume(int value)
    {
        masterMixer.SetFloat("VFXVolume", value);
        vfxVolume = value;
        vfxTestNoise.Play();
        SaveSettings();
    }
    public void SetMusicVolume(int value)
    {
        masterMixer.SetFloat("MusicVolume", value);
        musicVolume = value;
        SaveSettings();
    }

    public void SetMouseSensitivity(float value)
    {
        sensitivity = value;
        SaveSettings();
    }

    public void SetQualitySettings(int value)
    {
        qualitySetting = value;
        SaveSettings();
    }

    public void SetDisplayMode(int value)
    {
        displayMode = value;
        SaveSettings();
    }

    // TODO: TEST IF WORKING
/*    void ResetVFXVolume()
    {
        Slider[] sliders = Resources.FindObjectsOfTypeAll<Slider>() as Slider[];
        foreach (var slider in sliders)
        {
            if (slider.gameObject.name == "VFX Slider")
            {
                slider.gameObject.SetActive(false);
                slider.value = vfxVolume;
                slider.gameObject.SetActive(true);
*//*                Debug.Log("Found");*//*
                return;
            }
        }
    }

    void ResetMusicVolume()
    {
        Slider[] sliders = Resources.FindObjectsOfTypeAll<Slider>() as Slider[];
        foreach (var slider in sliders)
        {
            if (slider.gameObject.name == "Music Slider")
            {
                slider.gameObject.SetActive(false);
                slider.value = musicVolume;
                slider.gameObject.SetActive(true);
                *//*                Debug.Log("Found");*//*
                return;
            }
        }
    }

    void ResetMouseSensitivity()
    {
        Slider[] sliders = Resources.FindObjectsOfTypeAll<Slider>() as Slider[];
        foreach (var slider in sliders)
        {
            if (slider.gameObject.name == "Mouse Sensitivity Slider")
            {
                slider.gameObject.SetActive(false);
                slider.value = sensitivity;
                slider.gameObject.SetActive(true);
                *//*                Debug.Log("Found");*//*
                return;
            }
        }
    }
    void ResetQualitySettings()
    {
        TMPro.TMP_Dropdown[] dropdowns = Resources.FindObjectsOfTypeAll<TMPro.TMP_Dropdown>() as TMPro.TMP_Dropdown[];
        foreach (var dropdown in dropdowns)
        {
            if (dropdown.gameObject.name == "Quality Settings Dropdown")
            {
                dropdown.gameObject.SetActive(false);
                dropdown.value = qualitySetting;
                dropdown.gameObject.SetActive(true);
                *//*                Debug.Log("Found");*//*
                return;
            }
        }
    }

    void ResetDisplayMode()
    {
        TMPro.TMP_Dropdown[] dropdowns = Resources.FindObjectsOfTypeAll<TMPro.TMP_Dropdown>() as TMPro.TMP_Dropdown[];
        foreach (var dropdown in dropdowns)
        {
            if (dropdown.gameObject.name == "Display Mode Dropdown")
            {
                dropdown.gameObject.SetActive(false);
                dropdown.value = displayMode;
                dropdown.gameObject.SetActive(true);
                Debug.Log("Found");
                return;
            }
        }
    }*/
}
