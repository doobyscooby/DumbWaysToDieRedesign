using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayMusic : Interactable
{
    [SerializeField]
    AudioClip[] audioClips;
    AudioSource audioSource;

    int index = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
    public override void Action()
    {
        SwapMusic();
    }
    public void SwapMusic()
    {
        if (audioClips.Length == 0) return;
        if (audioClips.Length == 1)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
            else
                audioSource.Stop();
        }
        else
        {
            index++;
            if (index >= audioClips.Length) index = 0;
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
    }
    public void StopAll()
    {
        audioSource.Stop();
    }

    public void TransitionDay()
    {
        audioSource.volume = audioSource.volume / 2;
        audioSource.time = audioSource.time + 20;
    }
}
