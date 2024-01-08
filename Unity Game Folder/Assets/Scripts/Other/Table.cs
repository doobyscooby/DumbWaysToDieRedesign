using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("bip"))
        {
            if (!gameObject.GetComponent<Rigidbody>())
                transform.parent.gameObject.AddComponent<Rigidbody>();
        }
    }
}
