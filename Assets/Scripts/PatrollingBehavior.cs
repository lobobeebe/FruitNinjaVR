using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingBehavior : MonoBehaviour
{
    public static float aggroRadius = 25.0f;

    private Vector3 anchor;
    private GameObject player;

    private Vector3 target;
    private int counter = 0;
    private float step;
    private bool moveEnemy = false;

    // Use this for initialization
    void Start()
    {
        anchor = this.transform.position;
        player = GameObject.Find("RightController");
        step = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveEnemy)
        {
            counter++;
        }
        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) >= aggroRadius)
        {
            if (counter >= 600)
            {
                counter = 0;
                moveEnemy = true;
                float X = UnityEngine.Random.Range(anchor.x - 20.0f, anchor.x + 20.0f);
                float Z = UnityEngine.Random.Range(anchor.z - 20.0f, anchor.z + 20.0f);
                target = new Vector3(X, anchor.y, Z);
            }
            if (moveEnemy)
            {
                this.gameObject.transform.LookAt(target);
                this.transform.position = Vector3.MoveTowards(transform.position, target, step);
            }
            if (this.transform.position == target)
            {
                moveEnemy = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.Equals("Sword_Mesh"))
        {
            Destroy(this.gameObject);
        }
    }
}
