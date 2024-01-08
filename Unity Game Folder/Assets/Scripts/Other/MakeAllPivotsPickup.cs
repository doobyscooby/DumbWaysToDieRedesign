using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MakeAllPivotsPickup : MonoBehaviour
{
    private void Awake()
    {
        Interactable[] interactables = (Interactable[])Resources.FindObjectsOfTypeAll(typeof(Interactable));

        foreach (var interactable in interactables)
        {
            if (interactable.Type == Interactable.InteractableType.Pivot)
            {
                interactable.Type = Interactable.InteractableType.Pickup;
                MeshCollider mc = interactable.gameObject.GetComponent<MeshCollider>();
                if (mc)
                    mc.convex = true;
            }
        }

        Door[] doors = (Door[])Resources.FindObjectsOfTypeAll(typeof(Door));
        foreach (var door in doors)
        {
            door.Type = Interactable.InteractableType.Pickup;
        }

        Debug.Log("Done?");
    }
}
