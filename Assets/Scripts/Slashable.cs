using UnityEngine;

public class Slashable : MonoBehaviour
{
    public delegate void SlashHandler();
    public SlashHandler Slash = delegate { };
}
