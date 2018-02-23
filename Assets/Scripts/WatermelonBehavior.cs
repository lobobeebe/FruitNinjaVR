using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatermelonBehavior : MonoBehaviour {

    private GameObject player;
    private Transform objectOfAttraction;

    private Transform myTransform;
    private Rigidbody myRigidbody;

    void Awake()
    {
        // cache these
        myTransform = transform;
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        objectOfAttraction = player.transform;
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= PatrollingBehavior.aggroRadius)
        {
            // get the positions of this object and the target
            Vector3 targetPosition = objectOfAttraction.position;
            Vector3 myPosition = myTransform.position;

            // work out direction and distance
            Vector3 direction = (targetPosition - myPosition).normalized;
            float distance = Vector3.Magnitude(targetPosition - myPosition);       // you could move this inside the switch to avoid processing it for the Constant case where it's not used
            Vector3 resultingForceAmount = 2.0f * direction;

            // then finally add the force to the rigidbody
            myRigidbody.AddForce(resultingForceAmount);
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
