using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLayerBackgroundController : MonoBehaviour
{
    [SerializeField] private TransfusionScript Transfusion;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Transfusion == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Transfusion", name);
            enabled = false;
            return;
        }
    }
    void Awake()
    {
        Initialize();
    }

    private Color ReAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        float alpha = 0.75f;
        var config = Palette.PaletteManager.PaletteConfiguration;
        Transfusion.SetTransfusion(
            ReAlpha(config.GetFirstBackgroundColor(), alpha),
         ReAlpha(config.GetSecoundBackgroundColor(), alpha),
         ReAlpha(config.GetNormalColor(), alpha),
         ReAlpha(config.GetSideColor(), alpha));
        Transfusion.SpeedTransfusionColor = 0.025f;
        Transfusion.Loop = TransfusionScript.LoopMode.PingPong;
    }
}
