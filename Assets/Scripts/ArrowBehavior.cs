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

    private int tennis = 0;
    private GameObject player;
    private GameObject boss;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sources = GetComponents<AudioSource>();
        player = GameObject.Find("RightController");
        boss = GameObject.Find("Pineapple");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHit && this.gameObject.name.Equals("Arrow(Clone)"))
            rb.AddForce(transform.forward * 15f);
        else if (!isHit && this.gameObject.name.Equals("StrawberryBullet(Clone)"))
        {
            this.gameObject.transform.LookAt(player.transform);
            this.gameObject.transform.position += transform.forward * Time.deltaTime * 15.0f * VoiceController.speed;
        }
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
        }*/
        // Reflect the strawberry back into the boss
        if (other.gameObject.name == ("Sword_Mesh") && this.gameObject.name.Equals("Arrow(Clone)"))
        {
            if (!isHit)
            {
                rb.velocity = new Vector3(0, 1.0f, 0);
                int effect = UnityEngine.Random.Range(0, 3);
                float vol = UnityEngine.Random.Range(volLow, volHigh);
                sources[effect].PlayOneShot(shotSounds[effect], vol);
            }

            isHit = true;
        }
        if (other.gameObject.name == ("RightController") && this.gameObject.name.Equals("StrawberryBullet(Clone)"))
        {
            Debug.Log(other.gameObject.name);
            if (!isHit)
            {
                Debug.Log("I want to fly away to the boss now");
                this.gameObject.transform.LookAt(boss.transform);
                this.gameObject.transform.position += transform.forward * Time.deltaTime * 15.0f * VoiceController.speed;
                //int effect = UnityEngine.Random.Range(0, 3);
                //float vol = UnityEngine.Random.Range(volLow, volHigh);
                //sources[effect].PlayOneShot(shotSounds[effect], vol);
            }
            isHit = true;
        }
        else if (other.gameObject.name == ("Pineapple"))
        {
            if (tennis < 3)
            {
                tennis++;
                isHit = false;
                this.gameObject.transform.LookAt(player.transform);
                this.gameObject.transform.position += transform.forward * Time.deltaTime * 15.0f * VoiceController.speed;
            }
            if (tennis == 3)
            {
                BossBehavior.hits++;
                tennis = 0;
                Destroy(this.gameObject);
            }
        }
    }
}
