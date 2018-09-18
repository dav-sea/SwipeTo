using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SwitchDragBox : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    [SerializeField] SwitcherObject SwitcherObject;

    Transform _transform;

    public void OnDrag(PointerEventData data)
    {
        SwitcherObject.ScopeSwitch = data.pressEventCamera.ScreenToViewportPoint(data.position).y;
        // data.pressEventCamera.WorldToViewportPoint(_transform.position);
        // SwitcherObject.ScopeSwitch = data.pressEventCamera.WorldToViewportPoint(_transform.position).y;
    }
    public void OnBeginDrag(PointerEventData data)
    {
        // SwitcherObject.ActiveRotationScript = false;
    }
    public void OnEndDrag(PointerEventData data)
    {
        // SwitcherObject.ActiveRotationScript = true;
    }



    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (SwitcherObject == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "SwitcherObject", name);
            enabled = false;
            return;
        }
        _transform = transform;
    }
    void Awake()
    {
        Initialize();
    }
}
