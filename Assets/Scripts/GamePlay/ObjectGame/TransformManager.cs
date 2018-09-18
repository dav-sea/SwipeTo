using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetRotationScript))]
public class TransformManager : MonoBehaviour
{
    Transform _transform;

    private bool _initialized;

    TargetRotationScript RotationScript;

    // [SerializeField]
    // TargetScaleScript AppearanceScaleScript;

    private SpecialTarget TargetRotation;

    [SerializeField]
    private Transform Body;

    [SerializeField]
    private Transform SideSetupTarget;

    Vector3 StartSideSetup;

    public void ForceBackRotation()
    {
        SetTargetRotation(Quaternion.identity);
        SideSetupTarget.localPosition = StartSideSetup;
        SideSetupTarget.localRotation = Quaternion.identity;
    }

    public Quaternion RotatationTarget
    {
        set { SetTargetRotation(value); }
        get { return TargetRotation.RootRotation; }
    }

    public Transform Transform
    {
        get { return _transform; }
    }

    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        _transform = transform;

        RotationScript = GetComponent<TargetRotationScript>();

        TargetRotation = new SpecialTarget();
        RotationScript.SetTarget(TargetRotation);

        var acc = RotationScript.Accelerate;
        acc.Active = true;
        acc.AccelerateValue = 10;
        RotationScript.FilterDifference.Active = true;

        StartSideSetup = SideSetupTarget.localPosition;
        // var lcf = new TargetRotationScript.LocalChangeFilter(_transform.parent);
        // lcf.Active = true;
        // RotationScript.FilterTarget.AddFilter(lcf);
        // RotationScript.FilterTarget.Active = true;
    }

    // public void Show()
    // {
    //     AppearanceScaleScript.SetTarget(Vector3.one);
    // }

    // public void Hide()
    // {
    //     AppearanceScaleScript.SetTarget(Vector3.zero);
    // }

    public void AddDynamicRotation(IGettableQuaternion rotation)
    {
        if (rotation != null)
            TargetRotation.Rotations.Add(rotation);
    }

    public bool DeleteDynamicRotation(IGettableQuaternion rotation)
    {
        return TargetRotation.Rotations.Remove(rotation);
    }

    public void SetTargetRotation(Quaternion rotation)
    {
        TargetRotation.RootRotation = rotation;
        RotationScript.enabled = true;
    }

    public TargetRotationScript GetRotationScript()
    {
        return RotationScript;
    }

    [ContextMenu("RelativeRotateRight")]
    public void RelativeRotateRight(float angle = 90)
    {
        SideSetupTarget.RotateAround(Body.position, SideSetupTarget.up, angle);
        SetTargetRotation(Quaternion.AngleAxis(angle, -Vector3.up) * RotatationTarget);
    }
    [ContextMenu("RelativeRotateLeft")]
    public void RelativeRotateLeft(float angle = 90)
    {
        SideSetupTarget.RotateAround(Body.position, -SideSetupTarget.up, angle);
        SetTargetRotation(Quaternion.AngleAxis(angle, Vector3.up) * RotatationTarget);
    }
    [ContextMenu("RelativeRotateDown")]
    public void RelativeRotateDown(float angle = 90)
    {
        SideSetupTarget.RotateAround(Body.position, SideSetupTarget.right, angle);
        SetTargetRotation(Quaternion.AngleAxis(angle, -Vector3.right) * RotatationTarget);
    }
    [ContextMenu("RelativeRotateUp")]
    public void RelativeRotateUp(float angle = 90)
    {
        SideSetupTarget.RotateAround(Body.position, -SideSetupTarget.right, angle);
        SetTargetRotation(Quaternion.AngleAxis(angle, Vector3.right) * RotatationTarget);
    }


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Initialize();
    }

    private class SpecialTarget : TargetChange.ITarget<Quaternion>
    {
        public bool ClearForFinish() { return false; }

        public Quaternion RootRotation = Quaternion.identity;

        public List<IGettableQuaternion> Rotations = new List<IGettableQuaternion>();

        public Quaternion Get()
        {
            Quaternion result = RootRotation;
            foreach (IGettableQuaternion rotation in Rotations)
                if (rotation != null)
                    result *= rotation.GetQuaternion();
            return result;
        }
    }

    public interface IGettableQuaternion
    {
        Quaternion GetQuaternion();
    }
}