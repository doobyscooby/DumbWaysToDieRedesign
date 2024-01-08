using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Clothing[] clothings;
    #endregion

    #region properties
    public Clothing[] Clothings
    { 
        get { return clothings; } 
    }
    #endregion

    #region methods
    public void EnableVFX()
    {
        StartCoroutine(PlayVFX());
    }

    IEnumerator PlayVFX()
    {
        foreach (ParticleSystem particleEffect in transform.parent.GetComponentsInChildren<ParticleSystem>())
        {
            particleEffect.Play();
        }
        yield return new WaitForSeconds(0.75f);
        foreach (ParticleSystem particleEffect in transform.parent.GetComponentsInChildren<ParticleSystem>())
        {
            particleEffect.Stop();
        }
    }
    #endregion
}
