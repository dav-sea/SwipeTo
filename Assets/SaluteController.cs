using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaluteController : MonoBehaviour
{
    [SerializeField]
    UIOrganization.Screen LoseScreen;

    [SerializeField]
    ParticleSystem Main;
    [SerializeField]
    ParticleSystem Way;
    [SerializeField]
    ParticleSystem Boom;

    private bool flag;

    public void UpdateColors()
    {
        var main = Main.main;
        main.startColor = Palette.PaletteManager.PaletteConfiguration.GetNormalColor();
        main = Way.main;
        main.startColor = Palette.PaletteManager.PaletteConfiguration.GetLoseColor();
        main = Boom.main;
        main.startColor = Palette.PaletteManager.PaletteConfiguration.GetBlockColor();
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Way == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Way", name);
            enabled = false;
            return;
        }
        if (Main == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Main", name);
            enabled = false;
            return;
        }
        if (Boom == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", " Boom", name);
            enabled = false;
            return;
        }
        if (LoseScreen == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "LoseScreen", name);
            enabled = false;
            return;
        }
        LoseScreen.EventShow += delegate
        {
            if (flag)
            {
                UpdateColors();
                flag = false;
            }
        };
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
        UpdateColors();
        WorldEther.ChangePalette.Subscribe(delegate (Ethers.Channel.Info info)
        {
            if (gameObject.activeInHierarchy)
            {
                UpdateColors();
                flag = false;
            }
            else flag = true;
        });
    }
}
