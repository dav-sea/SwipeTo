using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using Screen = UIOrganization.Screen;

public class ScreenBackEvent : MonoBehaviour
{
    [SerializeField]
    private Screen Target;

    public bool ActiveModule = true;

    [SerializeField]
    private UnityEvent OnBack;

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
        Target.Initialize();
        Target.EventBack += delegate { if (ActiveModule) OnBack.Invoke(); };
    }
    void Awake()
    {
        Initialize();
    }
}