using UnityEngine;

[RequireComponent(typeof(Slashable))]
public class DestroyOnSlash : MonoBehaviour
{
    private Slashable mSlashable;

	// Use this for initialization
	void Start ()
    {
        mSlashable = GetComponent<Slashable>();
        mSlashable.OnSlash += OnSlash;
	}

    void OnSlash()
    {
        Destroy(this);
    }
}
