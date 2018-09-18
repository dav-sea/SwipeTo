using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Screen = UIOrganization.Screen;

public class CoinsViewerScreenController : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text CoinsCountText;

    [SerializeField]
    ParticleSystem Particles;
    [SerializeField]
    Screen Daily;
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        if (CoinsCountText == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "CoinsCountText", name);
            enabled = false;
            return;
        }
        if (Particles == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Particles", name);
            enabled = false;
            return;
        }
        UpdateCoins();
        UpdateColors();
        WorldEther.ChangePalette.Subscribe(HandlerChangePalette);
        WorldEther.CoinsChange.Subscribe(HandlerChangeCoins);
    }
    void Start()
    {
        Initialize();
    }

    public void TryShowDaily()
    {
        if (!TopBarScreen.TopBar.LockDaily)
            UIOrganization.UIController.Controller.ShowOnce(Daily);
    }

    public void UpdateCoins()
    {
        CoinsCountText.text = Coins.Manager.CoinsCount.ToString();
    }

    public void UpdateColors()
    {
        CoinsCountText.color = Palette.PaletteManager.PaletteConfiguration.GetCoinColor();
        var main = Particles.main; main.startColor = new Color(CoinsCountText.color.r, CoinsCountText.color.g, CoinsCountText.color.b, 0.4f);
    }

    private void HandlerChangePalette(Ethers.Channel.Info info)
    {
        UpdateColors();
    }
    private void HandlerChangeCoins(Ethers.Channel.Info info)
    {
        UpdateCoins();
    }
}
