using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RelativeMotion;
public class AntiPolygonRotateController : MonoBehaviour
{
    Transform Transform;
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        Transform = transform;
    }
    void Awake()
    {
        Initialize();
    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        Transform.Rotate(0, 0, 360 * Time.fixedDeltaTime);
    }
}
