using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttachFryingPan : MonoBehaviour
{
    #region fields
    private FryingPanTrap trap;
    private LineRenderer laser;
    #endregion

    #region methods
    private void Awake()
    {
        trap = transform.parent.parent.GetComponent<FryingPanTrap>();
        laser = transform.parent.parent.GetChild(1).GetComponent<LineRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Frying Pan" && !trap.Triggered && !trap.Picked)
        {
            // Play fx
            GetComponent<AudioSource>().Play();
            // Attach
            InteractionSystem.Instance.DropObject();
            Destroy(other.GetComponent<Rigidbody>());
            other.transform.parent = transform.GetChild(0);
            other.transform.localPosition = new Vector3(0.169f, 0.01286649f, 0.567f);
            other.transform.localRotation = Quaternion.Euler(new Vector3(90.0f, 0.0f, 90.0f));
            other.transform.localScale = new Vector3(1.126011f, 0.7271625f, 1.126011f);
            other.GetComponent<Interactable>().CanInteract = false;
            // Enable
            trap.FryingPan = other.gameObject;
            trap.enabled = true;
            laser.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Frying Pan" && trap.Picked)
        {
            trap.Picked = false;
        }
    }
    #endregion
}
