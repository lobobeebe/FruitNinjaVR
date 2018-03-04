using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawberryBehavior : MonoBehaviour {

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("RightController");
	}

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= PatrollingBehavior.aggroRadius)
        {
            this.gameObject.transform.LookAt(player.transform);
            this.gameObject.transform.position += transform.forward * Time.deltaTime * VoiceController.speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name.Equals("RightController"))
        {
            Destroy(this.gameObject);
        }
        // Bounce the strawberry on the ground
        if (collision.collider.gameObject.name.Equals("Terrain") && Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= PatrollingBehavior.aggroRadius)
        {
            float jumpHeight = 10.0f * VoiceController.speed;
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, jumpHeight, 0);
        }
    }
}
