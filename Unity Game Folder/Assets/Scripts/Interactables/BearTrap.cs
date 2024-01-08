using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    #region fields
    private bool triggered;
    #endregion

    #region methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            return;

        if (!triggered)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                StartCoroutine(TrapPlayer(other.gameObject));
            }
            else if (other.transform.name == "Cut Teddy")
            {
                StartCoroutine(TrapBear(other.gameObject));
            }
            else if (other.gameObject.layer != LayerMask.NameToLayer("Trapped"))
            {
                // Return if object is pivot interactable
                if (other.GetComponent<Interactable>() && other.GetComponent<Interactable>().Type == Interactable.InteractableType.Pivot)
                    return;
                StartCoroutine(TrapObject(other.gameObject));
            }
        }
    }

    IEnumerator TrapPlayer(GameObject player)
    {
        // Trigger
        GetComponent<Animator>().SetTrigger("Trigger");
        triggered = true;

        // Disable
        GameManager.Instance.EnableControls = false;
        player.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Camera.main.GetComponent<CameraController>().FollowHeadTime = 0.0f;
        player.GetComponent<Animator>().SetBool("Crouching", true);

        yield return new WaitForSeconds(3f);

        // Enable
        GameManager.Instance.EnableControls = true;
        player.GetComponent<Animator>().SetBool("Crouching", false);

        // Reset
        triggered = false;
        yield return new WaitForSeconds(0.25f);
        Camera.main.GetComponent<CameraController>().FollowHeadTime = 15.0f;
    }

    IEnumerator TrapBear(GameObject bear)
    {
        // Trigger
        GetComponent<Animator>().SetTrigger("Bear");
        triggered = true;

        // Disable
        if (bear.GetComponent<Interactable>())
        {
            bear.GetComponent<Interactable>().CanInteract = false;
        }
        if (bear.GetComponent<Rigidbody>() == null)
        {
            bear.AddComponent<Rigidbody>();
        }
        bear.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bear.GetComponent<Rigidbody>().isKinematic = true;
        bear.GetComponent<Collider>().enabled = false;

        // Mark as trapped
        bear.gameObject.layer = LayerMask.NameToLayer("Trapped");

        yield return new WaitForSeconds(1.0f);

        GetComponent<AudioSource>().Stop();

        yield return null;
    }

        IEnumerator TrapObject(GameObject obj)
    {
        // Trigger
        GetComponent<Animator>().SetTrigger("Trigger");
        triggered = true;

        // Disable
        if (obj.GetComponent<Interactable>())
        {
            obj.GetComponent<Interactable>().CanInteract = false;
        }
        if (obj.GetComponent<Rigidbody>() == null)
        {
            obj.AddComponent<Rigidbody>();
        }
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Collider>().enabled = false;

        // Mark as trapped
        obj.gameObject.layer = LayerMask.NameToLayer("Trapped");

        yield return new WaitForSeconds(3f);

        // Enable
        if (obj != null && obj.GetComponent<Interactable>())
        {
            obj.GetComponent<Interactable>().CanInteract = true;
        }
        obj.GetComponent<Rigidbody>().isKinematic = false;
        obj.GetComponent<Collider>().enabled = true;

        // Calculate force in random direction
        float z = Random.Range(0, 2);
        float x = Random.Range(0, 2);
        z = (z == 0) ? -1 : 1;
        x = (x == 0) ? -1 : 1;
        Vector3 force = new Vector3(x * 100 * Time.deltaTime, 100 * Time.deltaTime, z * 100 * Time.deltaTime);
        yield return new WaitForFixedUpdate();
        force = new Vector3(x * 100 * Time.deltaTime, 100 * Time.deltaTime, z * 100 * Time.deltaTime);
        // Add force
        obj.GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);

        // Reset
        triggered = false;

        // Wait until out of collider
        yield return new WaitForSeconds(0.5f);

        // Mark as untrapped
        obj.gameObject.layer = LayerMask.NameToLayer("Default");
    }
    #endregion
}
