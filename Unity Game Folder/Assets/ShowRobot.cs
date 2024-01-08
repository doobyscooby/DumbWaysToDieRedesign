using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRobot : MonoBehaviour
{
    [SerializeField]
    private GameObject myRobot;

    [SerializeField]
    private GameObject otherRobot;

    [SerializeField]
    private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.HasTransitioned)
        {
            Debug.Log("transitioned");
            if (other.gameObject.layer == 6)
            {
                myRobot.SetActive(true);
                otherRobot.SetActive(false);
                Debug.Log("Showing Robot");
            }
        }
    }
}
