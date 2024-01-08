using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BBQWire : Interactable
{
    #region fields
    [SerializeField]
    private TrapBBQ bbq;
    [SerializeField]
    private VisualEffect gasVFX;
    #endregion

    #region properties
    public VisualEffect GasVFX
    {
        get { return gasVFX; }
    }
    #endregion

    #region methods
    public override void Action()
    {
        // Update
        GameManager.Instance.taskManager.UpdateTaskCompletion("Make BBQ");
        bbq.GetComponent<Animator>().SetTrigger("Connect");
        bbq.Connect();
        // Disable fx
        gasVFX.Stop();
        gasVFX.GetComponent<AudioSource>().Stop();
        
        CanInteract = false;
    }
    #endregion
}
