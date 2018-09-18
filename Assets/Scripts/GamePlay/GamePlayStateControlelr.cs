using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayStateControlelr : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text Scores;

    [SerializeField]
    UnityEngine.UI.Text Multiplier;

    [Space(3)]
    [SerializeField]
    AnimationStateManager PauseButton;
    [SerializeField]
    AnimationClip PauseButtonShow;
    [SerializeField]
    AnimationClip PauseButtonHide;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Scores == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Scores", name);
            enabled = false;
            return;
        }
        if (Multiplier == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Multiplier", name);
            enabled = false;
            return;
        }
        UpdateTextColor();
    }
    void Awake()
    {
        Initialize();
    }

    public void SetTextColor(Color scores, Color multiplier)
    {
        Scores.color = scores;
        Multiplier.color = multiplier;
    }

    public void UpdateTextColor()
    {
        SetTextColor(Palette.PaletteManager.PaletteConfiguration.GetUIScoreColor(), Palette.PaletteManager.PaletteConfiguration.GetUIMultiplierColor());
    }

    public void SetActivePauseButton(bool value)
    {
        if (value) PauseButton.SetAndPlay(PauseButtonShow);
        else PauseButton.SetAndPlay(PauseButtonHide);
    }
}
