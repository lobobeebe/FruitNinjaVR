using UnityEngine;
using VRTK;

public class SwordController : MonoBehaviour
{
    public bool IsRightHand = true;
    
    public GameObject SwordPrefab;
    public GameObject ShurikenPrefab;

    public float ShurikenVelocityScale = 500;

    private GameObject mSword;
    private GameObject mShuriken;

    private Vector3 mLastLocation;
    private Vector3 mVelocity;

    enum HandState
    {
        Idle,
        Sword,
        Shuriken
    }
    
    private HandState mHandState;

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
        GameObject controller = GetController();

        if (controller)
        {
            InitListeners(controller, true);

            mSword = Instantiate(SwordPrefab, controller.transform);
            mSword.SetActive(false);

            mShuriken = Instantiate(ShurikenPrefab, controller.transform);
            Rigidbody rigidbody = mShuriken.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;

            mShuriken.SetActive(false);
        }
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
                    controllerEvents.TriggerClicked += OnTriggerClicked;
                    controllerEvents.TriggerReleased += OnTriggerReleased;
                    controllerEvents.ButtonTwoPressed += OnStartMenuPressed;
                }
            }
            else
            {
                if (controllerEvents)
                {
                    controllerEvents.TriggerClicked -= OnTriggerClicked;
                    controllerEvents.TriggerReleased -= OnTriggerReleased;
                    controllerEvents.ButtonTwoPressed -= OnStartMenuPressed;
                }
            }
        }
    }

    public void OnTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        if (mHandState == HandState.Idle)
        {
            mHandState = HandState.Shuriken;
            mShuriken.SetActive(true);
        }
    }

    public void OnTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (mHandState == HandState.Shuriken)
        {
            mHandState = HandState.Idle;
            
            GameObject shuriken = Instantiate(ShurikenPrefab, mShuriken.transform.position, 
                mShuriken.transform.rotation);
            Rigidbody rigidbody = shuriken.GetComponent<Rigidbody>();
            rigidbody.velocity = mVelocity * ShurikenVelocityScale;

            mShuriken.SetActive(false);
        }
    }

    public void OnStartMenuPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (mHandState == HandState.Idle)
        {
            mHandState = HandState.Sword;
            mSword.SetActive(true);
        }
        else if (mHandState == HandState.Sword)
        {
            mHandState = HandState.Idle;
            mSword.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        mVelocity = (GetController().transform.position - mLastLocation) / Time.deltaTime;
        mLastLocation = GetController().transform.position;
    }

    private GameObject GetController()
    {
        return IsRightHand ? VRTK_DeviceFinder.GetControllerRightHand() : VRTK_DeviceFinder.GetControllerLeftHand();
    }
}
