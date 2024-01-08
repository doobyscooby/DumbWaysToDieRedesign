using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRobot : MonoBehaviour
{
    [SerializeField]
    private GameObject bedRobot;

    [SerializeField]
    private GameObject kitchenRobot;

    [SerializeField]
    private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.HasTransitioned)
        {
            if (other.gameObject.layer == 6)
            {
                bedRobot.SetActive(false);
                kitchenRobot.SetActive(false);
                Debug.Log("hiding robot");
            }
        }
    }
}
