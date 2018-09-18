using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceActiveTime : MonoBehaviour
{
    [SerializeField] Appearance Target;
    public int IntervalPostHide = 3;

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
        TargetGameObject = Target.gameObject;
    }

    private GameObject TargetGameObject;
    void Awake()
    {
        Initialize();
    }
    private DeferredAction.IAction PostDisactive;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Target.EventHide += delegate
        {
            if (PostDisactive != null)
                PostDisactive.Cancel();
            PostDisactive = new DeferredAction.OnceAction(DisactiveTarget, IntervalPostHide);
            DeferredAction.Manager.AddDeferredAction(PostDisactive);
        };
        Target.EventShow += delegate
        {
            if (PostDisactive != null)
                PostDisactive.Cancel();
            TargetGameObject.SetActive(true);
        };
    }

    public void DisactiveTarget()
    {
        TargetGameObject.SetActive(false);
    }
}
