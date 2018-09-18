using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appearance : MonoBehaviour
{

    #region EVENTS
    public event AppearanceAction EventShow;
    public event AppearanceAction EventHide;
    #endregion
    #region  PRIVATE FIELDS
#if UNITY_EDITOR
    [SerializeField]
#endif
    private bool _isShow;
    [Space(5)]
    private bool _initialized;
    #endregion

    public bool ActivateOnShow = false;

    private GameObject _GameObject;

    public GameObject GameObject { get { if (_GameObject == null) _GameObject = gameObject; return _GameObject; } }

    public bool IsAppearance { set { SetAppearance(value); } get { return IsShow(); } }

    public void SetAppearance(bool value)
    {
        if (value) Show(); else Hide();
    }

    public bool IsShow()
    {
        return _isShow;
    }

#if UNITY_EDITOR
    [ContextMenu("Show")]
#endif
    public void Show()
    {
        if (_isShow) return;
        if (ActivateOnShow) GameObject.SetActive(true);
        _isShow = true;
        if (EventShow != null) EventShow();
    }

#if UNITY_EDITOR
    [ContextMenu("Hide")]
#endif
    public void Hide()
    {
        if (!_isShow) return;
        _isShow = false;
        if (EventHide != null) EventHide();
    }

    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        OnInitialize();
    }

    protected virtual void OnInitialize() { }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Initialize();
    }

    #region STATIC
    public delegate void AppearanceAction();
    #endregion
}
