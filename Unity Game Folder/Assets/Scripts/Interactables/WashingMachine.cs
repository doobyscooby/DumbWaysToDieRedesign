using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WashingMachine : Interactable
{
    #region fields
    #endregion
    bool active = false;
    #region methods
    public override void Action()
    {
        active = !active;
        if (active == true)
        {
            Text = "Turn Off";
            AudioManager.Instance.PlayAudio("Washing Machine");
        }
        else
        {
            Text = "Turn On";
            AudioManager.Instance.StopAudio("Washing Machine");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!active) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        try
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0.1f, 0.5f), 20.0f, Random.Range(0.1f, 0.5f)));
        }
        catch
        {
            other.gameObject.AddComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0.1f, 0.5f), 200.0f, Random.Range(0.1f, 0.5f)));
        }
    }
    #endregion
}
