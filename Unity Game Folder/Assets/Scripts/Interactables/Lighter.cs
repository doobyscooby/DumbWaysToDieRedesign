using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Lighter : Interactable
{
    #region methods
    private void Update()
    {
        if (Interacting)
        {
            transform.GetChild(0).GetComponent<VisualEffect>().Play();
        }
        else
        {
            transform.GetChild(0).GetComponent<VisualEffect>().Stop();
        }
    }
    #endregion
}
