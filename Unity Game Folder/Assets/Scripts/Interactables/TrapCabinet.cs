using System;
using System.Collections;
using UnityEngine;

public class TrapCabinet : Interactable
{
    #region fields
    private bool cut;

    private Animator anim;

    public bool kills = false;

    [SerializeField]
    float delay = 0.5f;

    [SerializeField]
    LayerMask playerLayer;
    #endregion

    #region methods
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Text != "Cut" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Scissors")
            Text = "Cut";
        else if (Text != "Open")
            Text = "Open";
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Cut rope
        if (collision.transform.name == "Scissors" && !cut)
        {
            anim.SetTrigger("Cut");
            AudioManager.Instance.PlayAudio("Cut");
            cut = true;
        }
    }
    private void OnDrawGizmos()
    {
        // Show Shotgun Hitbox
        // Gizmos.DrawCube(transform.GetChild(1).position, new Vector3(7, .7f, .7f));
    }
    public override void Action()
    {
        if (InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Scissors")
            return;
        // Open and shoot if not cut
        if (cut)
            anim.SetTrigger("Open");
        else if (!cut && kills)
        {
            if (Physics.CheckBox(transform.GetChild(1).position, new Vector3(7, .7f, .7f), Quaternion.identity, playerLayer))
            {
                StartCoroutine(TriggerTrap());

                anim.SetTrigger("Shoot Smoke");

                GetComponent<Collider>().enabled = false;
                return;
            }
            StartCoroutine(TriggerUntrapped());
            anim.SetTrigger("Shoot Smoke");
        }
        else if (!cut && !kills)
        {
            anim.SetTrigger("Shoot Confetti");
        }

        GetComponent<Collider>().enabled = false;
    }

    IEnumerator TriggerTrap()
    {
        // Reset player velocity and animation
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetFloat("dirX", 0);
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetFloat("dirY", 0);

        // Throw Player
        PlayerController.Instance.ThrowPlayerInDirection(new Vector3(100, 10, 0), delay, SelectCam.bathroomCam);

        // Wait before playing noise.
        yield return new WaitForSeconds(delay);
        GetComponent<AudioSource>().Play();
    }
    IEnumerator TriggerUntrapped()
    {
        yield return new WaitForSeconds(delay);
        GetComponent<AudioSource>().enabled = true;
        GetComponent<AudioSource>().Play();
    }
    #endregion
}
