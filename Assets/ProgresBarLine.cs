using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgresBarLine : MonoBehaviour
{
    [SerializeField] private ProgressBar Target;
    [SerializeField] private TargetScaleScript Slider;

    [SerializeField]
    [Range(0, 1)]
    private float Min = 0.1f;
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
        if (Slider == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Slider", name);
            enabled = false;
            return;
        }
        Target.EventChangeProgress += UpdateVisual;
        Slider.Initialize();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        SetSlider(Target.Progress);
    }

    private void SetSlider(float scale)
    {
        Slider.SetTarget(new Vector3((scale + Min) / (1 + Min), 1, 1));
    }

    void Awake()
    {
        Initialize();
    }
}
