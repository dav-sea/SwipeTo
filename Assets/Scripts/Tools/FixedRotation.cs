using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotation : MonoBehaviour
{
    [SerializeField]
    Transform _transform;

    [SerializeField]
    Vector3 Rotation;

    public bool AxisX = true, AxisY = true, AxisZ = true;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        _transform = transform;

    }
    void Awake()
    {
        Initialize();
    }

    [ContextMenu("UpdateRotation")]
    public void UpdateRotation()
    {
        _transform.rotation = Quaternion.Euler(AxisX ? Rotation.x : _transform.rotation.x, AxisY ? Rotation.y : _transform.rotation.y, AxisZ ? Rotation.z : _transform.rotation.z);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // UpdateRotation();
    }
}
