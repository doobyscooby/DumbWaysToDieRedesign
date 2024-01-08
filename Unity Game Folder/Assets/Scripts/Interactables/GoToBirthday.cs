using System.Collections;
using UnityEngine;

public class GoToBirthday : Interactable
{
    #region fields
    [SerializeField]
    Door doorToClose;
    [SerializeField]
    private GameObject cutscene;
    #endregion

    #region methods

    private void Update()
    {
        if (GameManager.Instance.taskManager.AllTasksComplete())
            Text = "Leave For Birthday";
        else
            Text = "It's not time to leave yet.";
    }

    public override void Action()
    {
        if (GameManager.Instance.taskManager.AllTasksComplete())
        {
            // TODO: Add ReverseBlink animation equivalent for leaving to work.
            StartCoroutine(LeaveForBirthday());
            cutscene.SetActive(true);
        }
    }

    IEnumerator LeaveForBirthday()
    {
        GameManager.Instance.EnableControls = false;
        GameManager.Instance.EnableCamera = false;
        Vector3 currentPosition = PlayerController.Instance.transform.position;
        GameUI.Instance.ReverseBlink();
        yield return new WaitForSeconds(2.4f);
        // TODO: Add Blink animation equivalent for coming home.
        Transform pcTransform = PlayerController.Instance.transform;
        pcTransform.rotation = Quaternion.Euler(pcTransform.rotation.x, 90, pcTransform.rotation.z);
        GameManager.Instance.TransitionDay();
        yield return new WaitForSeconds(2.4f);
        GameManager.Instance.EnableControls = true;
        GameManager.Instance.EnableCamera = true;
        doorToClose.ResetDoor();
    }
    #endregion
}
