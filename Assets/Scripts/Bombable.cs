using UnityEngine;
using System.Collections;

public class Bombable : MonoBehaviour
{
    public delegate void BombHandler();
    public BombHandler BombEnter = delegate { };
}
