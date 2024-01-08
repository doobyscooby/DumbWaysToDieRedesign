using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNuke : MonoBehaviour
{
    public GameObject nuke;
    public AudioSource audio;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            nuke.SetActive(true);
            audio.Play();
            CameraShaker.Instance.ShakeOnce(12.0f, 4f, 1.0f, 4.0f);
        }
    }
}
