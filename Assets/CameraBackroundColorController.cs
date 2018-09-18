using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackroundColorController : MonoBehaviour
{
    [SerializeField]
    Camera Target;

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
        UpdateColor();
        WorldEther.ChangePalette.Subscribe(delegate (Ethers.Channel.Info info) { UpdateColor(); });
    }

    public void UpdateColor()
    {
        Target.backgroundColor = Palette.PaletteManager.PaletteConfiguration.GetBackgroundColor();
    }
}
