using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPanTrap : MonoBehaviour
{
    #region fields
    private GameObject fryingPan;
    private LineRenderer laser;
    private bool triggered, picked;

    [SerializeField]
    private bool attachement;
    [SerializeField]
    private AudioSource missSFX;
    #endregion

    #region properties
    public GameObject FryingPan
    {
        set { fryingPan = value; }
    }
    public bool Triggered
    {
        get { return triggered; }
    }
    public bool Picked
    {
        get { return picked; }
        set { picked = value; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        if (!attachement)
        {
            fryingPan = transform.GetChild(0).GetChild(0).gameObject;
            fryingPan.GetComponent<Interactable>().CanInteract = false;
        }
        laser = transform.GetChild(1).GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (isActiveAndEnabled && attachement)
        {
            if (triggered && fryingPan.GetComponent<Interactable>().Interacting)
            {
                GetComponent<Animator>().SetBool("Trigger", false);
                fryingPan = null;
                triggered = false;
                picked = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActiveAndEnabled && fryingPan)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !triggered)
            {
                TriggerPlayer();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !triggered)
            {
                TriggerRobot(other.GetComponent<RobotAgent>());
            }
            else if (!triggered)
            {
                Trigger();
            }
        }
    }

    private void Trigger()
    {
        triggered = true;
        GetComponent<Animator>().SetBool("Trigger", true);
        missSFX.Play();

        fryingPan.GetComponent<Interactable>().CanInteract = true;
        laser.enabled = false;
    }

    private void TriggerPlayer()
    {
        // Play fx
        GetComponent<AudioSource>().Play();
        // Throw player
        PlayerController.Instance.DisableDeathFromCollision(5.0f);
        PlayerController.Instance.ThrowPlayerInRelativeDirection(50f, Direction.backwards, 2.0f, true);

        Trigger();
    }

    private void TriggerRobot(RobotAgent robot)
    {
        // Play fx
        GetComponent<AudioSource>().Play();
        // Stun robot
        robot.TriggerStun();
        // Drop pan
        StartCoroutine(FryingPanForce());

        Trigger();
    }

    IEnumerator FryingPanForce()
    {
        yield return new WaitForSeconds(0.3f);
        fryingPan.transform.parent = null;
        fryingPan.AddComponent<Rigidbody>();

        Vector3 force = (fryingPan.transform.forward * -10000.0f + fryingPan.transform.up * 10000.0f) * Time.deltaTime;
        yield return new WaitForFixedUpdate();
        force = (fryingPan.transform.forward * -10000.0f + fryingPan.transform.up * 10000.0f) * Time.deltaTime;
        fryingPan.GetComponent<Rigidbody>().AddForce(force);

        GetComponent<Animator>().SetBool("Trigger", false);
    }
    #endregion
}
