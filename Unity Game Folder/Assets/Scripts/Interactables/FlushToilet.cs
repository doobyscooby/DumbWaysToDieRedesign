using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FlushToilet : Interactable
{
    #region fields
    [SerializeField]
    private AudioSource flushAudio;
    #endregion

    private void Awake()
    {
        Text = "Flush";
    }
    #region methods
    public override void Action()
    {
        flushAudio.Stop();
        flushAudio.Play();
    }
    #endregion
}
