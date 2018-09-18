using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPaletteColorController : MonoBehaviour
{
    [SerializeField] TextMesh Target;
    [SerializeField] UnityEngine.UI.Text CanvasTarget;
    [SerializeField] ColorsReference TextColor = ColorsReference.UIText;


    [Space(5)]
    [SerializeField]
    UnityEngine.UI.Outline Outline;
    [SerializeField] ColorsReference EffectsColor = ColorsReference.UIOutline;
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Target == null && CanvasTarget == null && Outline == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TARGETS", name);
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
        UpdateColor();
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangePalette.Unsubscribe(Handler);
    }
    public void SetTextColor(Color color)
    {
        if (Target != null)
            Target.color = color;
        if (CanvasTarget != null)
            CanvasTarget.color = color;
    }
    public void SetEffectsColor(Color color)
    {
        if (Outline != null)
            Outline.effectColor = color;
    }
    private Color CreateToBaseColor(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    public void UpdateColor()
    {
        SetTextColor(ColorReference.ReferenceToColor(TextColor));
        SetEffectsColor(ColorReference.ReferenceToColor(EffectsColor));
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if (flagUpdateColor)
        {
            UpdateColor();
            flagUpdateColor = false;
        }
    }
    private bool flagUpdateColor;
    private void Handler(Ethers.Channel.Info info)
    {
        if (isActiveAndEnabled) UpdateColor(); else flagUpdateColor = true;
    }


#if UNITY_EDITOR
    [ContextMenu("FindMesh")]
    private bool Editor_FindMesh()
    {
        Target = GetComponent<TextMesh>();
        return Target != null;
    }
    [ContextMenu("FindUIText")]
    private bool Editor_FindUIText()
    {
        CanvasTarget = GetComponent<UnityEngine.UI.Text>();
        return CanvasTarget != null;
    }
    [ContextMenu("FindOutline")]
    private bool Editor_FindOutline()
    {
        Outline = GetComponent<UnityEngine.UI.Outline>();
        return Outline != null;
    }
    [ContextMenu("FindAll")]
    private void Editor_FindAll()
    {
        if (!Editor_FindMesh())
            Editor_FindUIComponents();
    }
    [ContextMenu("FindUIComponents")]
    private void Editor_FindUIComponents()
    {
        if (Editor_FindUIText()) Editor_FindOutline();
    }

    // [ContextMenu("213")]
    // private void asd()
    // {
    //     var all = FindObjectsOfType<TextPaletteColorController>();
    //     foreach (var e in all)
    //         e.EffectsColor = ColorsReference.UIOutline;
    // }
#endif
}
