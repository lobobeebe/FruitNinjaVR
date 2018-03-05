using UnityEngine;

public class Anchor : MonoBehaviour
{
    public GameObject AnchoredObject;

    private Vector3 mAnchorPoint;

    private void Awake()
    {
        mAnchorPoint = AnchoredObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        Rigidbody rigidbody = AnchoredObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.velocity = Vector3.zero;
        }

        AnchoredObject.transform.position = mAnchorPoint;
	}
}
