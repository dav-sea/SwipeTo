using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyInfoViewer : MonoBehaviour
{
    [SerializeField] Appearance ViewComonent;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (ViewComonent == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ViewComonent", name);
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
        DeferredAction.Manager.AddDeferredActionCycles(delegate { UpdateViewer(); }, 3f);
    }

    public void UpdateViewer()
    {
        ViewComonent.SetAppearance(DailyAds.Manager.IsReady() && !TopBarScreen.TopBar.LockDaily);
    }


}
