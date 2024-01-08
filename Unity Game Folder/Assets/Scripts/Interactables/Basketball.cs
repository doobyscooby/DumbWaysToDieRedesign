using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Basketball : MonoBehaviour
{
    VisualEffect vfx;

    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (vfx) vfx.Play();
    }
}
