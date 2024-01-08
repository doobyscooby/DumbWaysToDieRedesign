using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Ladder : Interactable
{
    bool active = false;
    #region methods

    private void OnTriggerEnter(Collider other)
    {
        if (!active && other.transform.name == "treehouse(FixedFinal)")
        {
            active = true;
            TreehouseSnap(other.gameObject);
        }
    }

    private void TreehouseSnap(GameObject treehouse)
    {
        // Drop ladder
        InteractionSystem.Instance.DropObject();
        // Disable
        Destroy(GetComponent<Rigidbody>());
        GetComponent<Interactable>().enabled = false;
        treehouse.GetComponent<Collider>().enabled = false;
        // Snap to treehouse
        transform.parent = treehouse.transform;
        StartCoroutine(SnapToTreeHouse(new Vector3(-2.33f, 1.05f, -3.71f), Quaternion.Euler(0.85f, 90, -80.952f)));
        transform.localScale = new Vector3(2.249109f, 1.932703f, 1.946055f);
        // Make climable
        GetComponent<Interactable>().Type = InteractableType.Other;
        GetComponent<Interactable>().Text = "Climb";
    }

    public override void Action()
    {
        PlayerController.Instance.transform.position = new Vector3(12.38533f, 5.190731f, 10.819f);
    }

    IEnumerator SnapToTreeHouse(Vector3 endingPos, Quaternion endingAngle)
    {
        Debug.Log("hIT");
        float speed = .25F;
        Vector3 startingPos = transform.localPosition;
        Quaternion startingAngle = transform.localRotation;
        for (float i = 0; i < speed; i += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(startingPos, endingPos, i / speed);
            transform.localRotation = Quaternion.Lerp(startingAngle, endingAngle, i / speed);
            yield return new WaitForFixedUpdate();
        }
    }
    #endregion
}
