using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Animator notifyAnim;
    [SerializeField]
    private Image dotImage;
    private TextMeshProUGUI interactText;
    private Animator dotAnim;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField]
    private Animator blinkAnim;


    public static GameUI Instance;
    #endregion

    #region properties
    public Animator DotAnim
    {
        get { return dotAnim; }
    }

    public Animator NotifyAnim
    {
        get { return notifyAnim; }
    }

    public TextMeshProUGUI InteractText
    {
        get { return interactText; }
    }

    public TextMeshProUGUI DialogueText
    {
        get { return dialogueText; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (Instance != this)
            Destroy(gameObject);

        dotAnim = dotImage.GetComponent<Animator>();
        interactText = dotImage.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ReverseBlink()
    {
        blinkAnim.GetComponent<Animator>().SetTrigger("Fade");
    }

    public void QuitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ChangeScene(string scene)
    {
        Time.timeScale = 1.0f;
        try
        {
            SceneManager.LoadScene(scene);
        }
        catch
        {
            Debug.Log("The scene named: '" + scene + "' does not exist");
        }
    }

    #endregion
}
