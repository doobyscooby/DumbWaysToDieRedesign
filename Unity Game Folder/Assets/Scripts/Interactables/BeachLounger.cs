using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachLounger : Interactable
{
    #region fields
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool transition, sitting;
    #endregion

    #region methods
    private void Update()
    {
        if (GameManager.Instance.taskManager.AllTasksComplete())
            Text = "Sit";
        else
            Text = "I'm too stressed right now.";

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
                PlayerController.Instance.transform.rotation = Quaternion.Lerp(PlayerController.Instance.transform.rotation, transform.rotation * Quaternion.Euler(45.0f, 0.0f, 0.0f), speed * Time.deltaTime);
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
        if (!GameManager.Instance.taskManager.AllTasksComplete())
            return;

        // Disable controls
        GameManager.Instance.EnableControls = false;
        GameManager.Instance.EnableCamera = false;
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetBool("Sit", true);
        // Save transform
        originalPos = PlayerController.Instance.transform.position;
        originalRot = PlayerController.Instance.transform.rotation;

        StartCoroutine(SetSitting());
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

    IEnumerator SetSitting()
    {
        transition = true;
        yield return new WaitForSeconds(1.0f);
        transition = false;
        sitting = true;

        GameManager.Instance.EnableControls = false;
        GameManager.Instance.EnableCamera = false;

        StartCoroutine(Blink());
    }

    IEnumerator UnsetSitting()
    {
        transition = true;
        yield return new WaitForSeconds(1.0f);
        transition = false;
        sitting = false;

        GameManager.Instance.EnableControls = true;
        GameManager.Instance.EnableCamera = true;
        Destroy(this);
    }
    #endregion
}
