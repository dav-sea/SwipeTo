using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManualStand : ManualKit
{
    #region OUT SCRIPTS
    //====================================================================================//
    [SerializeField]
    protected TransfusionScript TransfusionScript;
    //====================================================================================//
    #endregion

    private Appearance AppeatanceObject;

    public abstract GameObject GetPrefabObject();

    protected virtual Appearance CreateAppearanceObject()
    {
        return Instantiate(GetPrefabObject(), transform).GetComponent<Appearance>();
    }

    public override bool IsRotater()
    {
        return false;
    }

    public override bool IsSwipeble()
    {
        return !WasUsed;
    }

    protected bool WasUsed;
    protected override bool OnSwipe(Side.Diraction diraction)
    {
        if (!WasUsed && Diraction == diraction)
        {
            KitManual();
            SoundEffect();
            AppeatanceObject.Hide();
            TransfusionScript.enabled = true;
            WasUsed = true;
            return true;
        }
        return false;
    }

    public void UpdateColors()
    {
        TransfusionScript.SetTransfusion(Palette.PaletteManager.PaletteConfiguration.GetNormalColor(), Palette.PaletteManager.PaletteConfiguration.GetLoseColor());
    }
    public void SoundEffect()
    {
        AudioContainer.Manager.DefaultStandSound.Play();
    }
    public override void ResetAction()
    {
        base.ResetAction();
        if (AppeatanceObject == null) AppeatanceObject = CreateAppearanceObject();
        TransfusionScript.enabled = false;
        TransfusionScript.ResetStep();
        WasUsed = false;
        AppeatanceObject.Show();
    }
    public override bool WillRotate(Side.Diraction diraction)
    {
        return diraction == Diraction && !WasUsed && IsActiveAction;
    }
    protected override bool OnInitialize()
    {
        if (!base.OnInitialize()) return false;
        if (TransfusionScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TransfusionScript", name);
            enabled = false;
            return false;
        }
        AppeatanceObject = CreateAppearanceObject();
        UpdateColors();
        WorldEther.ChangePalette.Subscribe(HandlerChangePalette);
        return true;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangePalette.Unsubscribe(HandlerChangePalette);
    }

    private void HandlerChangePalette(Ethers.Channel.Info info)
    {
        UpdateColors();
    }
}