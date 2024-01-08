using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TrapCouch : Interactable
{
    #region fields
    [SerializeField]
    private bool spring;

    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool transition, sitting, trapped;
    [SerializeField]
    float delay = 1.0f;
    [SerializeField]
    private GameObject tv;

    [SerializeField]
    private VideoClip clip;

    [SerializeField]
    private bool trap;
    private bool triggered;

    public bool Trapped
    { 
        get { return trapped; } 
    }
    #endregion

    #region methods
    private void Update()
    {
        if (!trap)
        {
            if (GameManager.Instance.taskManager.AllTasksComplete())
                Text = "Watch TV";
            else
                Text = "I'm too stressed right now.";
        }

        // Trigger get up
        if (Input.GetButtonDown("Interact") && sitting && GameManager.Instance.EnableControls)
        {
            StartCoroutine(UnsetSitting());
        }

        if (transition)
        {
            float speed = 4.0f;
            // Sit down transition
            if (!sitting)
            {
                // Change player orientation
                PlayerController.Instance.transform.rotation = Quaternion.Lerp(PlayerController.Instance.transform.rotation, transform.rotation, speed * Time.deltaTime);
                // Change player position
                PlayerController.Instance.transform.position = Vector3.Lerp(PlayerController.Instance.transform.position, transform.GetChild(0).position, speed * Time.deltaTime);
                // Disable collision
                PlayerController.Instance.GetComponent<Rigidbody>().isKinematic = true;
            }
            // Get up transition
            else
            {
                // Reset
                PlayerController.Instance.transform.rotation = Quaternion.Lerp(PlayerController.Instance.transform.rotation, originalRot, speed * Time.deltaTime);
                PlayerController.Instance.transform.position = Vector3.Lerp(PlayerController.Instance.transform.position, originalPos, speed * Time.deltaTime);
                PlayerController.Instance.GetComponent<Rigidbody>().isKinematic = false;
                PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetBool("Sit", false);
                GameManager.Instance.EnableControls = true;
                GameManager.Instance.EnableCamera = true;
            }
        }
    }

    public override void Action()
    {
        if (!trap && !GameManager.Instance.taskManager.AllTasksComplete())
            return;

        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetBool("Sit", true);
        // Save transform
        originalPos = PlayerController.Instance.transform.position;
        originalRot = PlayerController.Instance.transform.rotation;

        if (spring)
            StartCoroutine(TriggerTrap());
        else
            StartCoroutine(SetSitting());

        if (trap && GameManager.Instance.taskManager.GetTask("Sit and Watch TV").stepsComplete == 0)
        {
            GameManager.Instance.taskManager.UpdateTaskCompletion("Sit and Watch TV");
        }
    }

    IEnumerator Blink()
    {
        GameManager.Instance.EnableControls = false;
        GameManager.Instance.EnableCamera = false;
        yield return new WaitForSeconds(1.5f);
        GameUI.Instance.ReverseBlink();
        yield return new WaitForSeconds(2.4f);
        GameManager.Instance.TransitionDay();
        yield return new WaitForSeconds(2.4f);
        GameManager.Instance.EnableControls = true;
        GameManager.Instance.EnableCamera = true;
        StartCoroutine(UnsetSitting());
    }

    IEnumerator TriggerTrap()
    {
        transition = true;
        PlayerController.Instance.ThrowPlayerInDirection(new Vector3(0.0f, 500.0f, 0.0f), delay, SelectCam.couchCam);
        yield return new WaitForSeconds(delay);
        transition = false;
        // Enable collision
        PlayerController.Instance.GetComponent<Rigidbody>().isKinematic = false;
        // Play animation
        transform.parent.parent.GetComponent<Animator>().SetTrigger("Activate");
        trapped = true;
    }

    IEnumerator SetSitting()
    {
        GameManager.Instance.EnablePause = false;
        transition = true;
        yield return new WaitForSeconds(1.0f);
        transition = false;
        sitting = true;

        if (!tv.transform.GetChild(0).GetComponent<VideoPlayer>().isPlaying)
        {
            tv.transform.GetChild(0).GetComponent<VideoPlayer>().clip = clip;
            tv.transform.GetChild(0).GetComponent<VideoPlayer>().Play();
            PlayerController.Instance.EnableNewCamera(SelectCam.tvCam, 1.5f);
        }

        if (!trap)
        {
            StartCoroutine(Blink());
            GameManager.Instance.EnableControls = false;
            GameManager.Instance.EnableCamera = false;
        }
    }

    IEnumerator UnsetSitting()
    {
        transition = true;
        yield return new WaitForSeconds(1.0f);
        transition = false;
        sitting = false;
        tv.transform.GetChild(0).GetComponent<VideoPlayer>().Stop();
        PlayerController.Instance.ReEnablePlayerCamera(1.0f);

        GameManager.Instance.EnableControls = true;
        GameManager.Instance.EnableCamera = true;
        GameManager.Instance.EnablePause = true;
        Destroy(this);
    }
    #endregion
}
