using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "New Tasks/Task")]
public class Task : ScriptableObject
{
    public string taskName;

    public string taskDescription;

    [SerializeField] 
    private bool baseTaskComplete = false;

    [HideInInspector]
    public bool taskComplete;

    [SerializeField]
    public GameObject associatedTrap;

    public bool spawn = false;

    public string nameOfPosition;

    public bool isDependent = false;

    public int steps = 1;

    [HideInInspector]
    public int stepsComplete;

    // Might be needed later
    /*
    private void OnEnable()
    {
        Reset();
    }*/

    public void Reset()
    {
        // Reset task
        taskComplete = baseTaskComplete;
        taskComplete = false;
        stepsComplete = 0;
        // Reset gameobject
        if (spawn)
        {
            if (string.IsNullOrEmpty(nameOfPosition))
            {
                throw new Exception("Position Name is Null or Empty for Task: " + taskName);
            }
            if (associatedTrap == null)
            {
                throw new Exception("Unassigned Associated Trap for Task: " + taskName);
            }
            GameObject pos = GameObject.Find(nameOfPosition);
            if (pos.transform.childCount > 0)
            {
                Destroy(pos.transform.GetChild(0).gameObject);
            }
            GameObject trap = Instantiate(associatedTrap);
            trap.transform.parent = pos.transform;
            trap.transform.localPosition = Vector3.zero;
            trap.transform.localRotation = Quaternion.identity;
            trap.transform.localScale = Vector3.one;
        }
    }
}
