using UnityEngine;

public class Anchor : MonoBehaviour
{
    public GameObject AnchoredObject;

    private Vector3 mAnchorPoint;

    private void Start()
    {
        mAnchorPoint = AnchoredObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        AnchoredObject.transform.position = mAnchorPoint;
	}
}
