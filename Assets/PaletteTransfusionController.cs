using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteTransfusionController : MonoBehaviour
{
    [SerializeField] TransfusionScript Target;

    [SerializeField] ColorsReference Left;
    [SerializeField] ColorsReference Right;

    // private Color ToColor(ColorReference color)
    // {
    //     switch (color)
    //     {
    //         case ColorReference.Block: return Palette.PaletteManager.PaletteConfiguration.GetBlockColor();
    //         case ColorReference.Side: return Palette.PaletteManager.PaletteConfiguration.GetSideColor();
    //         case ColorReference.Lose: return Palette.PaletteManager.PaletteConfiguration.GetLoseColor();
    //         default: return Palette.PaletteManager.PaletteConfiguration.GetNormalColor();
    //     }
    // }
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
        WorldEther.ChangePalette.Subscribe(Handles);
        Target.Loop = TransfusionScript.LoopMode.PingPong;
    }
    private void SetColors(ColorsReference left, ColorsReference right)
    {
        Target.SetTransfusion(ColorReference.ReferenceToColor(left), ColorReference.ReferenceToColor(right));
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        UpdateColors();
    }
    public void UpdateColors()
    {
        SetColors(Left, Right);

    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangePalette.Unsubscribe(Handles);
    }
    void Handles(Ethers.Channel.Info info)
    {
        UpdateColors();
    }
    void Awake()
    {
        Initialize();
    }

    // private enum ColorReference { Normal, Lose, Side, Block }
}