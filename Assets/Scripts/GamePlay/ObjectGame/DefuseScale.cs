using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefuseScale : MonoBehaviour
{
    [SerializeField]
    DefuseManager DefuseManager;
    [SerializeField]
    TargetScaleScript TargetScale;

    public AnimationCurve CurveScale;

    private bool _initialized;

    public void SetDefuseScale(float scope)
    {
        if (TargetScale != null)
            SetScale(CurveScale.Evaluate(scope));
    }

    private void SetScale(float scale)
    {
        TargetScale.SetTarget(new Vector3(scale, scale, scale));
    }

    public void UpdateDefuse()
    {
        if (DefuseManager != null)
            SetDefuseScale(DefuseManager.ScopeDefuse);
    }


    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TargetScale == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TargetScale", name);
            enabled = false;
            return;
        }
        if (DefuseManager == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefuseManager", name);
            enabled = false;
            return;
        }
        TargetScale.Initialize();
        TargetScale.FilterDifference.Active = true;
        var acc = TargetScale.Accelerate;
        acc.AccelerateValue = 10;
        acc.Active = true;
        DefuseManager.EventChangeDefuse += UpdateDefuse;
    }
    void Awake()
    {
        Initialize();
    }

}
