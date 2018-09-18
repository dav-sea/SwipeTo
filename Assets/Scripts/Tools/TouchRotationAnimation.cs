using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchRotationAnimation : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    public DragEvent Drag;
    [SerializeField]
    TargetRotationScript TouchRotationScript;

    [SerializeField]
    TargetRotationScript MainRotationScript;

    public event System.Action<int> DragBegin;

    public event System.Action<int, Vector2, Vector2> DragAction;

    public event System.Action<int> DragEnd;

    private Transform _transform;
    private Quaternion StartRotation;

    // public ITargetObject TargetObject { private set; get; }

    // [SerializeField]
    // private bool TargetLocalTransform = true;

    public bool CreateMessagesDragActions = true;

    public bool DragAnimation;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        // if (TargetLocalTransform)
        //     SetTargetObject(transform);
        _transform = transform;
        StartRotation = _transform.rotation;
        // RotationScript = GetComponent<TargetRotationScript>();
        if (TouchRotationScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TouchRotationScript", name);
            enabled = false;
            return;
        }
        if (MainRotationScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "MainRotationScript", name);
            enabled = false;
            return;
        }
        TouchRotationScript.Initialize();
        TouchRotationScript.FilterDifference.Active = true;
        // TouchRotationScript.Accelerate.Active = true;
        // TouchRotationScript.Accelerate.AccelerateValue = 150;
        accelerateFilter = TouchRotationScript.Accelerate;
        accelerateFilter.Active = true;
        accelerateFilter.AccelerateValue = 12;
        TouchRotationScript.Corrective.Active = false;

    }

    public float VerticalFactor = 64, HorizontalFactor = 36;

    // public void SetTargetObject(Transform transform)
    // {
    //     TargetObject = new TransformTarget(transform);
    // }

    // public void SetTargetObject(ITargetObject targetObject)
    // {
    //     TargetObject = targetObject;
    // }

    int TargetID = -1;
    public void OnDrag(PointerEventData data)
    {
        if (DragAnimation)
        {
            if (TargetID < 0)
            {
                TargetID = data.pointerId;
            }
            if (data.pointerId == TargetID)
                TouchRotationScript.SetTarget(
                    Quaternion.AngleAxis(((data.pressPosition.x - data.position.x) / Screen.width) * HorizontalFactor, Vector3.up)
                    * Quaternion.AngleAxis(((data.pressPosition.y - data.position.y) / Screen.height) * VerticalFactor, Vector3.left)
                    * StartRotation);
            if (CreateMessagesDragActions)
            {
                DragAction(data.pointerId, data.pressPosition, data.position);
            }
            MainRotationScript.enabled = false;
        }
    }
    TargetRotationScript.AccelerateFilter accelerateFilter;
    public void OnBeginDrag(PointerEventData data)
    {
        if (DragAnimation)
        {
            DragBegin(data.pointerId);
        }
    }
    public void OnEndDrag(PointerEventData data)
    {
        if (DragAnimation)
        {
            // TargetObject.End();
        }
        DragEnd(data.pointerId);
        Drag.Invoke(data.position - data.pressPosition);
        StartRotation = _transform.rotation;
        // accelerateFilter.AccelerateValue = 1;
        TouchRotationScript.enabled = false;
        MainRotationScript.enabled = true;
        if (TargetID == data.pointerId) TargetID = -1;
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Initialize();
    }

    [System.Serializable]
    public class DragEvent : UnityEvent<Vector2> { }
}