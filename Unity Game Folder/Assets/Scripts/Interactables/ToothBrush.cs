using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ToothBrush : Interactable
{
    #region methods
    public override void Action()
    {
        CanInteract = false;
        // Player Animation
        PlayerController.Instance.GetComponentInChildren<Animator>().SetTrigger("Brush Teeth");
        // Mark as complete
        GameManager.Instance.taskManager.UpdateTaskCompletion("Brush Teeth");
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(1.05f);
        GetComponent<MeshRenderer>().enabled = true;
        Destroy(this);
    }
    #endregion
}
