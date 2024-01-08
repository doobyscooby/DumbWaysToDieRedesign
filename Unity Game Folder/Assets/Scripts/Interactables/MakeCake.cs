using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class MakeCake : Interactable
{
    #region fields
    [SerializeField]
    private GameObject[] ingredientsRequired;
    [SerializeField]
    private GameObject insideOven, icing;
    private GameObject tin, mix, cooked, iced;
    private int ingredientsAdded;
    private bool added, started, opened, deadly;

    [SerializeField]
    private GameObject oven, ovenDoor;
    [SerializeField]
    private GameObject smokeVFX;

    [SerializeField]
    private VisualEffect[] fireVfxs;
    [SerializeField]
    AudioSource fireSFX;
    [SerializeField]
    AudioClip explosionSFX;

    [SerializeField]
    LayerMask playerLayer;

    private bool fired = false;
    #endregion

    #region methods
    private void Awake()
    {
        tin = transform.GetChild(0).gameObject;
        cooked = transform.GetChild(1).gameObject;
        mix = transform.GetChild(2).gameObject;
        iced = transform.GetChild(3).gameObject;
        CanInteract = false;
    }
    private void Update()
    {
        if (fired)
            return;
        if (!started && added && !ovenDoor.GetComponent<PivotSettings>().open)
        {
            // Disable door interaction
            ovenDoor.transform.GetChild(0).GetComponent<Interactable>().CanInteract = false;
            // Play animation
            oven.GetComponent<Animator>().SetBool("Switch", true);
            StartCoroutine(StartTimer());
            started = true;
        }

        if (started && ovenDoor.GetComponent<PivotSettings>().open)
        {
            // Stop
            StopCoroutine(StartTimer());
            oven.GetComponent<Animator>().SetBool("Switch", false);

            if (deadly)
             {
                fired = true;
                if (Physics.CheckBox(transform.position, new Vector3(1, 1, 4), Quaternion.identity, playerLayer))
                    StartCoroutine(KillPlayer(1.0f));
                else
                {
                    StartFire();
                }
            }

            CanInteract = true;
            opened = true;
        }
    }
    IEnumerator KillPlayer(float delay)
    {
        PlayerController.Instance.ThrowPlayerInDirection(new Vector3(0.0f, 20.0f, -30.0f), delay * 1.1f, SelectCam.toasterCam);
        
        yield return new WaitForSeconds(delay);
        StartFire();
        // Disable controls
        GameManager.Instance.EnableControls = false;
        // Start player fire fx
        foreach (Transform child in PlayerController.Instance.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "FireVFX")
            {
                child.gameObject.SetActive(true);
            }
        }
        smokeVFX.SetActive(false);
        Destroy(this);
    }

    void StartFire()
    {
        fireSFX.Play();
        fireSFX.PlayOneShot(explosionSFX);
        foreach (VisualEffect fireVfx in fireVfxs)
            fireVfx.Play();
    }
    IEnumerator StartTimer()
    {
        // Wait to cook
        yield return new WaitForSeconds(16.0f);
        // Enable door interaction
        ovenDoor.transform.GetChild(0).GetComponent<Interactable>().CanInteract = true;
        // Swap
        mix.gameObject.SetActive(false);
        cooked.gameObject.SetActive(true);

        // Start fire after 5 seconds
        yield return new WaitForSeconds(5.0f);
        
        if (!opened)
        {
            // Mark as deadly
            deadly = true;

            smokeVFX.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ingredient collision
        if (ingredientsRequired.Contains(collision.transform.gameObject))
        {
            // Increase
            ingredientsAdded++;
            IncreaseMix();
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Cake");

            // Destroy ingredient
            InteractionSystem.Instance.DropObject();
            Destroy(collision.transform.gameObject);

            // All ingredients are in check
            if (ingredientsAdded == 5)
            {
                // Make interactable
                CanInteract = true;
            }
        }
        // Oven collision
        else if (collision.gameObject == insideOven)
        {
            // Disable collider
            collision.gameObject.GetComponent<Collider>().enabled = false;
            // Remove rigidbody
            Destroy(GetComponent<Rigidbody>());

            // Snap inside
            InteractionSystem.Instance.DropObject();
            transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            transform.localPosition = new Vector3(-4.5f, 0.972f, 10.583f);
            CanInteract = false;

            // Mark as added
            added = true;
        }
        else if (collision.gameObject == icing && opened)
        {
            // Swap
            tin.gameObject.SetActive(false);
            cooked.gameObject.SetActive(false);
            iced.gameObject.SetActive(true);

            // Destroy icing
            InteractionSystem.Instance.DropObject();
            Destroy(collision.transform.gameObject);

            // Play sfx
            AudioManager.Instance.PlayAudio("Cloth");

            // Increase
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Cake");
        }
    }

    private void IncreaseMix()
    {
        // Increase scale
        switch(ingredientsAdded)
        {
            case 1:
                mix.transform.localScale = new Vector3(1.0f, 1.0f, 0.55f);
                break;
            case 2:
                mix.transform.localScale = new Vector3(1.0f, 1.0f, 1.1f);
                break;
            case 3:
                mix.transform.localScale = new Vector3(1.0f, 1.0f, 1.65f);
                break;
            case 4:
                mix.transform.localScale = new Vector3(1.0f, 1.0f, 2.2f);
                break;
            case 5:
                mix.transform.localScale = new Vector3(1.0f, 1.0f, 2.75f);
                break;
        }
        // Play sfx
        AudioManager.Instance.PlayAudio("Cloth");
    }
    #endregion
}
