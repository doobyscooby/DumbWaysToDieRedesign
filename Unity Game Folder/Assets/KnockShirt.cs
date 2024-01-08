using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockShirt : MonoBehaviour
{
    public GameObject shirt;
    public GameObject gasBottle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Gas Bottle")
        {
            Debug.Log("Bottle Hit");
            Animator anim = shirt.GetComponent<Animator>();
            anim.Play("Activate");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
