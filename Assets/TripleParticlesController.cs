using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleParticlesController : MonoBehaviour
{

    [SerializeField] private ParticleSystem Particles;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Particles == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Particles", name);
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
        WorldEther.ChangePalette.Subscribe(Handler);
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangePalette.Unsubscribe(Handler);
    }
    private void Handler(Ethers.Channel.Info info)
    {
        UpdateColor();
    }

    public void SetColor(Color color)
    {
        var main = Particles.main;
        main.startColor = color;
    }

    public void UpdateColor()
    {
        SetColor(Palette.PaletteManager.PaletteConfiguration.GetLoseColor());
    }
}
