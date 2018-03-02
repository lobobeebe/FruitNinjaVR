using UnityEngine;

public class Slashable : MonoBehaviour
{
    public delegate void SlashHandler();
    public SlashHandler OnSlash;

    public void Slash()
    {
        OnSlash();
    }
}
