using UnityEngine;

public class Piercable : MonoBehaviour
{
    public delegate void PierceHandler();
    public PierceHandler OnPierce;
    
    public void Pierce()
    {
        OnPierce();
    }
}
