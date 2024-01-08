using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBearTrap : MonoBehaviour
{
    // Beartrap Prefab
    [SerializeField]
    GameObject bearTrap;
    [SerializeField]
    Animator animator;
    [SerializeField]
    float timeBetweenPlacements = 4.0f;
    [SerializeField]
    int maxTraps = 2;

    [SerializeField]
    Transform trapPos;

    float timeTillNextPlace;
    List<GameObject> traps = new List<GameObject>();

    [SerializeField]
    bool canPlace = false;

    // Place Beartrap at position
    public void Place()
    {
        if (canPlace && Time.time > timeTillNextPlace && traps.Count < 5)
        {
            timeTillNextPlace = Time.time + timeBetweenPlacements;
            StartCoroutine(PlaceTrap());
        } 
    }

    IEnumerator PlaceTrap()
    {
        animator.SetTrigger("Bear Trap");
        yield return new WaitForSeconds(2.0f);
        GameObject newObject = Instantiate(bearTrap, trapPos.position, trapPos.rotation, null);
        newObject.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        traps.Add(newObject);
        animator.ResetTrigger("Bear Trap");
    }
}
