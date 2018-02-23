using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleBehavior : MonoBehaviour {

    private GameObject player;
    public GameObject arrow;
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
            if (counter >= 200)
            {
                GameObject newArrow = GameObject.Instantiate(arrow);
                newArrow.transform.position = this.gameObject.transform.position;
                newArrow.transform.rotation = this.gameObject.transform.rotation;

                counter = 0;

                Destroy(newArrow, 10.0f);
            }
            
        }
    }
}
