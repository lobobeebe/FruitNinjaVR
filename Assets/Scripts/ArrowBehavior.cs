using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowBehavior : MonoBehaviour {

    //Audio Sources
    public AudioClip[] shotSounds;
    private AudioSource[] sources;
    private float volLow = .1f;
    private float volHigh = .5f;

    public Rigidbody rb;
    public static int health = 100;
    public bool isHit = false;
    public bool fire = false;
    public static int minDamage = 1;
    public static int maxDamage = 10;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHit)
            rb.AddForce(transform.forward * 15f);
        if (health <= 0)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Shoot the player
        /*if (other.gameObject.name == ("Player"))
        {
            //health -= Random.Range(minDamage, maxDamage);
            TextMesh textObject = GameObject.Find("HealthText").GetComponent<TextMesh>();
            if (health >= 0)
                textObject.text = "Health: " + health;
            else
                textObject.text = "Health: 0";

            Destroy(gameObject);
        }
        // Reflect the bullets back into the droids
        else if (other.gameObject.name == ("katana"))
        {
            if (!isHit)
            {
                rb.velocity *= -1;
                int effect = UnityEngine.Random.Range(0, 3);
                float vol = UnityEngine.Random.Range(volLow, volHigh);
                sources[effect].PlayOneShot(shotSounds[effect], vol);
            }

            isHit = true;
        }*/
    }
}
