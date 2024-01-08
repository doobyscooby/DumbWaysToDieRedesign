using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TrapBBQ : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject lighter;
    [SerializeField]
    private GameObject gasBottle;
    [SerializeField]
    private GameObject[] chickens;
    [SerializeField]
    private VisualEffect explosionVFX;
    private BoxCollider boxCollider;
    private bool connected, lit;
    #endregion

    #region methods
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Connect()
    {
        boxCollider.center = new Vector3(-0.08164346f, 0.5073186f, -0.5437614f);
        boxCollider.size = new Vector3(0.433622f, 0.06625993f, 0.2780444f);
        connected = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!lit)
        {
            if (other.gameObject == lighter)
            {
                if (connected)
                {
                    GameManager.Instance.taskManager.UpdateTaskCompletion("Make BBQ");
                    GetComponent<Animator>().SetBool("Light", true);
                    foreach (GameObject chicken in chickens)
                    {
                        chicken.GetComponent<BBQChicken>().CanInteract = true;
                        chicken.GetComponent<BBQChicken>().Text = "Flip";
                    }
                }
                else
                {
                    StartCoroutine(KillPlayer());
                }

                lit = true;
            }
        }
    }

    IEnumerator KillPlayer()
    {
        // Disable
        GameManager.Instance.EnableControls = false;
        GameManager.Instance.EnableCamera = false;
        PlayerController.Instance.Die(0.5f, true, SelectCam.bathroomCam);
        yield return new WaitForSeconds(0.5f);
        // Disable Gas
        gasBottle.GetComponent<BBQWire>().GasVFX.GetComponent<AudioSource>().Stop();
        gasBottle.SetActive(false);
        // Explode
        explosionVFX.Play();
        explosionVFX.GetComponent<AudioSource>().Play();
        // Add backwards force
        PlayerController.Instance.AddRagdollForce(PlayerController.Instance.transform.forward * -20.0f + PlayerController.Instance.transform.up * 10.0f);
    }
    #endregion
}
