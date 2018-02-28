using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SpawnAtObject : MonoBehaviour
{
    public GameObject SpawnPoint;

    protected virtual void Awake()
    {
        VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
    }

    protected virtual void OnDestroy()
    {
        VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
    }

    // Use this for initialization
    protected virtual void OnEnable()
    {
        Transform playArea = VRTK_DeviceFinder.PlayAreaTransform();

        if (playArea && SpawnPoint)
        {
            playArea.position = SpawnPoint.transform.position;
        }
        else
        {
            Debug.LogError("Play Area or Spawn Point undefined. Could not spawn at object.");
        }
    }
}
