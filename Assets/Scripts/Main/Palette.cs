using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    public static Palette PaletteManager { private set; get; }

    [SerializeField] private PaletteClaster _PaletteConfiguration;

    public PaletteClaster PaletteConfiguration { get { return _PaletteConfiguration; } }

    public void SetColors(PaletteClaster claster)
    {
        if (claster == null) return;
        _PaletteConfiguration = claster;
        WorldEther.ChangePalette.Push(null, null);
    }

    private bool _initialized;

    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Lol
        //Initialize logic
        if (PaletteManager != null)
        {
            enabled = false;
            return;
        }
        PaletteManager = this;
    }
    void Awake()
    {
        Initialize();
    }

    [ContextMenu("Call Update")]

    private void EditorUpdate()
    {
        WorldEther.ChangePalette.Push(null, null);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // WorldEther.ChangePalette.Push(null, null);
    }

    [System.Serializable]
    public class PaletteClaster
    {
        [SerializeField] protected Color NormalColor;
        [SerializeField] protected Color LoseColor;
        [SerializeField] protected Color BlockColor;
        [SerializeField] protected Color SideColor;

        [SerializeField] protected Color BackgroundColor;


        [SerializeField] protected Color UIBorderColor;
        [SerializeField] protected Color UIActionColor;
        [SerializeField] protected Color UIScoreColor;
        [SerializeField] protected Color UIMultiplierColor;

        [SerializeField] protected Color UITextColor = Color.gray;
        [SerializeField] protected Color UITextOutline = Color.black;

        [SerializeField] protected Color CoinColor;

        [SerializeField] protected Color UILockColor = Color.gray;

        [SerializeField] protected Color FirstBackgroundColor;
        [SerializeField] protected Color ScoundBackgroundColor;

        public Color GetUILockColor()
        {
            return UILockColor;
        }
        public Color GetFirstBackgroundColor()
        {
            return FirstBackgroundColor;
        }
        public Color GetSecoundBackgroundColor()
        {
            return ScoundBackgroundColor;
        }

        public Color GetSideColor()
        {
            return SideColor;
        }
        public Color GetUITextColor()
        {
            return UITextColor;
        }

        public Color GetUITextOutline()
        {
            return UITextOutline;
        }
        public Color GetUIMultiplierColor()
        {
            return UIMultiplierColor;
        }
        public Color GetUIScoreColor()
        {
            return UIScoreColor;
        }
        public Color GetNormalColor()
        {
            return NormalColor;
        }
        public Color GetLoseColor()
        {
            return LoseColor;
        }
        public Color GetBlockColor()
        {
            return BlockColor;
        }
        public Color GetBackgroundColor()
        {
            return BackgroundColor;
        }
        public Color GetUIBorderColor()
        {
            return UIBorderColor;
        }
        public Color GetUIActionColor()
        {
            return UIActionColor;
        }
        public Color GetCoinColor()
        {
            return CoinColor;
        }
    }

}

public enum ColorsReference { Normal, Lose, Side, Block, UIText, UIBorder, UIOutline, UIScore }

public static class ColorReference
{
    public static Color ReferenceToColor(ColorsReference color)
    {
        switch (color)
        {
            case ColorsReference.Block: return Palette.PaletteManager.PaletteConfiguration.GetBlockColor();
            case ColorsReference.Side: return Palette.PaletteManager.PaletteConfiguration.GetSideColor();
            case ColorsReference.Lose: return Palette.PaletteManager.PaletteConfiguration.GetLoseColor();
            case ColorsReference.UIText: return Palette.PaletteManager.PaletteConfiguration.GetUITextColor();
            case ColorsReference.UIBorder: return Palette.PaletteManager.PaletteConfiguration.GetUIBorderColor();
            case ColorsReference.UIOutline: return Palette.PaletteManager.PaletteConfiguration.GetUITextOutline();
            case ColorsReference.UIScore: return Palette.PaletteManager.PaletteConfiguration.GetUIScoreColor();
            default: return Palette.PaletteManager.PaletteConfiguration.GetNormalColor();
        }
    }
    public static Color ReferenceToColor(ColorsReference color, float alpha)
    {
        var clr = ReferenceToColor(color);
        return new Color(clr.r, clr.g, clr.b, alpha);
    }
}