using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BookMenu : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    TextMeshProUGUI musicVolSlider;
    [SerializeField]
    TextMeshProUGUI sfxVolSlider;
    [SerializeField]
    GameObject qualitySettingsCircle;
    [SerializeField]
    GameObject displaySettingsCircle;

    [SerializeField]
    GameObject[] qualitySettingsObjs;
    [SerializeField]
    GameObject[] displaySettingsObjs;


    [SerializeField]
    string mainMenuSceneName;

    float maxMusicValue = 15;
    float currentMusicValue = 15;
    float maxSFXValue = 15;
    float currentSFXValue = 15;
    private void Awake()
    {
        try { GameSettings.Instance.ResetUI(); } catch { }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void PlayGame()
    {
        GameSettings.Instance.ResetLevel();
        if (GameSettings.Instance.loadTutorial == true)
        {
            GameSettings.Instance.loadTutorial = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Reset(float musicVolume, float sfxVolume, int qualitySetting, int displaySetting)
    {
        if (!musicVolSlider) return;
        currentMusicValue = (musicVolume + 80.0f) / 80.0f * maxMusicValue;
        PassMusicVolume(true);
        if (!sfxVolSlider) return;
        currentSFXValue = (sfxVolume + 80.0f) / 80.0f * maxSFXValue; 
        PassSFXVolume(true);
        if (qualitySettingsObjs.Length == 0) return;
        ChangeQualityLevel(qualitySetting);
        if (displaySettingsObjs.Length == 0) return;
        ChangeScreenSettings(displaySetting);
    }

    void PauseGame()
    {
        GameManager.Instance.PauseGame();
    }

    void ChangeMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    void ExitToMainMenu()
    {
        ChangeScene(mainMenuSceneName);
    }

    void ExitToDesktop()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void ChangeScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch
        {
            Debug.Log("The scene named: '" + sceneName + "' does not exist");
        }
    }

    void IncreaseMusicVol()
    {
        if (currentMusicValue + 1 <= maxMusicValue)
        {
            currentMusicValue++;
            PassMusicVolume();
        }
    }
    void DecreaseMusicVol()
    {
        if (currentMusicValue - 1 >= 0)
        {
            currentMusicValue--;
            PassMusicVolume();
        }
    }

    void PassMusicVolume(bool setSetting = false)
    {
        if (!setSetting)
        {
            int newValue = (int)((currentMusicValue / maxMusicValue * 80.0f) - 80.0f);
            GameSettings.Instance.SetMusicVolume(newValue);
        }
        musicVolSlider.text = "";
        for (int i = 0; i < currentMusicValue; i++)
        {
            musicVolSlider.text += "-";
        }
    }
    void IncreaseSFXVol()
    {
        if (currentSFXValue + 1 <= maxSFXValue)
        {
            currentSFXValue++;
            PassSFXVolume();
        }
    }
    void DecreaseSFXVol()
    {
        if (currentSFXValue - 1 >= 0)
        {
            currentSFXValue--;
            PassSFXVolume();
        }
    }

    void PassSFXVolume(bool setSetting = false)
    {
        if (!setSetting)
        {
            int newValue = (int)((currentSFXValue / maxSFXValue * 80.0f) - 80.0f);
            GameSettings.Instance.SetVFXVolume(newValue);
        }
        sfxVolSlider.text = "";
        for (int i = 0; i < currentSFXValue; i++)
        {
            sfxVolSlider.text += "-";
        }
    }

    void LowQualityLevel()
    {
        ChangeQualityLevel(0);
    }
    void MedQualityLevel()
    {
        ChangeQualityLevel(1);
    }
    void HighQualityLevel()
    {
        ChangeQualityLevel(2);
    }

    public void ChangeQualityLevel(int index)
    {
        qualitySettingsCircle.transform.position = qualitySettingsObjs[index].transform.position;
        GameSettings.Instance.SetQualitySettings(index);
        QualitySettings.SetQualityLevel(index, false);
    }
    void Windowed()
    {
        ChangeScreenSettings(0);
    }
    void Fullscreen()
    {
        ChangeScreenSettings(1);
    }
    void ExclusiveFullscreen()
    {
        ChangeScreenSettings(2);
    }

    public void ChangeScreenSettings(int index)
    {
        displaySettingsCircle.transform.position = displaySettingsObjs[index].transform.position;
        GameSettings.Instance.SetDisplayMode(index);
        if (index == 0)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else if (index == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
    }
}
