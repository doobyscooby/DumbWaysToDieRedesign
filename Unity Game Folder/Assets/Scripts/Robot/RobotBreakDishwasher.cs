using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBreakDishwasher : MonoBehaviour
{
    #region fields
    [SerializeField]
    private RobotLaserDetection laserDetection;
    [SerializeField]
    private GameObject smoke;
    [SerializeField]
    private RobotBearTrap robotBearTrap;
    private RobotAgent robot;
    #endregion

    #region methods
    private void Awake()
    {
        robot = robotBearTrap.transform.root.GetComponent<RobotAgent>();
    }

    Interactable fryingPan;
    Rigidbody fryingPanRb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Contains("Axe"))
        {
            if (robot.FryingPan != null)
            {
                fryingPan = robot.FryingPan.AddComponent<Interactable>();
                fryingPanRb = robot.FryingPan.AddComponent<Rigidbody>();
                fryingPanRb.isKinematic = true;
            }
            // Reset animation
            transform.root.GetChild(0).GetComponent<Animator>().SetBool("Defend Back", false);

            // Disable scripts
            if (laserDetection)
            {
                laserDetection.ChangeRed();
                laserDetection.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
                Destroy(laserDetection);
            }
            robotBearTrap.StopAllCoroutines();
            robot.BearTrap = null;
            Destroy(robotBearTrap);
            robot.DisableMovement();
            robot.CheckDeath();

            GameManager.Instance.taskManager.UpdateTaskCompletion("Defeat Robot");

            // Enable smoke
            smoke.gameObject.SetActive(true);
            GetComponent<Collider>().enabled = false;
            StartCoroutine(CheckForPickup());
        }
    }

    IEnumerator CheckForPickup()
    {
        if (fryingPan == null)
            yield break;
        bool found = false;
        while (!found)
        {
            if (fryingPan.Interacting == true)
            {
                found = true;
            }
            else
                yield return new WaitForFixedUpdate();
        }
        robot.TriggerStun();
        fryingPanRb.isKinematic = false;
    }
    #endregion
}
