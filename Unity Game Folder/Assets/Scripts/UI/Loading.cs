using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject loadingUI, continueUI;
    AsyncOperation asyncLoad;

    private bool loaded;
    #endregion

    #region methods
    private void Awake()
    {
        Debug.Log(GameSettings.Instance.currLevel);
        string scene;
        switch (GameSettings.Instance.currLevel)
        {
            case 1:
                scene = "Level 1";
                loadingUI.GetComponent<TextMeshProUGUI>().text = "LOADING...";
                break;
            case 2:
                scene = "Level 2";
                break;
            case 3:
                scene = "Level 3";
                break;
            case 4:
                scene = "Level 4";
                break;
            case 5:
                scene = "Level 5";
                break;
            case 6:
                scene = "End Scene";
                loadingUI.GetComponent<TextMeshProUGUI>().text = "LOADING...";
                break;
            default:
                scene = "Main Menu";
                loadingUI.GetComponent<TextMeshProUGUI>().text = "LOADING...";
                break;
        }

        StartCoroutine(waitForProgram(scene));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && loaded)
        {
            asyncLoad.allowSceneActivation = true;
        }
    }

    IEnumerator waitForProgram(string scene)
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(LoadAsyncScene(scene));
    }

    IEnumerator LoadAsyncScene(string scene)
    {
        asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;
        yield return (asyncLoad.progress > 0.9f);
        yield return new WaitForSeconds(5);
        loaded = true;
        loadingUI.SetActive(false);
        continueUI.SetActive(true);
    }
    #endregion
}
