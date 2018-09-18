using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
// using UIOrganization;

public class GameSettingsViewer : MonoBehaviour
{
    [SerializeField] private UIOrganization.Screen Target;
    [SerializeField] private SwitcherObject EffectsSwitcher;
    [SerializeField] private SwitcherObject AntiAlisSwitcher;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (EffectsSwitcher == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "EffectsSwitcher", name);
            enabled = false;
            return;
        }
        if (AntiAlisSwitcher == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "AntiAlisSwitcher", name);
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
        EffectsSwitcher.SwitchPosition = GameSettings.QualitySettingsToInt(GameSettings.Manager.GetEffects()) + 1;
        EffectsSwitcher.EventChangeSwitchPosition += HandlerEffectsSwitch;

        AntiAlisSwitcher.SwitchPosition = GameSettings.QualitySettingsToInt(GameSettings.Manager.GetAntialis()) + 1;
        AntiAlisSwitcher.EventChangeSwitchPosition += HandlerAntialisSwitch;
    }

    private void HandlerEffectsSwitch()
    {
        GameSettings.Manager.SetEffects(GameSettings.IntToQualitySettings(EffectsSwitcher.SwitchPosition - 1));
    }

    private void HandlerAntialisSwitch()
    {
        GameSettings.Manager.SetAntialis(GameSettings.IntToQualitySettings(AntiAlisSwitcher.SwitchPosition - 1));
    }
}