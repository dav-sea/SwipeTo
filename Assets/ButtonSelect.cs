using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelect : MonoBehaviour
{

    [SerializeField] Appearance TargetCheck;

    public bool IsCheck { set { TargetCheck.SetAppearance(value); } get { return TargetCheck.IsShow(); } }
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TargetCheck == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TargetCheck", name);
            enabled = false;
            return;
        }
    }
    void Awake()
    {
        Initialize();
    }
}
