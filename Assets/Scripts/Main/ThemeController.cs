using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ThemeController : MonoBehaviour
{
    public static ThemeController Manager { private set; get; }
    private bool _initialized;

    protected Theme Current;

    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Manager != null) { Destroy(this); return; }
        Manager = this;
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
        SelectTheme(ItemsBase.Base.Themes.GetFirstSelectedToPrefab());
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public void SelectTheme(GameObject themePrefab)
    {
        SelectTheme(Instantiate(themePrefab).GetComponent<Theme>());
    }
    public void SelectTheme(Theme theme)
    {
        if (theme == null) return;
        if (Current != null) Destroy(Current.gameObject);

        Current = theme;
        Palette.PaletteManager.SetColors(theme.Palette);
        BackgroundManager.Manager.SetBackground(theme.PrefabBackground);
    }
}