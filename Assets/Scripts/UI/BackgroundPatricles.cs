using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPatricles : MonoBehaviour
{
    public static BackgroundPatricles Manager { private set; get; }

    [SerializeField]
    private ParticleManager Particles;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Manager != null)
        {
            return;
        }
        if (Particles == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Particles", name);
            enabled = false;
            return;
        }
        Manager = this;
        WorldEther.ChangePalette.Subscribe(Handler);
    }
    private void Handler(Ethers.Channel.Info info)
    {
        UpdateParticlesColors();
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
        UpdateParticlesColors();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangePalette.Unsubscribe(Handler);
    }

    public void SetParticlesColors(params Color[] colors)
    {
        var ps = Particles.ParticleSystem;
        ps.Stop();
        var newGradient = new Gradient();
        var colorKeys = new GradientColorKey[Mathf.Clamp(colors.Length, 0, 8)];

        newGradient.mode = GradientMode.Fixed;

        for (int i = 0; i < colorKeys.Length; ++i)
        {
            colorKeys[i] = new GradientColorKey(colors[i], (float)i / (float)(colorKeys.Length) + (float)1 / (float)colorKeys.Length);
            // Debug.LogFormat(colorKeys[i].time.ToString());
        }
        // newGradient.SetKeys(colorKeys, new GradientAlphaKey[0]);
        newGradient.SetKeys(colorKeys, new GradientAlphaKey[0]);

        var main = ps.main;
        var gradient = new ParticleSystem.MinMaxGradient(newGradient);
        gradient.mode = ParticleSystemGradientMode.RandomColor;
        main.startColor = gradient;

        main.prewarm = true;
        ps.Play();
    }

    public void UpdateParticlesColors()
    {
        var config = Palette.PaletteManager.PaletteConfiguration;
        SetParticlesColors(
            // palette.GetColor(),
            // palette.LoseColor.GetColor(),
            // palette.OtherColor.GetColor(),
            // palette.BlockColor.GetColor()
            config.GetNormalColor(),
            config.GetLoseColor(),
            config.GetUIActionColor(),
            config.GetUIBorderColor()
        );
        // Debug.Log("Hooasd");
    }
}
