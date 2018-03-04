using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaBehavior : MonoBehaviour {

    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("RightController");
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= PatrollingBehavior.aggroRadius)
        {
            this.gameObject.transform.LookAt(player.transform);
            this.gameObject.transform.position += transform.forward * 8.0f * Time.deltaTime * VoiceController.speed;
        }
    }
}
