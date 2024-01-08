using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class DialogueSystem : Interactable
{
    [Header("Needed")]
    [SerializeField]
    bool interactable;
    [SerializeField]
    AudioClip tickAudio;

    [Header("Optional")]
    [SerializeField]
    AudioClip introAudio;
    [SerializeField]
    AudioClip outroAudio;


    [Header("Dialogue")]

    [SerializeField]
    public List<Dialogue> dialogues = new List<Dialogue>();

    string initialText;
    bool complete = true;

    AudioSource audioSource;
    public DialogueManager dialogueManager = new DialogueManager();
    
    public override void Action()
    {
        if (!interactable)
            return;

        if (TryTriggerDialogue())
            CanInteract = false;
    }

    private void Update()
    {
        if (!dialogueManager.CanPlay())
            Text = "";
        else
            Text = initialText;
    }

    private void Awake()
    {
        initialText = Text;
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            throw new System.Exception("AudioSource not added to dialogue system");
    }
    public bool TryTriggerDialogue()
    {
        if (dialogueManager.CanPlay())
        {
            audioSource.volume = 0.1f;
            StartCoroutine(dialogueManager.StartDialogue(GameUI.Instance, dialogues.ToArray(), audioSource, tickAudio, introAudio, outroAudio));
            return true;
        }
        return false;
    }
}
