using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteChangeEvent : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent OnChange;
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        WorldEther.ChangePalette.Subscribe(
            delegate (Ethers.Channel.Info info) { OnChange.Invoke(); });
    }
    void Awake()
    {
        Initialize();
    }
}
