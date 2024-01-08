using UnityEngine;

public class Interactable : MonoBehaviour
{
    #region fields
    public enum InteractableType
    {
        Pickup,
        Pivot,
        Trap,
        Other,
        Readable
    }
    [Header("Interactable Settings")]
    [SerializeField]
    private InteractableType type;
    [SerializeField]
    private string text;
    [SerializeField]
    private bool keepRotation = true;
    private bool canInteract = true;
    private bool interacting = false;

    #endregion

    #region properties
    public InteractableType Type
    {
        get { return type; }
        set { type = value; }
    }
    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public bool KeepRotation
    {
        get { return keepRotation; }
    }

    public bool CanInteract
    {
        get { return canInteract; }
        set { canInteract = value; }
    }

    public bool Interacting
    {
        get { return interacting; }
        set { interacting = value; }
    }
    #endregion

    #region methods
    private void OnValidate()
    {
        switch (Type)
        {
            case InteractableType.Pickup:
                Text = "Pick Up";
                break;
            case InteractableType.Pivot:
                break;
        }
    }

    public virtual void Action() { }
    #endregion
}
