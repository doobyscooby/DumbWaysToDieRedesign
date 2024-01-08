using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPlayerSFX : MonoBehaviour
{
    [SerializeField]
    private TrapCouch couch;
    private bool hit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !hit && couch && couch.Trapped)
        {
            AudioManager.Instance.PlayAudio("Hit Ground");
            hit = true;
        }
    }
}
