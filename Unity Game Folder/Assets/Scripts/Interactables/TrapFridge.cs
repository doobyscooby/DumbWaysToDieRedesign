using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class TrapFridge : Interactable
{
    #region fields
    [SerializeField]
    private VisualEffect vfx;
    [SerializeField]
    private bool sleep;
    #endregion

    #region methods
    public override void Action()
    {
        // Ignore fridge objects
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        transform.GetChild(1).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Cast ray to see if player will be hit
        RaycastHit hit;
        if (Physics.BoxCast(transform.GetChild(0).GetChild(0).transform.position, new Vector3(0.5f, 0.2f, 0.6f), Vector3.right, out hit, Quaternion.identity, 2f))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                StartCoroutine(TriggerTrap());
            else
                GetComponent<Animator>().SetTrigger("Activate");
        }
        else
            GetComponent<Animator>().SetTrigger("Activate");
    }

    IEnumerator TriggerTrap()
    {
        // Disable controls
        GameManager.Instance.EnableControls = false;
        // Reset player velocity and animation
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetFloat("dirX", 0);
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetFloat("dirY", 0);
        float delay = .3f;
        PlayerController.Instance.ThrowPlayerInDirection(new Vector3(100, 10, 0), delay, SelectCam.fridgeCam);
        yield return new WaitForSeconds(delay * 1.2f);
        GetComponent<Animator>().SetTrigger("Activate");
        GetComponent<AudioSource>().Play();
        vfx.Play();
        if (sleep)
        {
            yield return new WaitForSeconds(4.0f);
            // Advance level
            GameManager.Instance.MoveToNextLevel();
        }
    }
    #endregion
}
