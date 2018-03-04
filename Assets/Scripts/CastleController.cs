using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CastleController : MonoBehaviour {

    private void Awake()
    {
        GetComponent<Renderer>().enabled = false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.name.Equals("Body"))
        {
            SceneManager.LoadScene("Castle", LoadSceneMode.Single);
        }
    }
}
