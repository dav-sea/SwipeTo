using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Manager { private set; get; }

    private static float _volume = 0.2f;
    public static float BaseVolume
    {
        set { _volume = value; WorldEther.ChnageVolume.Push(null, null); }
        get { return _volume; }
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Manager != null)
        {
            Destroy(this);
            return;
        }

        Manager = this;
    }
    void Awake()
    {
        Initialize();
    }
}
