using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private GameObject bigHand;
    [SerializeField]
    private GameObject smallHand;

    [SerializeField][Range(-10.0f, 10.0f)]
    private float speed = 10.0f;

    private void FixedUpdate() { 
        float randomNum = Random.Range(0, speed);
        bigHand.transform.Rotate(new Vector3(0, 0, randomNum));
        smallHand.transform.Rotate(new Vector3(0, 0, randomNum/12.0f));
    }
}
