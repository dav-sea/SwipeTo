using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleCoreAnimation : MonoBehaviour
{

    [SerializeField] private TransfusionScript TransfusionScript;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TransfusionScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TransfusionScript", name);
            enabled = false;
            return;
        }
        TransfusionScript.Loop = TransfusionScript.LoopMode.PingPong;
        TransfusionScript.SpeedTransfusionColor = 2;
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
        UpdateTransfusion();
        WorldEther.ChangePalette.Subscribe(Handler);
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangePalette.Unsubscribe(Handler);
    }
    private void Handler(Ethers.Channel.Info info)
    {
        UpdateTransfusion();
    }
    public void SetTransfusion(Color color1, Color color2)
    {
        TransfusionScript.SetTransfusion(new Color[] { color1, color2, color1, color2, color1, color2, color1, color2 });
        TransfusionScript.ResetStep();
    }
    public void UpdateTransfusion()
    {
        SetTransfusion(Palette.PaletteManager.PaletteConfiguration.GetLoseColor(), Palette.PaletteManager.PaletteConfiguration.GetNormalColor());
    }
}
