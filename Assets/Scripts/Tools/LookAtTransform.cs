using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTransform : MonoBehaviour
{

    public Transform TargetLook;

    [SerializeField]
    private bool TargetIsMainCamera;

    private Transform _transform;

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

    // Update is called once per frame
    void Update()
    {
        _transform.LookAt(TargetIsMainCamera ? Camera.main.transform : TargetLook);
    }
}
