using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryBehavior : MonoBehaviour {

    private GameObject player;
    private int counter = 0;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	}

    // Update is called once per frame
    void Update()
    {
        counter++;
        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= PatrollingBehavior.aggroRadius)
        {
            this.gameObject.transform.LookAt(player.transform);
            this.gameObject.transform.position += transform.forward * Time.deltaTime;
            
            if (counter >= 400)
            {
                counter = 0;
                this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 10.0f, 0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name.Equals("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
