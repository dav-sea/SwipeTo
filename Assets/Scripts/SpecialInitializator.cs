using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class SpecialInitializator : MonoBehaviour
{
    [SerializeField] ItemsThemeViewer ThemeViewer;
    [SerializeField] ItemsObjectGamesViewer ObjectGamesViewer;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (ThemeViewer == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ThemeViewer", name);
            enabled = false;
            return;
        }
        if (ObjectGamesViewer == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ObjectGamesViewer", name);
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
        ObjectGamesViewer.InitializeViewer();
        ThemeViewer.InitializeViewer();
    }
}