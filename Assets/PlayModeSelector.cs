using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeSelector : MonoBehaviour
{
    public const string KEY_SELECT_CORE = "SGC";
    [SerializeField] ModeSelector Selector;

    [SerializeField] GamePlaySingleCore SingleCore; // id - 10;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Selector == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Selector", name);
            enabled = false;
            return;
        }
        if (SingleCore == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "SingleCore", name);
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
        Load();
    }

    public void Select()
    {
        Selector.SelectMode();
    }

    public void SetMode(GamePlayCore corePrefab)
    {
        Selector.CorePrefab = corePrefab;
    }

    private int SelectToID()
    {
        return 10;//Single Core
    }

    private GamePlayCore IDToCore(int id)
    {
        return SingleCore; // id - 10
    }

    public void Save()
    {
        PlayerPrefs.SetInt(KEY_SELECT_CORE, SelectToID());
    }

    public void Load()
    {
        SetMode(IDToCore(PlayerPrefs.GetInt(KEY_SELECT_CORE, 0)));
    }
}
