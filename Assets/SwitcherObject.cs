using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitcherObject : MonoBehaviour
{
    public event System.Action EventChangeSwitchPosition;
    [SerializeField] SwitcherConfiguration Switcher;
    [SerializeField] private int Max = 1;
    private SwitchPositionSettings SwitchSettings = new SwitchPositionSettings();

    private Transform _transform;
    public Transform Transform
    {
        get { if (_transform == null) _transform = transform; return _transform; }
    }

    public bool ActiveRotationScript { set { Switcher.RotationScript.enabled = value; } get { return Switcher.RotationScript.enabled; } }

    public int SwitchPosition
    {
        set
        {
            if (value == SwitchSettings.SwitchPosition) return;
            SwitchSettings.SwitchPosition = value;
            EventChangeSwitchPosition();
        }
        get { return SwitchSettings.SwitchPosition; }
    }
    public int SwitchMax
    {
        set
        {
            SwitchSettings.MaxSwitchPosition = value;
        }
        get { return SwitchSettings.MaxSwitchPosition; }
    }
    public int SwitchMin
    {
        set { SwitchSettings.MinSwitchPosition = value; }
        get { return SwitchSettings.MinSwitchPosition; }
    }
    public float ScopeSwitch
    {
        set
        {
            SwitchPosition = (int)Mathf.Round(value * (float)SwitchMax);
        }
        get { return (float)SwitchPosition / (float)SwitchMax; }
    }
    public int CountPositions { get { return SwitchMax - SwitchMin + 1; } }
    private void SetRotatation(float scope)
    {
        Switcher.SwitcherContenier.localRotation = Switcher.OffsetRotation * Quaternion.AngleAxis(Switcher.DegreeMeasure * scope, Vector3.up);
    }


    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        EventChangeSwitchPosition += delegate { SetRotatation(ScopeSwitch); };

        var rotater = Switcher.RotationScript;
        rotater.Initialize();
        rotater.Local = true;
        SwitchMax = Max;
        SwitchPosition = 0;
    }
    void Awake()
    {
        Initialize();
    }

    public void SwitchToMax()
    {
        SwitchPosition = SwitchMax;
    }
    public void SwitchToMin()
    {
        SwitchPosition = SwitchMin;
    }
    public void Switch()
    {
        if (SwitchPosition == SwitchMin) SwitchToMax(); else SwitchToMin();
    }

    private class SwitchPositionSettings
    {
        private int _pos, _min, _max;
        public int SwitchPosition
        {
            set
            {
                _pos = Mathf.Clamp(value, _min, _max);
            }
            get
            {
                return _pos;
            }
        }

        public int MaxSwitchPosition
        {
            set
            {
                _max = value >= _min ? value : _min;
            }
            get
            {
                return _max;
            }
        }

        public int MinSwitchPosition
        {
            set
            {
                _min = value <= _max ? value : _max;
            }
            get
            {
                return _min;
            }
        }
    }


    [System.Serializable]
    private class SwitcherConfiguration
    {
        [SerializeField] public float DegreeMeasure = 60; // Градусная мера
        [SerializeField] public Quaternion OffsetRotation = Quaternion.identity;
        [SerializeField] public Transform SwitcherContenier;
        [SerializeField] public TargetRotationScript RotationScript;
    }
}
