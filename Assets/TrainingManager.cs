using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{

    public const string KEY_LEVELVIEWER = "LVTC";
    public const string KEY_DAILY = "DTC";
    public const string KEY_CUSTOMIZE = "CTC";
    public const string KEY_MODES = "MTC";

    private void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
    private bool GetBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 0 ? false : true;
    }

    public bool CompleteLevelViewer
    {
        set { SetBool(KEY_LEVELVIEWER, value); }
        get { return GetBool(KEY_LEVELVIEWER); }
    }
    // public bool CompleteDaily
    // {
    //     set { SetBool(KEY_DAILY, value); }
    //     get { return GetBool(KEY_DAILY); }
    // }
    public bool CompleteCustomize
    {
        set { SetBool(KEY_CUSTOMIZE, value); }
        get { return GetBool(KEY_CUSTOMIZE); }
    }
    public bool CompleteModes
    {
        set { SetBool(KEY_MODES, value); }
        get { return GetBool(KEY_MODES); }
    }

    public static TrainingManager Manager { private set; get; }

    [SerializeField]
    private GameObject TrainingLevelViewer;
    // [SerializeField]
    // private GameObject TrainingDaily;
    [SerializeField]
    private GameObject TrainingCustomize;
    [SerializeField]
    private GameObject TrainingModes;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        if (TrainingLevelViewer == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TrainingLevelViewer", name);
            enabled = false;
            return;
        }
        //Initialize logic
        if (Manager != null) { Destroy(this); return; }
        Manager = this;
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
        if (!CompleteLevelViewer) TrainingLevelViewer.SetActive(true);
        // if (!CompleteDaily) TrainingDaily.SetActive(true);
        // if (!CompleteCustomize) TrainingCustomize.SetActive(true);
        //if (!CompleteModes) TrainingLevelViewer.SetActive(true);
    }
}
