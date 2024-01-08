using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakePlate : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject cooked, iced, icing;
    private bool placed;
    #endregion

    #region methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == cooked)
        {
            // Drop
            InteractionSystem.Instance.DropObject();
            if (collision.gameObject.GetComponent<Rigidbody>())
                Destroy(collision.gameObject.GetComponent<Rigidbody>());
            // Snap
            collision.transform.parent = transform;
            collision.transform.localPosition = new Vector3(0.0f, 0.012f, 0.0f);
            collision.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            collision.transform.localScale = new Vector3(0.034f, 0.034f, 0.072f);
            // Disable interaction
            collision.gameObject.GetComponent<Interactable>().CanInteract = false;
            placed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (placed && other.gameObject == icing)
        {
            // Drop
            InteractionSystem.Instance.DropObject();
            // Destroy
            Destroy(other.gameObject);
            Destroy(cooked);
            // Spawn iced cake
            GameObject cake = Instantiate(iced);
            cake.transform.parent = transform;
            cake.transform.localPosition = new Vector3(0.0f, 0.016f, 0.0f);
            cake.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, -180.0f);
            cake.transform.localScale = new Vector3(0.038f, 0.038f, 0.081f);

            // Increase
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Cake");
        }
    }
    #endregion
}
