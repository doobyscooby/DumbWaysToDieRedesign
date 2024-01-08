using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BasketBallHoop : MonoBehaviour
{
    [SerializeField]
    VisualEffect vfx;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Basketball")
        {
            vfx.Play();
        }
    }
}
