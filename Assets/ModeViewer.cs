using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DELETE ?
public class ModeViewer : MonoBehaviour
{
    public const string KEY_ACTIVEMODE = "mode";// Не трогать ключ, блять!!
    [SerializeField] Leaf Single;
    [SerializeField] Leaf Time;
    [SerializeField] Leaf Dual;
    [SerializeField] ModeSelector TargetModeSelector;
    [SerializeField] TextMesh BestLabel;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TargetModeSelector == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TargetModeSelector", name);
            enabled = false;
            return;
        }
        if (Single.CheckNull())
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Single", name);
            enabled = false;
            return;
        }
        if (Time.CheckNull())
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Time", name);
            enabled = false;
            return;
        }
        if (BestLabel == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "BestLabel", name);
            enabled = false;
            return;
        }
        // if (Dual.CheckNull())
        // {
        //     Debug.LogWarningFormat("{0} (in {1}) is null", "Dual", name);
        //     enabled = false;
        //     return;
        // }
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
        LoadActive();
    }

    public void SaveActive()
    {
        PlayerPrefs.SetInt(KEY_ACTIVEMODE, ModeToInt(Current));
    }

    public void LoadActive()
    {
        SetActiveMode(PlayerPrefs.GetInt(KEY_ACTIVEMODE, 0));
    }

    private Modes Current;
    public void SetActiveMode(Modes mode)
    {
        ModeToLeaf(Current).GameObject.SetActive(false); ;

        var leafNewMode = ModeToLeaf(mode);
        TargetModeSelector.CorePrefab = leafNewMode.PrefabCore;
        leafNewMode.GameObject.SetActive(true);
        if (mode == Modes.Dual)
            BestLabel.text = "";
        else if (leafNewMode.PrefabCore != null)
            BestLabel.text = leafNewMode.PrefabCore.BestResult().ToString();
        Current = mode;
        SaveActive();
    }

    public void UpdateBestScores()
    {
        if (Current == Modes.Dual)
            BestLabel.text = "";
        else if (ModeToLeaf(Current).PrefabCore != null)
            BestLabel.text = ModeToLeaf(Current).PrefabCore.BestResult().ToString();
    }

    public void SetActiveMode(int mode)
    {
        SetActiveMode(IntToMode(mode));
    }

    public Modes IntToMode(int mode)
    {
        return
                  mode == 1 ? Modes.Time
                : mode == 2 ? Modes.Dual
                : Modes.Single;
    }

    public int ModeToInt(Modes mode)
    {
        return
          mode == Modes.Single ? 0
        : mode == Modes.Time ? 1
        : mode == Modes.Dual ? 2
        : -1;
    }

    private Leaf ModeToLeaf(Modes mode)
    {
        return
          mode == Modes.Single ? Single
        : mode == Modes.Time ? Time
        : mode == Modes.Dual ? Dual
        : null;
    }

    public enum Modes { Single, Time, Dual }

    [System.Serializable]
    private class Leaf
    {
        public GameObject GameObject;
        public GamePlayCore PrefabCore;

        public bool CheckNull()
        {
            return GameObject == null || PrefabCore == null;
        }
    }
}
