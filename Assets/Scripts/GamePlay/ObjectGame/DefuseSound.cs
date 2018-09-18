using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class DefuseSound : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float FreeInterval = 0.5f;
    [SerializeField] [Range(0, 1)] float CriticalInterval = 0.2f;
    [SerializeField] DefuseManager Target;

    private SoundController _sound;
    private SoundController Sound { get { if (_sound == null) _sound = AudioContainer.Manager.CriticalDefuseLoop; return _sound; } }
    protected void SetVolume(float value)
    {
        Sound.ForceVolume = value;
    }
    public void UpdateDefuse()
    {
        // Debug.Log("" + Target.ScopeDefuse);
        SetVolume(1 - Mathf.Clamp((Target.ScopeDefuse - CriticalInterval) / (1 - FreeInterval), 0, 1));
    }
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Target == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Target", name);
            enabled = false;
            return;
        }
        Target.EventChangeDefuse += UpdateDefuse;
    }
    void Awake()
    {
        Initialize();
    }

    // [ContextMenu("S")]
    // private void asd()
    // {
    //     Target = GetComponent<DefuseManager>();
    // }
}