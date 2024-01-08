using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Decoupled AudioManager
public class AudioManager : MonoBehaviour
{

    // Anytime an audio is needed, the script needs a reference to this script,
    // if PlayAudio function is called and passed through a name, then if there is an 
    // audioSource with that name, it will be found and played.

    // When instantiated, the values can be editted in the inspector.
    [System.Serializable]
    public class Sound
    {
        public AudioSource audioSource;
        public string audioName;
    }

    // Instantiates a version of the Sound class
    public Sound[] sounds;
    Sound foundAudio;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(string nameToFind, float pitch = 1.0f)
    {
        bool found = false;
        // Check sounds
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].audioName == nameToFind)
            {
                foundAudio = sounds[i];
                foundAudio.audioSource.pitch = pitch;
                // Play audio
                foundAudio.audioSource.Play();
                found = true;
            }
        }
        if (found == false)
        {
            Debug.Log("Sound not found " + nameToFind);
        }
    }
    public void StopAudio(string nameToFind)
    {
        bool found = false;
        // Check sounds
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].audioName == nameToFind)
            {
                foundAudio = sounds[i];
                // Play audio
                foundAudio.audioSource.Stop();
                found = true;
            }
        }
        if (found == false)
        {
            Debug.Log("Sound not found " + nameToFind);
        }
    }

    public Sound GetAudio(string nameToFind)
    {
        bool found = false;
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].audioName == nameToFind)
            {
                foundAudio = sounds[i];
                return foundAudio;
            }
        }
        if (found == false)
        {
            Debug.Log("Sound not found " + nameToFind);
        }
        return null;
    }

    public void turnVolumeDown(Slider volumeSlider)
    {
        float value = volumeSlider.value;
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].audioSource.volume = value;
        }
    }
}
