using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBattery : Interactable
{
    #region fields
    [SerializeField]
    private Clothes clothes;
    [SerializeField]
    private Beams beams;
    #endregion

    #region methods
    public override void Action()
    {
        // Disable electricity
        foreach (Clothing clothing in clothes.Clothings)
        {
            clothing.Electric = false;
        }
        foreach (electricBeam beam in beams.electricBeams)
        {
            beam.Electric = false;
        }
        // Stop sfx
        transform.parent.GetComponent<AudioSource>().Stop();
        // Play animation
        transform.parent.GetComponent<Animator>().SetTrigger("Trigger");
        // Play switch sfx
        AudioManager.Instance.PlayAudio("Switch");
        Destroy(this);
    }
    #endregion
}