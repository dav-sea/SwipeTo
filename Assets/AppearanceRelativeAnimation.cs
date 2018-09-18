using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RelativeMotion;

public class AppearanceRelativeAnimation : MonoBehaviour
{
    [SerializeField] Appearance Target;
    [SerializeField] RelativeTransformMotion TransformMotion;

    private RelativeMotionTransformEngine Engine;

    [Space(5)]
    [SerializeField]
    AnimationKey ShowState;
    [SerializeField]
    AnimationKey HideState;

    private void SetAnimationKey(AnimationKey key)
    {
        if (key == null) return;
        SetPositionKey(key.Position);
        SetRotationKey(key.Rotation);
        SetScaleKey(key.Scale);
        Engine.Active = true;
    }

    public void Show()
    {
        SetAnimationKey(ShowState);
    }

    public void Hide()
    {
        SetAnimationKey(HideState);
    }

    private void SetPositionKey(AnimationKey.InspectorMotionDataVector3 key)
    {
        if (key == null || !key.Changeble) return;

        if (key.ModeStart == AnimationKey.StartMode.ConditionRelative)
        {
            float time = 1 - Engine.PositionEngine.TimeCurrent;
            Engine.NewMotionPosition(key);
            Engine.PositionEngine.TimeCurrent = time;
            return;
        }
        else if (key.ModeStart == AnimationKey.StartMode.Relative)
        {
            Engine.NewMotionPosition(key);
            Engine.PositionEngine.Start = TransformMotion.GetEngine().GetPosition();
        }
        else Engine.NewMotionPosition(key);
    }
    private void SetRotationKey(AnimationKey.InspectorMotionDataQuaternion key)
    {
        if (key == null || !key.Changeble) return;

        if (key.ModeStart == AnimationKey.StartMode.ConditionRelative)
        {
            float time = 1 - Engine.RotationEngine.TimeCurrent;

            Engine.NewMotionRotation(key);
            Engine.RotationEngine.TimeCurrent = time;
            return;
        }
        else if (key.ModeStart == AnimationKey.StartMode.Relative)
        {
            Engine.NewMotionRotation(key);
            Engine.RotationEngine.Start = TransformMotion.GetEngine().GetRotation();
        }
        else Engine.NewMotionRotation(key);
    }
    private void SetScaleKey(AnimationKey.InspectorMotionDataVector3 key)
    {
        if (key == null || !key.Changeble) return;

        if (key.ModeStart == AnimationKey.StartMode.ConditionRelative)
        {
            float time = 1 - Engine.ScaleEngine.TimeCurrent;
            Engine.NewMotionScale(key);
            Engine.ScaleEngine.TimeCurrent = time;
            return;
        }
        else if (key.ModeStart == AnimationKey.StartMode.Relative)
        {
            Engine.NewMotionScale(key);
            Engine.ScaleEngine.Start = TransformMotion.GetEngine().GetScale();
        }
        else Engine.NewMotionScale(key);
    }

    // private float Avg(params float[] value)
    // {
    //     float summ = value[0];
    //     for (int i = 1; i < value.Length; ++i) summ += value[i];
    //     return summ / value.Length;
    // }

    // private float Distance(float a, float b)
    // {
    //     return Mathf.Max(a, b) - Mathf.Min(a, b);
    // }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TransformMotion == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TransformMotion", name);
            enabled = false;
            return;
        }
        Engine = TransformMotion.GetEngine();
    }
    void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Target.EventShow += Show;
        Target.EventHide += Hide;
        Hide();
    }

    [System.Serializable]
    public class AnimationKey
    {
        [Space(5)]
        public InspectorMotionDataVector3 Position;
        [Space(5)]
        public InspectorMotionDataQuaternion Rotation;
        [Space(5)]
        public InspectorMotionDataVector3 Scale;
        public class AnimationKeyField<T> : MotionData<T>
        {
            public bool Changeble = true;
            public StartMode ModeStart = StartMode.ConditionRelative;
            public AnimationKeyField()
            : base(default(T), default(T)) { }
        }

        [System.Serializable]
        public class InspectorMotionDataVector3 : AnimationKeyField<Vector3>
        {
        }
        [System.Serializable]
        public class InspectorMotionDataQuaternion : AnimationKeyField<Quaternion>
        {
        }

        public enum StartMode { Lock, Relative, ConditionRelative };
    }
}
