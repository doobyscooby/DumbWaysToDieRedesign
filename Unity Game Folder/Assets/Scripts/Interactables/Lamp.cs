using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Light lampLight;
    #endregion

    #region methods
    private void Update()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.transform.name == "SM_Prop_Lamp_03")
            {
                lampLight.gameObject.SetActive(!lampLight.gameObject.activeSelf);
                GetComponent<AudioSource>().Play();
            }
        }
    }
    #endregion
}
