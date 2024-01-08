using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Interactable
{
    #region fields
    [SerializeField]
    private Mesh breadJam;

    private GameObject knife;
    private bool toasted, placed;
    #endregion

    #region properties
    public bool Toasted
    {
        set { toasted = value; }
    }
    public bool Placed
    {
        set { placed = value; }
    }
    #endregion

    #region methods
    public override void Action()
    {
        // Play SFX
        AudioManager.Instance.PlayAudio("Eat");
        // Play eat FX
        Camera.main.transform.Find("VFX").transform.Find("Eating Effect").GetComponent<ParticleSystem>().Play();

        CanInteract = false;
        transform.GetComponent<Renderer>().enabled = false;

        GameManager.Instance.taskManager.UpdateTaskCompletion("Make Jam Toast");
        // Play grab animation
        Animator anim = PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>();
        if (!anim.GetBool("Notepad"))
            anim.SetTrigger("Grab");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Knife Jam" && placed)
        {
            if (knife == null)
                GameManager.Instance.taskManager.UpdateTaskCompletion("Make Jam Toast");

            knife = collision.gameObject;

            // Play sfx
            AudioManager.Instance.PlayAudio("Spread");
            // Change mesh
            GetComponent<MeshFilter>().mesh = breadJam;
            // Make interactable
            GetComponent<Bread>().Type = InteractableType.Other;
            GetComponent<Bread>().CanInteract = true;
            // Change text
            GetComponent<Bread>().Text = "Eat";
            // Enable collider
            GetComponent<Collider>().enabled = true;
        }
    }
    #endregion
}
