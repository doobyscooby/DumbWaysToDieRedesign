using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class electricBeam : MonoBehaviour
{

    #region fields
    [SerializeField]
    private bool electric;
    #endregion

    #region properties
    public bool Electric
    {
        set { electric = value; }
    }
    #endregion

    #region methods
   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Player" && electric && !PlayerController.Instance.Dead)
        {
            StartCoroutine(KillPlayer());
        }
    }

    IEnumerator KillPlayer()
    {
        // Disable controls
        GameManager.Instance.EnableControls = false;
        // Start player electric fx
        foreach (Transform child in PlayerController.Instance.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "ElectricityFX")
            {
                child.gameObject.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.2f);
        PlayerController.Instance.Die(0.0f, false, SelectCam.outsideCam);
        // Play animation
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Electrecute");
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, PlayerController.Instance.GetComponent<Rigidbody>().velocity.y, 0.0f);
        // Play sfx
        AudioManager.Instance.PlayAudio("Tase");
        yield return new WaitForSeconds(0.75f);
        // Stop player electric fx
        foreach (Transform child in PlayerController.Instance.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "ElectricityFX")
            {
                child.gameObject.SetActive(false);
            }
        }
        // Ragdoll
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().enabled = false;
        PlayerController.Instance.EnableRagdoll();
    }
    #endregion
}
