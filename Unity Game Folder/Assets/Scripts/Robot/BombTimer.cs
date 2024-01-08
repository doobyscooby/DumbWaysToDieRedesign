using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class BombTimer : MonoBehaviour
{
    #region fields
    [SerializeField]
    private float startingTimeInSeconds;
    private float currentTime;
    [SerializeField]
    private TextMeshPro timerText;
    [SerializeField]
    private VideoPlayer video;
    [SerializeField]
    private Animator bombAnimator;

    private bool ended;
    #endregion

    #region properties
    public float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value; }
    }
    public bool Ended
    {
        get { return ended; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        currentTime = startingTimeInSeconds;
    }

    public void StartTimer()
    {
        StartCoroutine(Countdown());
    }

    public void StopTimer()
    {
        StopCoroutine(Countdown());
        Destroy(this);
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator Countdown()
    {

        // Reduce time
        while (currentTime > 0.0f)
        {
            if (!GameManager.Instance.IsPaused)
            {
                Debug.Log("Something anything");
                currentTime -= Time.deltaTime;
                if (currentTime < 0.0f)
                    currentTime = 0.0f;
                DisplayTime(currentTime);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        // Timer finished
        DisplayTime(currentTime);
        ended = true;
        PlayNukeScene();
        yield return null;
    }

    private void PlayNukeScene()
    {
        GameManager.Instance.EnableControls = false;
        GameManager.Instance.EnableCamera = false;

        float delay = 4.0f;
        PlayerController.Instance.EnableNewCamera(SelectCam.outsideCam, delay);
        StartCoroutine(EnableVideo(delay));
    }

    IEnumerator EnableVideo(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Play Anim
        bombAnimator.SetTrigger("Fire");
        Vector3 direction = PlayerController.Instance.transform.position - transform.position;
        direction = direction.normalized;

        // Prevent additional death from collision
        PlayerController.Instance.DisableDeathFromCollision(100.0f);

        yield return new WaitForSeconds(1.0f);
        PlayerController.Instance.EnableRagdoll();
        PlayerController.Instance.AddRagdollForce(direction * 150.0f + new Vector3(0, 10.0f, 0));
        yield return new WaitForSeconds(1.0f);

        video.enabled = true;
        GameManager.Instance.StopMusic();
        GameManager.Instance.EnableCamera = false;
        GameManager.Instance.EnableControls = false;

        video.loopPointReached += NukeVideoFinished;
    }

    void NukeVideoFinished(VideoPlayer vp)
    {
        vp.enabled = false;
        GameManager.Instance.Restart();
    }
    #endregion
}
