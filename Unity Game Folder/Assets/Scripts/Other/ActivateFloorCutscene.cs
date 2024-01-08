using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ActivateFloorCutscene : MonoBehaviour
{
    #region fields
    private VideoPlayer video;
    #endregion

    #region methods
    private void Awake()
    {
        video = GetComponent<VideoPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("MainCamera") && GameManager.Instance.HasTransitioned == true)
        {
            GameManager.Instance.EnableControls = false;
            GameManager.Instance.EnableCamera = false;
            video.enabled = true;
            GameManager.Instance.StopMusic();
            StartCoroutine(Switch());
        }
    }

    IEnumerator Switch()
    {
        yield return new WaitForSeconds(13.0f);
        GameManager.Instance.MoveToNextLevel();
    }
    #endregion
}
