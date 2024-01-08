using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : Interactable
{
    #region fields
    [SerializeField]
    private bool ring;

    private string initText;
    private DialogueSystem dialogueSystem;
    #endregion

    #region methods
    private void Awake()
    {
        initText = Text;
        dialogueSystem = GetComponent<DialogueSystem>();
        if (ring)
            StartCoroutine(StartAlarm());
        else
            CanInteract = false;
    }
    private void Update()
    {
        if (!dialogueSystem.dialogueManager.CanPlay())
            Text = "";
        else
            Text = initText;
    }


    public override void Action()
    {
        if (!dialogueSystem.TryTriggerDialogue())
        {
            return;
        }
        // Stop audio
        GetComponent<AudioSource>().Stop();
        // Stop animation
        GetComponent<Animator>().SetBool("Ring", false);
        CanInteract = false;
    }

    public void Ring()
    {
        // Play audio
        GetComponent<AudioSource>().Play();
        // Play animation
        GetComponent<Animator>().SetBool("Ring", true);
    }

    private IEnumerator StartAlarm()
    {
        yield return new WaitForSeconds(0.25f);
        Ring();
    }
    #endregion
}