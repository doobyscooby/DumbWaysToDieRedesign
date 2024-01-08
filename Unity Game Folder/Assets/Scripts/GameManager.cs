using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region fields
    // Managers
    public static GameManager Instance;
    public TaskManager taskManager;
    GameSettings settings;

    // Set in inspector Dependencies
    [SerializeField]
    private GameUI gameUI;

    [SerializeField]
    private PlayMusic music;

    // Instatiating variables.
    private bool enableControls = false;
    private bool enableCamera = false;
    private bool isPaused;
    private bool hasTransitioned = false;
    private bool canPause = true;
    #endregion

    #region properties
    public bool EnableControls
    {
        get { return enableControls; }
        set { enableControls = value; }
    }
    public bool EnableCamera
    {
        get { return enableCamera; }
        set { enableCamera = value; }
    }
    public bool EnablePause
    {
        get { return canPause; }
        set { canPause = value; }
    }
    public bool IsPaused
    {
        get { return isPaused; }
    }
    public bool HasTransitioned
    {
        get { return hasTransitioned; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        // GameManager Instance
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Get Managers
        try
        {
            taskManager = GetComponent<TaskManager>();
        }
        catch
        {
            throw new Exception("Could not find TaskManager");
        }
        settings = GameObject.FindObjectOfType<GameSettings>();
        if (settings == null)
            throw new Exception("Could not find TaskManager");

        // InitTasks
        InitTasks();

        StartCoroutine(EnablePlayer());
    }

    /// <summary>
    ///  Inits Tasks for the day.
    /// </summary>
    private void InitTasks()
    {
        taskManager.SetUp();
        Task[] tempTasks = taskManager.CurrentTasks;
        if(!tempTasks[0])
            throw new Exception("Failed to Initialize Tasks");
        //Debug.Log("Breakfast task is: " + tempTasks[0].name + ", Midday Task is: " + tempTasks[1].name + ", Final Task is: " + tempTasks[2].name);
    }

    /// <summary>
    ///  Sets game state equal to false and activates game ui.
    /// </summary>
    public void PauseGame()
    {
        if (!canPause)
            return;
        isPaused = !isPaused;

        // Paused
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            enableControls = false;
            EnableCamera = false;
        }
        // Unpaused
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            enableControls = true;
            EnableCamera = true;
        }

        // Trigger book animation
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetBool("Book", IsPaused);
    }

    public void MoveToNextLevel()
    {
        GameSettings.Instance.currLevel++;
        SceneManager.LoadScene("Loading");
    }
    /// <summary>
    ///  Restarts current scene.
    /// </summary>
    public void Restart()
    {
        GameManager.Instance.taskManager.ResetAllTraps();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator EnablePlayer()
    {
        yield return new WaitForSeconds(3.3f);
        enableControls = true;
        enableCamera = true;
    }

    public void TransitionDay()
    {
        taskManager.SwapTasksOver();
        hasTransitioned = true;

        StartCoroutine(PlayerController.Instance.OpenNotepadAfterAwake());
        music.TransitionDay();
        AudioManager.Instance.PlayAudio("Yawn");
    }
    public void StopMusic()
    {
        music.StopAll();
    }
    public void SwapMusic()
    {
        music.SwapMusic();
    }
    #endregion
}