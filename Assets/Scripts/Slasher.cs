using UnityEngine;

public class Slasher : MonoBehaviour
{
    public float VelocityThreshold;
    public float AngularVelocityThreshold;

    private Vector3 mVelocity;
    private Vector3 mAngularVelocity;

    private Vector3 mLastPosition;
    private Vector3 mLastEulerAngles;

    private void FixedUpdate()
    {
        mVelocity = transform.position - mLastPosition;
        mLastPosition = transform.position;

        mAngularVelocity = transform.rotation.eulerAngles - mLastEulerAngles;
        mLastEulerAngles = transform.rotation.eulerAngles;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (mVelocity.magnitude > VelocityThreshold ||
            mAngularVelocity.magnitude > AngularVelocityThreshold)
        {
            Slashable Slashable = collision.gameObject.GetComponent<Slashable>();
            if (Slashable)
            {
                Slashable.Slash();
            }
        }
    }
}
