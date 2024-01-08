using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeButton : Interactable
{
    [SerializeField]
    private GameObject lights;
    [SerializeField]
    private RobotAgent robot;

    [SerializeField]
    private Animator bombAnimator;
    private Animator buttonAnimator;

    [SerializeField]
    private BombTimer timer;

    [SerializeField]
    Transform[] barricadeParents;

    private void Awake()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    public void DisableLights()
    {
        lights.SetActive(false);
    }
    public override void Action()
    {
        GameManager.Instance.SwapMusic();
        lights.SetActive(true);
        buttonAnimator.SetTrigger("activate");
        bombAnimator.SetTrigger("Open");
        StartCoroutine(ActivateRobot());
        StartCoroutine(ActivateTimer());
        GameManager.Instance.taskManager.UpdateTaskCompletion("Do Not Press");
        GameManager.Instance.taskManager.SwapTasksOver(false);
        if (!PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().GetBool("Notepad"))
        {
            StartCoroutine(PlayerController.Instance.UseNotepad());
            GameManager.Instance.taskManager.FindNotepadText();
            StartCoroutine(PlayerController.Instance.UseNotepad());
        }
        else
            GameManager.Instance.taskManager.FindNotepadText();
        GetComponent<Collider>().enabled = false;
        CanInteract = false;
    }

    IEnumerator ActivateRobot()
    {
        yield return new WaitForSeconds(3.0f);
        foreach (Transform boxParent in barricadeParents)
        {
            if (boxParent.transform.name == "Sample Barricade")
            {
                for (int i = 0; i < boxParent.childCount; i++)
                {
                    boxParent.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            else
            {
                boxParent.gameObject.layer = LayerMask.NameToLayer("Barricade");
                for (int i = 0; i < boxParent.childCount; i++)
                {
                    boxParent.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Barricade");
                }
            }
        }
        robot.Activate();
        Destroy(this);
    }

    IEnumerator ActivateTimer()
    {
        yield return new WaitForSeconds(1.3f);
        timer.StartTimer();
    }
}
