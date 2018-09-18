using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class AppearanceScale : MonoBehaviour
{
    [SerializeField] Appearance Target;
    [SerializeField] Vector3 ShowScale = Vector3.one, HideScale = Vector3.zero;
    [SerializeField] TargetScaleScript ScaleScript;

    public bool Active = true;

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
        if (ScaleScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ScaleScript", name);
            enabled = false;
            return;
        }
        ScaleScript.Initialize();
        ScaleScript.DisableForFinish = true;
        ScaleScript.Accelerate.AccelerateValue = 10;
        Target.EventShow += Show;
        Target.EventHide += Hide;
    }
    void Awake()
    {
        Initialize();
    }

    private void Show()
    {
        ScaleScript.SetTarget(ShowScale);
    }

    private void Hide()
    {
        ScaleScript.SetTarget(HideScale);
    }

    [ContextMenu("Find All")]
    public void FindAll()
    {
        Target = GetComponent<Appearance>();
        ScaleScript = GetComponent<TargetScaleScript>();
    }
}