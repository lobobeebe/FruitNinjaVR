using UnityEngine;

[RequireComponent(typeof(Slashable))]
public class DestroyOnSlash : MonoBehaviour
{
    private Slashable mSlashable;

	// Use this for initialization
	void Start ()
    {
        mSlashable = GetComponent<Slashable>();
        mSlashable.Slash += OnSlash;
	}

    private void OnDestroy()
    {
        mSlashable.Slash -= OnSlash;
    }

    void OnSlash()
    {
        Destroy(gameObject);
    }
}
