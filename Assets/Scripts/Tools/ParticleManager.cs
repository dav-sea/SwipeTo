using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleManager : MonoBehaviour
{

    private Transform _transform;
    private ParticleSystem _ParticleSystem;
    public ParticleSystem ParticleSystem
    {
        private set { _ParticleSystem = value; }
        get
        {
            if (_ParticleSystem == null) _ParticleSystem = GetComponent<ParticleSystem>();
            return _ParticleSystem;
        }
    }

    // private Follow Follow
    // {
    //     set { _Follow = value; }
    //     get
    //     {
    //         if (_Follow == null) _Follow = GetComponent<Follow>();
    //         return _Follow;
    //     }
    // }
    // public void SetTargetPosition(Vector3 pos)
    // {
    //     Follow.PointTarget = pos;
    // }
    public ParticleSystem.EmissionModule Emission
    {
        get { return ParticleSystem.emission; }
    }
    public ParticleSystem.MainModule Main
    {
        get { return ParticleSystem.main; }
    }

    public bool EmissonActive
    {
        set
        {
            var e = Emission;
            e.enabled = value;
        }
        get
        {
            return Emission.enabled;
        }
    }

    public ParticleSystem.MinMaxGradient StartColor
    {
        set
        {
            var e = Main;
            e.startColor = value;
        }
        get
        {
            return Main.startColor;
        }
    }

    public float Duration
    {
        set
        {
            var e = Main;
            e.duration = value;
        }
        get { return Main.duration; }
    }

    public Vector3 Position
    {
        set { _transform.position = value; }
        get { return _transform.position; }
    }

    public Quaternion Rotation
    {
        set { _transform.rotation = value; }
        get { return _transform.rotation; }
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        _transform = transform;

    }

    public Transform Transform
    {
        get { return _transform; }
    }
    void Awake()
    {
        Initialize();
    }
}
