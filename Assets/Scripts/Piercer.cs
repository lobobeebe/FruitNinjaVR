using UnityEngine;

public class Piercer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Piercable piercable = collision.gameObject.GetComponent<Piercable>();
        if (piercable)
        {
            piercable.Pierce();
        }
    }
}
