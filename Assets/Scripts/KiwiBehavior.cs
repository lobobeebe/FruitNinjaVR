using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiwiBehavior : MonoBehaviour {

    private GameObject player;
    private GameObject spray;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("RightController");
        spray = this.gameObject.transform.Find("SteamSpray").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= PatrollingBehavior.aggroRadius)
        {
            spray.SetActive(true);
            this.gameObject.transform.LookAt(player.transform);
            this.gameObject.transform.position += transform.forward * Time.deltaTime * VoiceController.speed;
        }
        else
        {
            spray.SetActive(false);
        }
    }
}
