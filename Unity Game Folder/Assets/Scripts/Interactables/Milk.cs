using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : Interactable
{
    #region methods
    public override void Action()
    {
        // Complete
        GameManager.Instance.taskManager.UpdateTaskCompletion("Drink Milk");
        GetComponent<AudioSource>().Play();
        // Disable
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 1.0f);
    }
    #endregion
}
