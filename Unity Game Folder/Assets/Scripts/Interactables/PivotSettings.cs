using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotSettings : MonoBehaviour
{
    [HideInInspector]
    public bool inUse = false;
    [HideInInspector]
    public Vector3 startingPos;
    [HideInInspector]
    public Vector3 startingAngle;

    public Vector3 endingPos;

    public Vector3 endingAngle;

    public bool usingMovement = false;

    public float openSpeed = 1.75f;

    public bool open = false;

    public float speed { get => openSpeed; }

    private void Start()
    {
        startingAngle = transform.localRotation.eulerAngles;
        startingPos = transform.localPosition;
    }
}
