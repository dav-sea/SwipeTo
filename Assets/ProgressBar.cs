using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public event System.Action EventChangeProgress;
    private float progress;
    public float Progress
    {
        set
        {
            if (progress == value) return;
            progress = Mathf.Clamp(value, 0, 1);
            EventChangeProgress();
        }

        get { return progress; }
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
    }

    public

    void Awake()
    {
        Initialize();
    }
}
