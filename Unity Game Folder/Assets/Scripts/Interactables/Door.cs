using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField]
    PivotSettings openingForward;
    [SerializeField]
    PivotSettings openingBackward;

    private void Start()
    {
        StartCoroutine(fixDoors());
    }

    IEnumerator fixDoors()
    {
        yield return new WaitForSeconds(0.1f);
        if (openingForward.open == true)
        {
            openingBackward.startingPos = openingForward.endingPos;
            openingBackward.startingAngle = openingForward.endingAngle;

            Vector3 startingPos = openingForward.startingPos;
            openingForward.startingPos = openingForward.endingPos;
            openingForward.endingPos = startingPos;

            Vector3 startingAngle = openingForward.startingAngle;
            openingForward.startingAngle = openingForward.endingAngle;
            openingForward.endingAngle = startingAngle;
        }
        else if (openingBackward.open == true)
        {
            openingForward.startingPos = openingBackward.endingPos;
            openingForward.startingAngle = openingBackward.endingAngle;

            Vector3 startingPos = openingBackward.startingPos;
            openingBackward.startingPos = openingBackward.endingPos;
            openingBackward.endingPos = startingPos;

            Vector3 startingAngle = openingBackward.startingAngle;
            openingBackward.startingAngle = openingBackward.endingAngle;
            openingBackward.endingAngle = startingAngle;
        }
    }
    public override void Action()
    {
        // Something is moving
       if (openingBackward.inUse || openingForward.inUse)
        {
            return;
        }
        else if (openingForward.open == true)
        {
            OpenForward();
            return;
        }
        else if (openingBackward.open == true)
        {
            OpenBackward();
            return;
        }

        float dot = Quaternion.Dot(transform.parent.rotation.normalized, PlayerController.Instance.transform.rotation.normalized);
        // Infront of player
        if (dot > -0.7f  && dot < 0.7f)
        {
            OpenForward();
        }
        // Behind Player
        else
        {
            OpenBackward();
        }
    }

    public void ResetDoor()
    {
        if (openingBackward.inUse || openingForward.inUse)
        {
            return;
        }
        else if (openingForward.open == true)
        {
            OpenForward();
            return;
        }
        else if (openingBackward.open == true)
        {
            OpenBackward();
            return;
        }
    }

    private void OpenForward()
    {
        StartCoroutine(PivotObjectEnumerator(openingForward));
    }

    private void OpenBackward()
    {
        StartCoroutine(PivotObjectEnumerator(openingBackward));
    }


    IEnumerator PivotObjectEnumerator(PivotSettings pivotSettings)
    {
        // If object is in use, Ignores
        if (pivotSettings.inUse == true)
        {
            yield break;
        }

        pivotSettings.open = !pivotSettings.open;
        // Setting up values for object
        pivotSettings.inUse = true;
        bool usingMovement = pivotSettings.usingMovement;
        if (pivotSettings.open)
            Text = "Close";
        else
            Text = "Open";

        Quaternion startingAngle;
        Quaternion endingAngle;
        Vector3 startingPos;
        Vector3 endingPos;
        if (pivotSettings.open == true)
        {
            startingAngle = Quaternion.Euler(pivotSettings.startingAngle);
            endingAngle = Quaternion.Euler(pivotSettings.endingAngle.x, pivotSettings.endingAngle.y, pivotSettings.endingAngle.z);
            startingPos = pivotSettings.startingPos;
            endingPos = pivotSettings.endingPos;
            //AudioManager.Instance.PlayAudio("DoorOpen");
        }
        else
        {
            endingAngle = Quaternion.Euler(pivotSettings.startingAngle);
            startingAngle = Quaternion.Euler(pivotSettings.endingAngle.x, pivotSettings.endingAngle.y, pivotSettings.endingAngle.z);
            endingPos = pivotSettings.startingPos;
            startingPos = pivotSettings.endingPos;
            //AudioManager.Instance.PlayAudio("DoorClose");
        }

        float speed = 1 / pivotSettings.speed;
        for (float i = 0; i < speed; i+=Time.deltaTime)
        {
            if (usingMovement)
            {
                transform.localPosition = Vector3.Lerp(startingPos, endingPos, i / speed);
            }

            transform.localRotation = Quaternion.Lerp(startingAngle, endingAngle, i / speed);
            yield return new WaitForFixedUpdate();
        }

        pivotSettings.inUse = false;
    }
}
