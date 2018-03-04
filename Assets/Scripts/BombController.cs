using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name.Equals("AppleShogun"))
        {
            PlotController.AppleShogunDefeated = true;
            Destroy(collision.collider.gameObject);
            Destroy(this.gameObject);
        }
    }
}
