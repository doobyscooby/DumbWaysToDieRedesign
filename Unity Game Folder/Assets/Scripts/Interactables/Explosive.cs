using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explosive : Interactable
{
    #region fields
    [SerializeField]
    private LayerMask breakLayer;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask barricadeLayer;
    [SerializeField]
    private float sphereDistance;
    [SerializeField]
    private GameObject rootBone;
    [SerializeField]
    private RobotPunchingGlove punchingGlove;

    // Respawn
    [SerializeField]
    private bool respawnable = true;
    private Vector3 initPosition;
    private Quaternion initRotation;


    private RobotAgent robot;
    #endregion

    #region methods
    private void Awake()
    {
        initPosition = transform.position;
        initRotation = transform.rotation;
        if (punchingGlove != null)
            robot = punchingGlove.transform.root.GetComponent<RobotAgent>();
        StartCoroutine(ActivateExplosive());
    }

    IEnumerator ActivateExplosive()
    {
        while (!Interacting)
        {
            yield return new WaitForFixedUpdate();
        }
        while (Interacting)
        {
            yield return new WaitForFixedUpdate();
        }

        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GetComponent<BoxCollider>().isTrigger)
            return;

        // Deflect if frying pan enabled
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !robot.Dead && robot.FryingPan)
        {
            StartCoroutine(Deflect());
            return;
        }
        Instantiate(gameObject, initPosition, initRotation, transform.parent).GetComponent<BoxCollider>().isTrigger = false;

        // Play fx
        GetComponent<VisualEffect>().Play();
        GetComponent<AudioSource>().Play();

        // Disable
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(GetComponent<Rigidbody>());
        Destroy(gameObject, 1.0f);

        // Check player collision
        if (Physics.CheckSphere(transform.position, sphereDistance, playerLayer))
        {
            Vector3 dir = (PlayerController.Instance.transform.position - transform.position).normalized;
            PlayerController.Instance.ThrowPlayerInDirection(dir, 0.2f);
            return;
        }
        // Check robot collision
        if (Physics.CheckSphere(transform.position, sphereDistance, breakLayer) && !robot.Dead)
        {
            if (!robot.FryingPan)
            {
                try
                {
                    BreakPunchingGlove();
                }
                catch { }
            }
            else
            {
                robot.transform.GetChild(0).GetComponent<Animator>().SetBool("Defend Front", true);
            }
        }
        // Check barricade collision
        if (Physics.CheckSphere(transform.position, sphereDistance, barricadeLayer))
        {
            Collider[] objects = Physics.OverlapSphere(transform.position, 3.0f, barricadeLayer);
            foreach (Collider h in objects)
            {
                if (h.transform.name == "Metal Barricade")
                {
                    Destroy(h.GetComponent<Rigidbody>());
                    Destroy(h);
                }
                Rigidbody r = h.GetComponent<Rigidbody>();
                if (r != null)
                {
                    r.isKinematic = false;
                    r.AddExplosionForce(200.0f, transform.position, 3.0f);
                    Destroy(r.gameObject, 3.0f);
                }
            }
        }
    }

    private void BreakPunchingGlove()
    {
        // Remove
        rootBone.transform.parent = null;
        robot.PunchingGlove = null;
        Destroy(punchingGlove);
        robot.CheckDeath();
        // Drop
        rootBone.AddComponent<Rigidbody>();
        rootBone.AddComponent<Interactable>();
        GameManager.Instance.taskManager.UpdateTaskCompletion("Defeat Robot");
    }

    IEnumerator Deflect()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 force = (transform.forward * -2000.0f + transform.up * 1000.0f) * Time.deltaTime;
        yield return new WaitForFixedUpdate();
        force = (transform.forward * -2000.0f + transform.up * 1000.0f) * Time.deltaTime;
        GetComponent<Rigidbody>().AddForce(force);
        robot.FryingPan.GetComponent<AudioSource>().Play();
    }
    #endregion
}