using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;

public class PullLocomotion : MonoBehaviour
{
    [Header("Climb Settings")]
    [Tooltip("Will scale movement up and down based on the player transform's scale.")]
    public bool usePlayerScale = true;

    [Header("Pull Settings")]
    [Tooltip("The factor by which a pulling motion's velocity will be multiplied before applying a ground movement to the body.")]
    public float PullForceFactor = 1f;
    [Tooltip("The minimum controller x/z velocity to register as movement. Must be a positive number.")]
    public float PullThreshold = .005f;
    [Tooltip("The factor by which a pulling motion's velocity will be multiplied before applying a jump velocity to the body.")]
    public float JumpForceFactor = 100f;
    [Tooltip("The minimum controller Y velocity to register as a jump. Must be a positive number.")]
    public float JumpYThreshold = .035f;

    [Header("Custom Settings")]
    [Tooltip("The VRTK Body Physics script to use for dealing with climbing and falling. If this is left blank then the script will need to be applied to the same GameObject.")]
    public VRTK_BodyPhysics BodyPhysics;
    [Tooltip("The VRTK Headset Collision script to use for determining if the user is climbing inside a collidable object. If this is left blank then the script will need to be applied to the same GameObject.")]
    public VRTK_HeadsetCollision HeadsetCollision;
    [Tooltip("The VRTK Position Rewind script to use for dealing resetting invalid positions. If this is left blank then the script will need to be applied to the same GameObject.")]
    public VRTK_PositionRewind PositionRewind;

    private Transform mPlayArea;

    // Climb
    private Vector3 mStartControllerScaledLocalPosition;
    private Vector3 mStartGrabPointLocalPosition;
    private Vector3 mStartPlayAreaWorldOffset;
    private GameObject mGrabbingController;
    private GameObject mClimbingObject;
    private Quaternion mClimbingObjectLastRotation;
    private bool mIsClimbing;
    private bool mUseGrabbedObjectRotation;

    // Pull
    private bool mIsPulling;
    private GameObject mPullingController;
    private Vector3 mLastPullingPosition;


    protected virtual void Awake()
    {
        BodyPhysics = (BodyPhysics != null ? BodyPhysics : GetComponentInChildren<VRTK_BodyPhysics>());
        HeadsetCollision = (HeadsetCollision != null ? HeadsetCollision : GetComponentInChildren<VRTK_HeadsetCollision>());
        PositionRewind = (PositionRewind != null ? PositionRewind : GetComponentInChildren<VRTK_PositionRewind>());

        VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
    }

    protected virtual void OnDestroy()
    {
        VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
    }

    protected virtual void OnEnable()
    {
        mPlayArea = VRTK_DeviceFinder.PlayAreaTransform();
        InitListeners(true);
    }

    protected virtual void OnDisable()
    {
        Ungrab(false, null, mClimbingObject);
        InitListeners(false);
    }

    protected virtual void InitListeners(bool state)
    {
        InitControllerListeners(VRTK_DeviceFinder.GetControllerLeftHand(), state);
        InitControllerListeners(VRTK_DeviceFinder.GetControllerRightHand(), state);
    }

    protected virtual void InitControllerListeners(GameObject controller, bool state)
    {
        if (controller)
        {
            var grabScript = controller.GetComponent<VRTK_InteractGrab>();
            var controllerEvents = controller.GetComponent<VRTK_ControllerEvents>();
            if (state)
            {
                if (grabScript)
                {
                    grabScript.ControllerGrabInteractableObject += new ObjectInteractEventHandler(OnGrabObject);
                    grabScript.ControllerUngrabInteractableObject += new ObjectInteractEventHandler(OnUngrabObject);
                }
                if (controllerEvents)
                {
                    controllerEvents.GripPressed += new ControllerInteractionEventHandler(OnGripPressed);
                    controllerEvents.GripReleased += new ControllerInteractionEventHandler(OnGripReleased);
                }
            }
            else
            {
                if (grabScript)
                {
                    grabScript.ControllerGrabInteractableObject -= new ObjectInteractEventHandler(OnGrabObject);
                    grabScript.ControllerUngrabInteractableObject -= new ObjectInteractEventHandler(OnUngrabObject);
                }
                if (controllerEvents)
                {
                    controllerEvents.GripPressed -= new ControllerInteractionEventHandler(OnGripPressed);
                    controllerEvents.GripReleased -= new ControllerInteractionEventHandler(OnGripReleased);
                }
            }
        }
    }

    protected virtual bool IsActiveClimbingController(GameObject controller)
    {
        return (controller == mGrabbingController);
    }

    protected virtual bool IsClimbableObject(GameObject obj)
    {
        var interactObject = obj.GetComponent<VRTK_InteractableObject>();
        return (interactObject && interactObject.grabAttachMechanicScript && interactObject.grabAttachMechanicScript.IsClimbable());
    }

