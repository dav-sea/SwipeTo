using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockButton : LockTouch
{
    [SerializeField] Leaf[] Leafs;

    [SerializeField] Appearance LockText;

    [SerializeField] private bool StartLock;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        EventChangeLock += delegate (bool value) { UpdateColor(); if (value) LockText.Show(); else LockText.Hide(); };
        Lock = StartLock;
    }

    public void UpdateColor()
    {
        SetColor(Lock);
    }

    public void Subscribe()
    {
        WorldEther.ChangePalette.Subscribe(delegate (Ethers.Channel.Info info)
        {
            UpdateColor();
        });
    }

    protected void SetColor(bool Lock = false)
    {
        // Debug.Log("is lock: " + Lock);
        if (!Lock)
            foreach (Leaf leaf in Leafs)
                leaf.SetColorRGB(GetBaseColor(leaf.color));
        else
            foreach (Leaf leaf in Leafs)
                leaf.SetColorRGB(GetLockColor());
    }
    protected Color GetBaseColor(BaseColor color)
    {
        switch (color)
        {
            case BaseColor.Action: return Palette.PaletteManager.PaletteConfiguration.GetUIActionColor();
            case BaseColor.Block: return Palette.PaletteManager.PaletteConfiguration.GetBlockColor();
            case BaseColor.Normal: return Palette.PaletteManager.PaletteConfiguration.GetNormalColor();
            case BaseColor.Side: return Palette.PaletteManager.PaletteConfiguration.GetSideColor();
            case BaseColor.Lose: return Palette.PaletteManager.PaletteConfiguration.GetLoseColor();
            default: return Palette.PaletteManager.PaletteConfiguration.GetUIBorderColor();
        }
    }

    protected Color GetLockColor()
    {
        return Palette.PaletteManager.PaletteConfiguration.GetUILockColor();
    }

    [System.Serializable]
    protected class Leaf
    {
        public Renderer renderer;
        public BaseColor color;

        public void SetColorRGB(Color color)
        {
            renderer.material.color = new Color(color.r, color.g, color.b, renderer.material.color.a);
        }
    }

    protected enum BaseColor { Border, Action, Block, Normal, Side, Lose }
}
