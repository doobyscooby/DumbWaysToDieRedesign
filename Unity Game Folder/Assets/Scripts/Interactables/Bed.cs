using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bed : Interactable
{
    #region fields

    public Vector3 bedPosition;
    public Vector3 bedRotation;
    #endregion

    #region methods

    private void Update()
    {
        if (GameManager.Instance.taskManager.CurrentTasks == GameManager.Instance.taskManager.afterTransitionTasks && GameManager.Instance.taskManager.AllTasksComplete())
            Text = "Sleep";
        else
            Text = "It's too early to sleep";
    }

    public override void Action()
    {

        if (GameManager.Instance.taskManager.CurrentTasks == GameManager.Instance.taskManager.afterTransitionTasks && GameManager.Instance.taskManager.AllTasksComplete())
        {
            GameUI.Instance.ReverseBlink();
            Camera.main.GetComponent<CameraController>().FollowHeadTime = 0.0f;
            PlayerController.Instance.transform.position = bedPosition;
            PlayerController.Instance.transform.rotation = Quaternion.Euler(bedRotation);
            GameManager.Instance.EnableCamera = false;
            GameManager.Instance.EnableControls = false;
            PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Sleep");
            StartCoroutine(GoToSleep());
            CanInteract = false;
        }
    }


    IEnumerator GoToSleep()
    {
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.taskManager.ResetAllTraps();
        // TODO: FIX this!!!
        // Advance level
        GameManager.Instance.MoveToNextLevel();
    }
    #endregion
}