    protected virtual void OnGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        var controller = ((VRTK_ControllerEvents)sender).gameObject;
        var actualController = VRTK_DeviceFinder.GetActualController(controller);
        Pull(actualController);
    }

    protected virtual void OnGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        var controller = ((VRTK_ControllerEvents)sender).gameObject;
        var actualController = VRTK_DeviceFinder.GetActualController(controller);
        Release(actualController);
    }

    protected virtual void Pull(GameObject actualController)
    {
        mIsPulling = true;

        mPullingController = actualController;
        mLastPullingPosition = GetScaledLocalPosition(mPullingController.transform);
    }

    protected virtual void Release(GameObject actualController)
    {
        if (actualController == mPullingController)
        {
            mIsPulling = false;

            mPullingController = null;
        }
    }

    protected virtual void OnGrabObject(object sender, ObjectInteractEventArgs e)
    {
        if (IsClimbableObject(e.target))
        {
            var controller = ((VRTK_InteractGrab)sender).gameObject;
            var actualController = VRTK_DeviceFinder.GetActualController(controller);
            Grab(actualController, e.controllerReference, e.target);
        }
    }

    protected virtual void OnUngrabObject(object sender, ObjectInteractEventArgs e)
    {
        var controller = ((VRTK_InteractGrab)sender).gameObject;
        var actualController = VRTK_DeviceFinder.GetActualController(controller);
        if (e.target && IsClimbableObject(e.target) && IsActiveClimbingController(actualController))
        {
            Ungrab(true, e.controllerReference, e.target);
        }
    }

    protected virtual void Grab(GameObject currentGrabbingController, VRTK_ControllerReference controllerReference, GameObject target)
    {
        BodyPhysics.ResetFalling();
        BodyPhysics.TogglePreventSnapToFloor(true);
        BodyPhysics.enableBodyCollisions = false;
        BodyPhysics.ToggleOnGround(false);

        mIsClimbing = true;
        mClimbingObject = target;
        mGrabbingController = currentGrabbingController;
        mStartControllerScaledLocalPosition = GetScaledLocalPosition(mGrabbingController.transform);
        mStartGrabPointLocalPosition = mClimbingObject.transform.InverseTransformPoint(mGrabbingController.transform.position);
        mStartPlayAreaWorldOffset = mPlayArea.transform.position - mGrabbingController.transform.position;
        mClimbingObjectLastRotation = mClimbingObject.transform.rotation;
        mUseGrabbedObjectRotation = mClimbingObject.GetComponent<VRTK_ClimbableGrabAttach>().useObjectRotation;
    }

    protected virtual void Ungrab(bool carryMomentum, VRTK_ControllerReference controllerReference, GameObject target)
    {
        mIsClimbing = false;
        if (PositionRewind != null && IsHeadsetColliding())
        {
            PositionRewind.RewindPosition();
        }
        if (IsBodyColliding() && !IsHeadsetColliding())
        {
            BodyPhysics.ForceSnapToFloor();
        }

        BodyPhysics.enableBodyCollisions = true;

        if (carryMomentum)
        {
            Vector3 velocity = Vector3.zero;

            if (VRTK_ControllerReference.IsValid(controllerReference))
            {
                velocity = -VRTK_DeviceFinder.GetControllerVelocity(controllerReference);
                if (usePlayerScale)
                {
                    velocity = mPlayArea.TransformVector(velocity);
                }
                else
                {
                    velocity = mPlayArea.TransformDirection(velocity);
                }
            }

            BodyPhysics.ApplyBodyVelocity(velocity, true, true);
        }

        mGrabbingController = null;
        mClimbingObject = null;
    }

    protected virtual Vector3 GetScaledLocalPosition(Transform objTransform)
    {
        if (usePlayerScale)
        {
            return mPlayArea.localRotation * Vector3.Scale(objTransform.localPosition, mPlayArea.localScale);
        }

        return mPlayArea.localRotation * objTransform.localPosition;
    }

    protected virtual bool IsBodyColliding()
    {
        return (BodyPhysics != null && BodyPhysics.GetCurrentCollidingObject() != null);
    }

    protected virtual bool IsHeadsetColliding()
    {
        return (HeadsetCollision != null && HeadsetCollision.IsColliding());
    }
    
    protected virtual void Update()
    {
        if (mIsClimbing)
        {
            Vector3 controllerLocalOffset = GetScaledLocalPosition(mGrabbingController.transform) - mStartControllerScaledLocalPosition;
            Vector3 grabPointWorldPosition = mClimbingObject.transform.TransformPoint(mStartGrabPointLocalPosition);
            mPlayArea.position = grabPointWorldPosition + mStartPlayAreaWorldOffset - controllerLocalOffset;

            if (mUseGrabbedObjectRotation)
            {
                Vector3 lastRotationVec = mClimbingObjectLastRotation * Vector3.forward;
                Vector3 currentObectRotationVec = mClimbingObject.transform.rotation * Vector3.forward;
                Vector3 axis = Vector3.Cross(lastRotationVec, currentObectRotationVec);
                float angle = Vector3.Angle(lastRotationVec, currentObectRotationVec);

                mPlayArea.RotateAround(grabPointWorldPosition, axis, angle);
                mClimbingObjectLastRotation = mClimbingObject.transform.rotation;
            }

            if (PositionRewind != null && !IsHeadsetColliding())
            {
                PositionRewind.SetLastGoodPosition();
            }
        }
        else if (mIsPulling)
        {
            Vector3 controllerLocalOffset = GetScaledLocalPosition(mPullingController.transform) - mLastPullingPosition;

            Vector3 pullGroundVelocity = -controllerLocalOffset * PullForceFactor;
            pullGroundVelocity.y = 0;

            float pullJumpVelocity = -controllerLocalOffset.y;

            if (BodyPhysics.OnGround())
            {
                if (pullJumpVelocity > JumpYThreshold)
                {
                    BodyPhysics.ApplyBodyVelocity(new Vector3(pullGroundVelocity.x * JumpForceFactor, pullJumpVelocity * JumpForceFactor, pullGroundVelocity.x * JumpForceFactor));
                }
                else if (pullGroundVelocity.magnitude > PullThreshold)
                {
                    mPlayArea.position += pullGroundVelocity;
                }
            }

            mLastPullingPosition = GetScaledLocalPosition(mPullingController.transform);

            Debug.Log("Pulling");
        }
    }
}
