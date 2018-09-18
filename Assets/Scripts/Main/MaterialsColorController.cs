using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class MaterialsColorController : MonoBehaviour
{
    [SerializeField] private MaterialsContenier Contenier;
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Contenier == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Contenier", name);
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
        Contenier.Update();
        WorldEther.ChangePalette.Subscribe(Handler);
    }

    private void Handler(Ethers.Channel.Info info)
    {
        Contenier.Update();
    }

    [System.Serializable]
    public class MaterialsContenier
    {
        [SerializeField] private Material NormalMaterial;
        [SerializeField] private Material LoseMaterial;
        [SerializeField] private Material SideMaterial;
        [SerializeField] private Material BlockMaterial;
        [SerializeField] private Material UIBorderMaterial;
        [SerializeField] private Material UIActionMaterial;
        // [SerializeField] private Material UITextMaterial;
        [SerializeField] private Material CoinMaterial;

        public void Update()
        {
            var config = Palette.PaletteManager.PaletteConfiguration;

            NormalMaterial.color = config.GetNormalColor();
            LoseMaterial.color = config.GetLoseColor();
            BlockMaterial.color = config.GetBlockColor();
            UIBorderMaterial.color = config.GetUIBorderColor();
            UIActionMaterial.color = config.GetUIActionColor();
            CoinMaterial.color = config.GetCoinColor();
            SideMaterial.color = config.GetSideColor();
            // UITextMaterial.color = config.GetUITextColor();
        }
    }
}