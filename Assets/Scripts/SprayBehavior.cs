using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayBehavior : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            //To Do: Edit move speed of player - reduce by half
            Debug.Log("Sticky");
        }
    }
}
