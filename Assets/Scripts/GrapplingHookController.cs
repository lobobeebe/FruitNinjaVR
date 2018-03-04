using UnityEngine;
using VRTK;
using VRTK.Examples;

public class GrapplingHookController : MonoBehaviour
{
    public bool IsRightHand;
    public GameObject GrapplingGunPrefab;
    public GameObject HighlightPylonPrefab;
    public GameObject PylonPrefab;
    public GameObject RopePrefab;
    public GameObject HandlePrefab;

    public float PylonHeight = 2.5f;

    enum HookState
    {
        Off,
        Idle,
        Targeting,
        Sourcing
    }

    private HookState mHookState;

    private GameObject mGrapplingGun;

    private GameObject mTempPylon;
    private GameObject mTargetPylon;
    private GameObject mSourcePylon;
    private GameObject mRope;
    private GameObject mHandle;
    private GameObject mStartPosition;
    private GameObject mEndPosition;

    protected virtual void Awake()
    {
        VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
    }

    protected virtual void OnDestroy()
    {
        VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
    }

    protected virtual void OnEnable()
    {
        InitListeners(GetController(), true);
    }

    protected virtual void OnDisable()
    {
        InitListeners(GetController(), false);
    }

    private void InitListeners(GameObject controller, bool state)
    {
        if (controller)
        {
            var controllerEvents = controller.GetComponent<VRTK_ControllerEvents>();

            if (state)
            {
                if (controllerEvents)
                {
                    controllerEvents.TriggerHairlineStart += OnTriggerHairlineStart;
                    controllerEvents.TriggerClicked += OnTriggerClicked;
                    controllerEvents.TriggerReleased += OnTriggerReleased;
                    controllerEvents.ButtonTwoPressed += OnStartMenuPressed;
                }
            }
            else
            {
                if (controllerEvents)
                {
                    controllerEvents.TriggerHairlineStart -= OnTriggerHairlineStart;
                    controllerEvents.TriggerClicked -= OnTriggerClicked;
                    controllerEvents.TriggerReleased -= OnTriggerReleased;
                    controllerEvents.ButtonTwoPressed -= OnStartMenuPressed;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (mHookState == HookState.Targeting || mHookState == HookState.Sourcing)
        {
            RaycastHit hitInfo;
            GameObject controller = GetController();

            if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hitInfo))
            {
                if (mTempPylon != null)
                {
                    mTempPylon.transform.position = hitInfo.point;
                    mTempPylon.transform.rotation = Quaternion.LookRotation(Vector3.forward, hitInfo.normal);
                }
            }
        }
    }

    public void OnTriggerHairlineStart(object sender, ControllerInteractionEventArgs e)
    {
        if (mHookState == HookState.Idle)
        {
            DestroyItems();

            mTempPylon = Instantiate(HighlightPylonPrefab);

            Vector3 scale = mTempPylon.transform.localScale;
            scale.y = PylonHeight;
            mTempPylon.transform.localScale = scale;

            mHookState = HookState.Targeting;
        }
    }

    public void OnTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        if (mHookState == HookState.Targeting)
        {
            // Create Target Pylon
            mTargetPylon = Instantiate(PylonPrefab, mTempPylon.transform.position, mTempPylon.transform.rotation);

            Vector3 scale = mTargetPylon.transform.localScale;
            scale.y = PylonHeight;
            mTargetPylon.transform.localScale = scale;

            mHookState = HookState.Sourcing;
        }
        else if (mHookState == HookState.Sourcing)
        {
            // Create Source Pylon
            mSourcePylon = Instantiate(PylonPrefab, mTempPylon.transform.position, mTempPylon.transform.rotation);

            Vector3 scale = mSourcePylon.transform.localScale;
            scale.y = PylonHeight;
            mSourcePylon.transform.localScale = scale;

            Vector3 ropeStartPos = mSourcePylon.transform.position + mSourcePylon.transform.up * PylonHeight * .9f;
            Vector3 ropeEndPos = mTargetPylon.transform.position + mTargetPylon.transform.up * PylonHeight * .9f;
            Vector3 ropeLengthDistance = ropeEndPos - ropeStartPos;
            Vector3 ropeLook = ropeLengthDistance.normalized;

            mRope = Instantiate(RopePrefab, ropeStartPos, Quaternion.LookRotation(ropeLook));
            scale = mRope.transform.localScale;
            scale.z = ropeLengthDistance.magnitude;
            mRope.transform.localScale = scale;

            mStartPosition = new GameObject("StartPosition");
            mStartPosition.transform.position = ropeStartPos + ropeLook * .75f;

            mEndPosition = new GameObject("EndPosition");
            mEndPosition.transform.position = ropeEndPos - ropeLook * .75f;

            mHandle = Instantiate(HandlePrefab, mStartPosition.transform.position, Quaternion.LookRotation(ropeLook));
            Zipline ziplineObj = mHandle.GetComponentInChildren<Zipline>();
            ziplineObj.handleStartPosition = mStartPosition.transform;
            ziplineObj.handleEndPosition = mEndPosition.transform;

            Destroy(mTempPylon);
            mTempPylon = null;

            mHookState = HookState.Idle;
        }
    }

    public void OnTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        // Delete?
    }

    public void DestroyItems()
    {
        if (mTempPylon)
        {
            Destroy(mTempPylon);
            mTempPylon = null;
        }

        if (mTargetPylon)
        {
            Destroy(mTargetPylon);
            mTargetPylon = null;
        }

        if (mSourcePylon)
        {
            Destroy(mSourcePylon);
            mSourcePylon = null;
        }

        if (mRope)
        {
            Destroy(mRope);
            mRope = null;
        }

        if (mHandle)
        {
            Destroy(mHandle);
            mHandle = null;
        }
    }

    public void OnStartMenuPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (mHookState != HookState.Off)
        {
            Destroy(mGrapplingGun);
            mGrapplingGun = null;

            mHookState = HookState.Off;
        }
        else
        {
            mGrapplingGun = Instantiate(GrapplingGunPrefab, GetController().transform);

            mHookState = HookState.Idle;
        }
    }

    private GameObject GetController()
    {
        return IsRightHand ? VRTK_DeviceFinder.GetControllerRightHand() : VRTK_DeviceFinder.GetControllerLeftHand();
    }
}
