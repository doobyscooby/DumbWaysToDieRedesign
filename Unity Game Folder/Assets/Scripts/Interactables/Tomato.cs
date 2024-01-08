using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : MonoBehaviour
{
    private bool played;

    private void Update()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && !played && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.transform.name == "SM_Item_Tomato_01")
            {
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                GetComponent<AudioSource>().Play();
                played = true;
            }
        }
    }
}
