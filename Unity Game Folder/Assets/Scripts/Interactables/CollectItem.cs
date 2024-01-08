using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : Interactable
{
    public enum ObjectsToPickup
    {
        CarKeys = 0,
        Pills = 1,
        Rubbish = 2
    };

    [SerializeField]
    ObjectsToPickup objectToPickup;

    [SerializeField]
    AudioSource associatedAudio;
    private void Update()
    {
        if (GameManager.Instance.taskManager.CurrentTasks == GameManager.Instance.taskManager.afterTransitionTasks)
            Text = "Take Pill";
        else
            Text = "I need to take these before bed.";
    }
    public override void Action()
    {
        if (GameManager.Instance.taskManager.CurrentTasks != GameManager.Instance.taskManager.afterTransitionTasks)
            return;
        if (associatedAudio != null)
            associatedAudio.Play();
        switch (objectToPickup)
        {
            case ObjectsToPickup.CarKeys:
                GameManager.Instance.taskManager.UpdateTaskCompletion("Find Car Keys");
                break;
            case ObjectsToPickup.Pills:
                if (GameManager.Instance.taskManager.GetTask("Take Meds").stepsComplete < 1)
                    GameManager.Instance.taskManager.UpdateTaskCompletion("Take Meds");
                break;
            case ObjectsToPickup.Rubbish:
                GameManager.Instance.taskManager.UpdateTaskCompletion("Clean Kitchen");
                break;
            default:
                Debug.Log("This Object does not exist and cannot be picked up");
                break;
        }
        Destroy(gameObject);
    }
}
