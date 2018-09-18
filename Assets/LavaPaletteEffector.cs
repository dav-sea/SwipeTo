using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPaletteEffector : MonoBehaviour
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
    }
    void Awake()
    {
        Initialize();
    }

    // Use this for initialization
    void Start()
    {
        WorldEther.ChangePalette.Subscribe(Handler);
        UpdateColor();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangePalette.Unsubscribe(Handler);
    }

    public void UpdateColor()
    {
        Color lose = Palette.PaletteManager.PaletteConfiguration.GetLoseColor(), normal = Palette.PaletteManager.PaletteConfiguration.GetNormalColor();
        TransfusionScript.SetTransfusion(lose, normal, lose);
        TransfusionScript.ResetStep();
    }

    private void Handler(Ethers.Channel.Info info)
    {
        UpdateColor();
    }
}
