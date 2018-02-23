using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name.Equals("Player"))
        {
            Debug.Log("Ouch!");
        }
    }
}
