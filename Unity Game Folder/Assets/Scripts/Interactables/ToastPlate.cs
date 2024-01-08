using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastPlate : Interactable
{
    #region fields
    [SerializeField]
    private Mesh knifeJam;

    private GameObject bread, jam, knife;
    #endregion

    #region methods
    private void Update()
    {
        if (Text != "Place" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Toasted Bread")
            Text = "Place";
        else if (Text != "Spread" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Jam" && bread)
            Text = "Spread";
        else if (Text != "")
            Text = "";
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.name == "Toasted Bread")
        {
            bread = collision.gameObject;
            InteractionSystem.Instance.DropObject();

            // Remove rigidbody
            Destroy(bread.GetComponent<Rigidbody>());
            // Attach to plate
            bread.transform.parent = transform;
            // Set transform
            bread.transform.localPosition = new Vector3(0.1f, 0.02f, 0.0f);
            bread.transform.localEulerAngles = new Vector3(0.0f, 90f, 0.0f);
            bread.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // Make non-pickable
            bread.GetComponent<Bread>().CanInteract = false;
            // Reset text
            bread.GetComponent<Bread>().Text = "";
            // Mark as placed
            bread.GetComponent<Bread>().Placed = true;

            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Jam Toast");
        }
        else if (collision.transform.name == "Jam")
        {
            jam = collision.gameObject;

            // Drop jam from player
            InteractionSystem.Instance.DropObject();
            // Disable interaction with jam
            jam.GetComponent<Interactable>().CanInteract = false;
            // Remove jam rigidbody
            Destroy(jam.GetComponent<Rigidbody>());

            // Snap to position
            jam.transform.position = new Vector3(-6.9f, 1.39f, 8.3f);
            jam.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            // Play open jam animation
            jam.GetComponent<Animator>().SetTrigger("Open");

            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Jam Toast");
        }
    }
    
    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.name == "Knife" && jam)
        {
            if (knife == null)
                GameManager.Instance.taskManager.UpdateTaskCompletion("Make Jam Toast");

            knife = collision.gameObject;

            knife.name = "Knife Jam";
            // Change mesh
            knife.GetComponent<MeshFilter>().mesh = knifeJam;
        }
    }
    #endregion
}