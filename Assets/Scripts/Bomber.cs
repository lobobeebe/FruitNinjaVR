using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Bombable bombable = collision.gameObject.GetComponent<Bombable>();
        if (bombable)
        {
            bombable.BombEnter();
            Destroy(gameObject);
        }
    }
}
