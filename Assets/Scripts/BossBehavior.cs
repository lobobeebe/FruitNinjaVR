using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour {

    //Stage 2
    public static int hits = 0;
    public GameObject roofToDestroy1;
    public GameObject roofToDestroy2;
    public GameObject roofToDestroy3;
    public GameObject roofToDestroy4;
    public GameObject roofToDestroy5;
    public GameObject roofToDestroy6;
    public GameObject roofToDestroy7;

    private int counter = 0;
    public GameObject strawberry;
    private GameObject player;

    public AudioClip explosion;
    private AudioSource source;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("RightController");
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (hits == 3)
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            source.Play();
            Destroy(roofToDestroy1);
            Destroy(roofToDestroy2);
            Destroy(roofToDestroy3);
            Destroy(roofToDestroy4);
            Destroy(roofToDestroy5);
            Destroy(roofToDestroy6);
            Destroy(roofToDestroy7);
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(2.0f, 18.0f, 4.0f);
            hits = 4;
        }
        counter++;
        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= PatrollingBehavior.aggroRadius)
        {
            this.gameObject.transform.LookAt(player.transform);
            if (counter >= 100)
            {
                GameObject newArrow = GameObject.Instantiate(strawberry);
                newArrow.transform.position = this.gameObject.transform.position;
                newArrow.transform.rotation = this.gameObject.transform.rotation;

                counter = 0;

                Destroy(newArrow, 10.0f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.Equals("platform.003 (4)") || collision.collider.name.Equals("platform.003 (2)"))
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (collision.collider.name.Equals("Sword_Mesh"))
        {
            hits++;
        }
    }
}
