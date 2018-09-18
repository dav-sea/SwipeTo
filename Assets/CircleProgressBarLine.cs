using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleProgressBarLine : MonoBehaviour
{
    [SerializeField] private ProgressBar Target;
    [SerializeField] private UnityEngine.UI.Image Slider;
    [SerializeField] private ColorsReference Color = ColorsReference.Lose;

    [SerializeField] [Range(0, 1)] private float AlphaModificator = 1;

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
        if (Slider == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Slider", name);
            enabled = false;
            return;
        }
        Target.EventChangeProgress += UpdateVisual;
        Slider.fillMethod = UnityEngine.UI.Image.FillMethod.Radial360;
        Slider.fillAmount = Target.Progress;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        UpdateVisual();
        UpdateColor();
        WorldEther.ChangePalette.Subscribe(delegate (Ethers.Channel.Info inf)
        {
            UpdateColor();
        });
    }

    public void UpdateColor()
    {
        SetColor(ColorReference.ReferenceToColor(Color));
    }

    public void SetColor(Color color)
    {
        Slider.color = new Color(color.r, color.g, color.b, AlphaModificator);
    }

    public void UpdateVisual()
    {
        SetSlider(Target.Progress);
    }

    private void SetSlider(float scale)
    {
        Slider.fillAmount = scale;
    }

    void Awake()
    {
        Initialize();
    }
}
