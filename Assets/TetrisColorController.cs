using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisColorController : MonoBehaviour
{

    [SerializeField] private TransfusionScript[] Elements;
    [SerializeField] private int CountColorsOnElement = 3;
    [SerializeField] private float Speed = 0.5f;
    [Range(0, 1)] [SerializeField] private float SpeedStep = 1;

    private Color[] PaletteColors;

    public void SetupColors()
    {
        for (int i = 0; i < Elements.Length; ++i)
            RandomizeElement(Elements[i]);
    }

    private void SetTransfusion(TransfusionScript transfusion, float speed, params Color[] colors)
    {
        transfusion.SetTransfusion(colors);
        transfusion.SpeedTransfusionColor = speed;
        transfusion.ResetAndStart();
    }
    private void SetTransfusion(int index, float speed, params Color[] colors)
    {
        if (index < 0 || index >= Elements.Length) return;
        SetTransfusion(Elements[index], speed, colors);
    }

    private void RandomizeElement(TransfusionScript transfusion)
    {
        SetTransfusion(transfusion, GetRandomSpeed(), GetRandomColors(CountColorsOnElement));
    }

    private Color[] GetRandomColors(int count)
    {
        var result = new Color[count];
        for (int i = count - 1; i >= 0; --i)
            result[i] = GetRandomColor();
        return result;
    }

    private float GetRandomSpeed()
    {
        return Speed + (Speed * SpeedStep * Random.value) * (Random.Range(0, 10) % 2 == 0 ? 1 : -1);
    }


    private Color GetRandomColor()
    {
        return PaletteColors[Random.Range(0, PaletteColors.Length)];
    }

    public void SetPalette(params Color[] colors)
    {
        PaletteColors = colors;
        SetupColors();
    }

    public void UpdatePalette()
    {
        var config = Palette.PaletteManager.PaletteConfiguration;
        SetPalette(config.GetBlockColor(), config.GetLoseColor(), config.GetNormalColor(),
        config.GetSideColor(), config.GetBackgroundColor(), config.GetFirstBackgroundColor(),
        config.GetSecoundBackgroundColor());
    }

    private void HandlerChangePalette(Ethers.Channel.Info info)
    {
        if (gameObject.activeSelf)
            UpdatePalette();
        else _updColors = true;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if (_updColors) UpdatePalette();
        _updColors = false;
    }

    private bool _initialized, _updColors;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic

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
        UpdatePalette();
        WorldEther.ChangePalette.Subscribe(HandlerChangePalette);
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangePalette.Unsubscribe(HandlerChangePalette);
    }
}
