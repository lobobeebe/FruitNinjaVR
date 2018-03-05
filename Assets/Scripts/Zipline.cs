using UnityEngine;
using VRTK;

public class Zipline : VRTK_InteractableObject
{
    [Header("Zipline Options", order = 4)]
    public float downStartSpeed = 0.2f;
    public float acceleration = 1.0f;
    public Transform handleEndPosition;
    public Transform handleStartPosition;
    public GameObject handle;

    private bool isMoving = false;
    private bool isMovingDown = true;

    private float currentSpeed;

    public override void OnInteractableObjectGrabbed(InteractableObjectEventArgs e)
    {
        base.OnInteractableObjectGrabbed(e);
        isMoving = true;
    }

    protected override void Awake()
    {
        base.Awake();
        currentSpeed = downStartSpeed;
    }

    protected override void Update()
    {
        base.Update();

        if (isMoving)
        {
            Vector3 moveAmount;

            Vector3 distance = handleEndPosition.position - handleStartPosition.position;

            currentSpeed += acceleration * Time.deltaTime;

            if (isMovingDown)
            {
                moveAmount = distance.normalized * currentSpeed * Time.deltaTime;
            }
            else
            {
                moveAmount = -distance.normalized * currentSpeed * Time.deltaTime;
            }
            
            if ((isMovingDown && (handle.transform.localPosition - handleEndPosition.localPosition).magnitude < moveAmount.magnitude) ||
                (!isMovingDown && (handle.transform.localPosition - handleStartPosition.localPosition).magnitude < moveAmount.magnitude))
            {
                if (isMovingDown)
                {
                    handle.transform.localPosition = handleEndPosition.position;
                }
                else
                {
                    handle.transform.localPosition = handleStartPosition.position;
                }

                isMoving = false;
                isMovingDown = !isMovingDown;
                currentSpeed = downStartSpeed;
            }
            else
            {
                handle.transform.localPosition += moveAmount;
            }
        }
    }
}