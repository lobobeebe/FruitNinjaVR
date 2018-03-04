using UnityEngine;
using VRTK;

public class SwordController : MonoBehaviour
{
    public bool IsRightHand = true;

    public GameObject SwordPrefab;
    public GameObject ShurikenPrefab;

    private GameObject mSword;
    private GameObject mShuriken;

    enum HandState
    {
        Idle,
        Sword,
        Shuriken
    }
    
    private HandState mHandState;

    // Use this for initialization
    void Start ()
    {
        InitListeners(GetController(), true);

        mSword = Instantiate(SwordPrefab, GetController().transform);
        mSword.SetActive(false);

        mShuriken = Instantiate(SwordPrefab, GetController().transform);
        mShuriken.SetActive(false);
    }

    private void InitListeners(GameObject controller, bool state)
    {
        if (state)
        {
            var controllerEvents = controller.GetComponent<VRTK_ControllerEvents>();
            if (state)
            {
                if (controllerEvents)
                {
                    controllerEvents.TriggerClicked += OnTriggerClicked;
                    controllerEvents.TriggerReleased += OnTriggerReleased;
                    controllerEvents.StartMenuPressed += OnStartMenuPressed;
                }
            }
            else
            {
                if (controllerEvents)
                {
                    controllerEvents.TriggerClicked -= OnTriggerClicked;
                    controllerEvents.TriggerReleased -= OnTriggerReleased;
                    controllerEvents.StartMenuPressed -= OnStartMenuPressed;
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
            
            Instantiate(SwordPrefab, mShuriken.transform.position, 
                mShuriken.transform.rotation);
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

    private GameObject GetController()
    {
        return IsRightHand ? VRTK_DeviceFinder.GetControllerRightHand() : VRTK_DeviceFinder.GetControllerLeftHand();
    }
}
