using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bombable))]
public class AppleShogunController : MonoBehaviour
{
    private Bombable mBombable;

    public void Start()
    {
        mBombable = GetComponent<Bombable>();

        if (mBombable)
        {
            mBombable.BombEnter += OnBombEnter;
        }
    }

    private void OnDestroy()
    {
        mBombable.BombEnter -= OnBombEnter;
    }

    public void OnBombEnter()
    {
        PlotController.AppleShogunDefeated = true;
        Destroy(gameObject);
    }
}
