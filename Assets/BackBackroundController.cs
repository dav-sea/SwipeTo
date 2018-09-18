using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBackroundController : MonoBehaviour
{
    [SerializeField] SpriteRenderer LeftGrand;
    [SerializeField] SpriteRenderer RightGrand;

    [SerializeField] TransfusionScript LeftTransfusion;
    [SerializeField] TransfusionScript RightTransfusion;

    Color BaseLeftColor;
    Color BaseRightColor;

    Material LeftMaterial;
    Material RightMaterial;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (LeftGrand == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "LeftGrand", name);
            enabled = false;
            return;
        }
        if (RightGrand == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", " RightGrand", name);
            enabled = false;
            return;
        }
        LeftMaterial = LeftGrand.material;
        RightMaterial = RightGrand.material;
        if (LeftTransfusion == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "LeftTransfusion", name);
            enabled = false;
            return;
        }
        if (RightTransfusion == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "RightTransfusion", name);
            enabled = false;
            return;
        }
        LeftTransfusion.EventFinish += FinishTransfusion;
        RightTransfusion.EventFinish += FinishTransfusion;
    }

    public void SetSize(Vector2 size)
    {
        LeftGrand.size = size;
        RightGrand.size = size;
    }

    public void UpdateBackgroundSize()
    {
        SetSize(new Vector2(1280 * (Screen.width / (float)Screen.height), 1280));
    }

    public void SetBase(Color left, Color right)
    {
        BaseLeftColor = left;
        BaseRightColor = right;
    }

    public void SetColors(Color left, Color right)
    {
        LeftMaterial.color = left;
        RightMaterial.color = right;
    }

    public void SetLeftTarget(Color secound, Color last, float time)
    {
        LeftTransfusion.SetTransfusion(GetLeftCurrent(), secound, last);
        if (time == 0) time = 1;
        LeftTransfusion.SpeedTransfusionColor = 1 / time;
        LeftTransfusion.ResetAndStart();
    }
    public void SetRightTarget(Color secound, Color last, float time)
    {
        RightTransfusion.SetTransfusion(GetRightCurrent(), secound, last);
        if (time == 0) time = 1;
        RightTransfusion.SpeedTransfusionColor = 1 / time;
        RightTransfusion.ResetAndStart();
    }

    public Color GetLeftCurrent()
    {
        return LeftMaterial.color;
    }

    public Color GetRightCurrent()
    {
        return RightMaterial.color;
    }

    public void UpdateBases()
    {
        SetBase(Palette.PaletteManager.PaletteConfiguration.GetFirstBackgroundColor(),
            Palette.PaletteManager.PaletteConfiguration.GetSecoundBackgroundColor());
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
        UpdateBases();
        SetColors(BaseLeftColor, BaseRightColor);
        // Debug.Log("here" + BaseLeftColor);
        // WorldEther.LoseGame.Subscribe(HandlerLoseGame);
        // WorldEther.CoinsChange.Subscribe(HandlerCoins);
        WorldEther.EventSwipe.Subscribe(HandlerSwipe);
        UpdateBackgroundSize();
        SwitchBases(2);
        // WorldEther.ChangePalette.Subscribe(HandlerChangePaletteColor);
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        //  WorldEther.ChangePalette.Unsubscribe(HandlerChangePaletteColor);
        // WorldEther.LoseGame.Unsubscribe(HandlerLoseGame);
        // WorldEther.CoinsChange.Unsubscribe(HandlerCoins);
        WorldEther.EventSwipe.Unsubscribe(HandlerSwipe);
        if (SwitchBasesDeferred != null) SwitchBasesDeferred.Cancel();
    }

    // private void HandlerLoseGame(Ethers.Channel.Info info)
    // {
    //     SetLeftTarget(Palette.PaletteManager.PaletteConfiguration.GetBackgroundColor(), BaseLeftColor, 0.5f);
    //     SetRightTarget(Palette.PaletteManager.PaletteConfiguration.GetBackgroundColor(), BaseRightColor, 0.5f);
    // }

    // private void HandlerCoins(Ethers.Channel.Info info)
    // {
    //     SetLeftTarget(Palette.PaletteManager.PaletteConfiguration.GetCoinColor(), BaseLeftColor, 0.4f);
    //     SetRightTarget(Palette.PaletteManager.PaletteConfiguration.GetCoinColor(), BaseRightColor, 0.4f);
    // }

    private void HandlerSwipe(Ethers.Channel.Info info)
    {
        SwitchBases(2);
    }

    DeferredAction.OnceAction SwitchBasesDeferred;

    private void FinishTransfusion()
    {
        if (LeftTransfusion.enabled || RightTransfusion.enabled) return;
        // SwitchBases(10);
        if (SwitchBasesDeferred != null) SwitchBasesDeferred.Cancel();
        SwitchBasesDeferred = new DeferredAction.OnceAction(delegate { SwitchBases(10); }, 1);
        DeferredAction.Manager.AddDeferredAction(SwitchBasesDeferred);
    }

    public void SwitchBases(float time)
    {
        if (SwitchBasesDeferred != null) SwitchBasesDeferred.Cancel();
        var buffer = BaseLeftColor;
        BaseLeftColor = BaseRightColor;
        BaseRightColor = buffer;
        if (time == 0) time = 1;

        // SetLeftTarget(GetLeftCurrent(), BaseLeftColor, 5);
        // SetRightTarget(GetRightCurrent(), BaseRightColor, 5);
        LeftTransfusion.SetTransfusion(GetLeftCurrent(), BaseRightColor);
        LeftTransfusion.SpeedTransfusionColor = 1 / time;
        LeftTransfusion.ResetAndStart();

        RightTransfusion.SetTransfusion(GetRightCurrent(), BaseLeftColor);
        RightTransfusion.SpeedTransfusionColor = 1 / time;
        RightTransfusion.ResetAndStart();
    }

    // void HandlerChangePaletteColor(Ethers.Channel.Info info)
    // {
    //     UpdateBases();
    //     SetColors(BaseLeftColor, BaseRightColor);
    // }
}
