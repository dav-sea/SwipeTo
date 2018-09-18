using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockText : MonoBehaviour
{
    private const string TRANSLATION_NAME = "UI_OpenNumber";
    [SerializeField] UnityEngine.UI.Text Text;
    [SerializeField] [Range(1, 10)] int TargetLevel;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Text == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Text", name);
            enabled = false;
            return;
        }
        Text.supportRichText = true;
        Text.resizeTextForBestFit = true;

    }
    public void UpdateText()
    {
        Text.text = string.Format(TranslationManager.GetText(TRANSLATION_NAME), TargetLevel);
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        UpdateText();
    }
    void Awake()
    {
        Initialize();
    }
}
