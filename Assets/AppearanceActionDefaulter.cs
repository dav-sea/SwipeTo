using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIOrganization;

public class AppearanceActionDefaulter : MonoBehaviour
{
    [SerializeField] private Appearance Target;
    [SerializeField] private ActionScaler[] Scalers;
    public bool ForceDefault;
    public DefaultOn DefaultMode;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Target == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Target", name);
            enabled = false;
            return;
        }
        Target.EventShow += delegate { if (DefaultMode == DefaultOn.OnShow) Default(); };
        Target.EventHide += delegate { if (DefaultMode == DefaultOn.OnHide) Default(); };
    }
    public void Default()
    {
        Default(ForceDefault);
    }
    private void Default(bool force)
    {
        if (force)
            foreach (ActionScaler scaler in Scalers)
                scaler.SetForceNormal();
        else
            foreach (ActionScaler scaler in Scalers)
                scaler.UnScale();
    }
    void Awake()
    {
        Initialize();
    }
    public enum DefaultOn { OnShow, OnHide }
}
