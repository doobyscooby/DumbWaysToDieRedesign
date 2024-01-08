using System;
using System.Collections;
using UnityEngine;

public class TrapToaster : Interactable
{
    #region fields
    [SerializeField]
    private GameObject bread;
    [SerializeField]
    private GameObject knife;
    [SerializeField]
    private GameObject jam;
    [SerializeField]
    private GameObject plate;
    private bool placed;

    [SerializeField]
    private GameObject explosionVFX;

    [SerializeField]
    private bool kills = true;
    [SerializeField]
    private float delay = 0.5f;
    #endregion

    #region methods
    private void Awake()
    {
        if (bread == null || knife == null || jam == null || plate == null)
        {
            if (kills == true)
                throw new Exception("Toaster trap has not yet been set up!");
        }
    }

    private void Update()
    {
        if (Text != "Drop" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Bread")
            Text = "Drop";
        else if (Text != "")
            Text = "";

        // Change toast color
        if (placed)
            bread.GetComponent<Renderer>().material.color = Color.Lerp(bread.GetComponent<Renderer>().material.color, new Color32(145, 126, 98, 255), 0.5f * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Bread")
        {
            GameObject obj = collision.gameObject;
            InteractionSystem.Instance.DropObject();
            // If knife has not been taken out of toaster then kill the player.
            if (knife != null && knife.transform.parent != null && kills)
                StartCoroutine(KillPlayer());
            else
                StartCoroutine(ChangeBread());

            // Remove rigidbody
            Destroy(obj.GetComponent<Rigidbody>());

            // Attach to toaster
            obj.transform.parent = transform;
            // Change type
            obj.GetComponent<Bread>().CanInteract = false;
            // Change text
            obj.GetComponent<Bread>().Text = "";
            // Set transform
            obj.transform.localPosition = new Vector3(0.0f, 0.075f, 0.03f);
            obj.transform.localEulerAngles = new Vector3(90f, 0.0f, 0.0f);
            obj.transform.localScale = new Vector3(1.0f, 0.8f, 1.2f);
        }
    }

    IEnumerator KillPlayer()
    {
        GetComponent<Animator>().SetTrigger("Explode");
        PlayerController.Instance.ThrowPlayerInDirection(new Vector3(100, 10, 0), delay, SelectCam.toasterCam);
        
        yield return new WaitForSeconds(delay);
        Destroy(bread);
        AudioManager.Instance.PlayAudio("Explosion");
        explosionVFX.SetActive(true);
    }

    IEnumerator ChangeBread()
    {
        placed = true;
        GetComponent<Animator>().SetTrigger("Activate");
        yield return new WaitForSeconds(2.0f);
        // Rename
        bread.name = "Toasted Bread";
        // Change type
        bread.GetComponent<Bread>().CanInteract = true;
        // Change text
        bread.GetComponent<Bread>().Text = "Pick Up";
        // Mark as toasted
        bread.GetComponent<Bread>().Toasted = true;
        // Move slightly up
        bread.transform.position += Vector3.up * 0.025f;
        // Reset
        placed = false;

        GameManager.Instance.taskManager.UpdateTaskCompletion("Make Jam Toast");
    }
    #endregion
}