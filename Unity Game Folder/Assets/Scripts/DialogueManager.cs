using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string text;
    public float speed = 5.0f;
    public float holdAfterFinished = 3.0f;
}

public class DialogueManager
{
    // Checks if Audio can be played
    public bool CanPlay()
    {
        if (GameUI.Instance.DialogueText.transform.parent.gameObject.activeSelf)
            return false;
        return true;
    }
    void PlayMurmur(AudioSource audioSource, AudioClip audio)
    {
        if (audio != null)
        {
            audioSource.pitch = Random.Range(0.90f, 1.10f);
            audioSource.PlayOneShot(audio);
        }
    }
    public IEnumerator StartDialogue(GameUI gameUI, Dialogue[] dialogues,AudioSource audioSource, AudioClip tickAudio, AudioClip introAudio = null, AudioClip outroAudio = null)
    {
        // Set GameUI to active
        gameUI.DialogueText.transform.parent.gameObject.SetActive(true);
        if (introAudio != null)
        { 
            audioSource.PlayOneShot(introAudio);

            // Wait for intro audio to play
            yield return new WaitForSeconds(introAudio.length);
        }

        // For each Dialogue
        foreach (Dialogue dialogue in dialogues)
        {
            // Rest UI
            gameUI.DialogueText.text = "";

            // For each tick
            float speed = 1 / dialogue.speed;
            for (float i = 0; i < speed; i += Time.deltaTime)
            {
                // Gets integer value representing how far we are into the dialogue
                int value = (int)Mathf.Lerp(0, dialogue.text.Length - 1, i / speed);
                // If the value has changed
                if (value > gameUI.DialogueText.text.Length)
                {
                    // Update GameUI
                    gameUI.DialogueText.text = dialogue.text.Substring(0, value);

                    // Play noise
                    PlayMurmur(audioSource, tickAudio);
                }
                yield return new WaitForFixedUpdate();
            }
            // Finish GameUI Text
            gameUI.DialogueText.text = dialogue.text;
            yield return new WaitForSeconds(dialogue.holdAfterFinished);
        }
        // Play Outro Audio
        if (outroAudio != null)
        {
            audioSource.PlayOneShot(outroAudio);

            yield return new WaitForSeconds(outroAudio.length);
        }

        // Disable GameUI

        GameUI.Instance.DialogueText.text = "";
        GameUI.Instance.DialogueText.transform.parent.gameObject.SetActive(false);

        // Stop Audio
        audioSource.Stop();
    }
}
