using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        try
        {
            GameSettings.Instance.ResetUI();
        }
        catch { }
    }

    public void Continue()
    {
        if (GameSettings.Instance.loadTutorial == true)
        {
            GameSettings.Instance.loadTutorial = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame()
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

    public void ChangeTutorialOption()
    {
        GameSettings.Instance.loadTutorial = !GameSettings.Instance.loadTutorial;
    }

    public void Quit()
    {
        Debug.Log("Quit");
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SetQualityLevel(int index)
    {
        GameSettings.Instance.SetQualitySettings(index);
        QualitySettings.SetQualityLevel(index, false);
    }

    public void SetVFXVolume(Slider slider)
    {
        if (slider.value != GameSettings.Instance.vfxVolume && slider.value != 0)
        {
            GameSettings.Instance.SetVFXVolume((int)slider.value);
        }
    }
    public void SetMusicVolume(Slider slider)
    {
        if (slider.value != 0)
            GameSettings.Instance.SetMusicVolume((int)slider.value);
    }
    public void SetMouseSensitivity(Slider slider)
    {
        if (slider.value != 0.5f)
        {
            GameSettings.Instance.SetMouseSensitivity(slider.value);
        }
    }

    public void ChangeScreenSettings(TMP_Dropdown dropdown)
    {
        GameSettings.Instance.SetDisplayMode(dropdown.value);
        if (dropdown.value == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (dropdown.value == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }
}
