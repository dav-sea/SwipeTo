using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchAnimationController : MonoBehaviour
{
    // private TargetRotationScript BackScript;
    private TouchRotationAnimation TouchRotationAnimation;

    private bool _initialized;

    public bool DragAnimation
    {
        set { TouchRotationAnimation.DragAnimation = value; }
        get { return TouchRotationAnimation.DragAnimation; }
    }

    public TouchRotationAnimation GetTouchRotation()
    {
        return TouchRotationAnimation;
    }

    public void Initialize()
    {
        if (_initialized) return;

        _initialized = true;
        TouchRotationAnimation = GetComponent<TouchRotationAnimation>();

    }

    void Awake()
    {
        Initialize();
    }
}
