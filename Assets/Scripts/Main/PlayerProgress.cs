using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Manager { private set; get; }

    public const string KEY_PROGRESS = "PROGRESS";
    private int _progressScore;

    public int ProgressScore
    {
        set { _progressScore = value; WorldEther.ProgressScoreChagne.Push(null, null); Save(); }
        get { return _progressScore; }
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        if (Manager != null)
        {
            Destroy(this);
            return;
        }
        Manager = this;
        //Initialize logic
        Load();
    }

#if UNITY_EDITOR
    [ContextMenu("Flash To Zero")]
    private void Editor_FlashProgress()
    {
        ProgressScore = 0;
    }
    [ContextMenu("Add 250")]
    private void Editor_add250()
    {
        ProgressScore += 250;
    }
#endif

    public void Save()
    {
        PlayerPrefs.SetInt(KEY_PROGRESS, _progressScore);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        _progressScore = PlayerPrefs.GetInt(KEY_PROGRESS, 0);
    }


    void Awake()
    {
        Initialize();
    }
}
