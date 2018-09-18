using UnityEngine;

public class DefuseColor : MonoBehaviour
{
    // [SerializeField]
    // private DefuseManager Defuse;

    // public ColorEvent ChangeColor;

    [SerializeField]
    private DefuseManager DefuseManager;

    [SerializeField]
    private MeshRenderer TargetRenderer;

    private Material Material;

    public Gradient GradientColor;

    [Range(0, 1)]
    public float FreeInterval = 0.1f;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TargetRenderer == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TargetMaterial", name);
            enabled = false;
            return;
        }
        if (DefuseManager == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefuseManager", name);
            enabled = false;
            return;
        }
        Material = TargetRenderer.material;
        DefuseManager.EventChangeDefuse += UpdateDefuse;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        UpdateDefuseColors();
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
        // Debug.Log("here");
        UpdateDefuseColors();
    }

    public void UpdateDefuseColors()
    {
        SetDefuseColors(Palette.PaletteManager.PaletteConfiguration.GetBlockColor(), Palette.PaletteManager.PaletteConfiguration.GetLoseColor());
        SetDefuseColor(DefuseManager.ScopeDefuse);
    }

    public void SetDefuseColors(Color Start, Color Finish)
    {
        var newGradient = new Gradient();
        var colorKeys = new GradientColorKey[2];

        colorKeys[0] = new GradientColorKey(Finish, 0);
        colorKeys[1] = new GradientColorKey(Start, 1 - FreeInterval);

        newGradient.SetKeys(colorKeys, new GradientAlphaKey[0]);
        newGradient.mode = GradientMode.Blend;

        GradientColor = newGradient;
    }

    public void SetDefuseManager(DefuseManager defuse)
    {
        DefuseManager = defuse;
    }

    public void SetDefuseColor(float scope)
    {
        // _value = value / max; //ScopeValue
        if (Material != null)
            Material.color = GradientColor.Evaluate(scope);
    }

    public void UpdateDefuse()
    {
        if (DefuseManager != null)
            SetDefuseColor(DefuseManager.ScopeDefuse);

    }

    void Awake()
    {
        Initialize();
    }

    // public class ColorEvent : UnityEngine.Events.UnityEvent<Color> { }
}