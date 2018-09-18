using System;
using UnityEngine;
using RelativeMotion;

public class RelativeTransformMotion : MonoBehaviour
{
    [SerializeField] private RelativeMotionTransformEngine TransformEngine;

    // private Transform _transform;

    public RelativeMotionTransformEngine GetEngine()
    {
        return TransformEngine;
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
    }

    public void SetTargetPosition(Vector3 target, float time)
    {
        TransformEngine.NewMotionPosition(TransformEngine.GetPosition(), target, time);
    }

    public void SetTargetRotation(Quaternion target, float time)
    {
        TransformEngine.NewMotionRotation(TransformEngine.GetRotation(), target, time);
    }

    public void SetTargetScale(Vector3 target, float time)
    {
        TransformEngine.NewMotionScale(TransformEngine.GetScale(), target, time);
    }

    void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        TransformEngine.Update(Time.deltaTime);
    }

}
