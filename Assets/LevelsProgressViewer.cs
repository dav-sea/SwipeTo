using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Screen = UIOrganization.Screen;

public class LevelsProgressViewer : MonoBehaviour
{
    [SerializeField] private Screen Target;
    [SerializeField] private UnityEngine.UI.Text Text;
    [Space(5)]
    [Header("Appearence Environment")]
    [SerializeField]
    Appearance MaxLevel;
    [SerializeField]
    Appearance LevelProgress;


    protected bool IsMaxLevel
    {
        set
        {
            MaxLevel.IsAppearance = value;
            LevelProgress.IsAppearance = !value;
        }

        get { return MaxLevel.IsAppearance; }
    }
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Target == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Target", name);
            enabled = false;
            return;
        }
        if (MaxLevel == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "MaxLevel", name);
            enabled = false;
            return;
        }
        if (LevelProgress == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "LevelProgress", name);
            enabled = false;
            return;
        }
        Target.EventShow += delegate { UpdateViewer(); };
    }
    void Awake()
    {
        Initialize();
    }
    private int _currentlevel = -1;
    public void UpdateViewer()
    {
        var lvl = ProgressLevels.Manager.CurrentLevelToNumber();
        if (lvl == ProgressLevels.Manager.CountLevels)
        {
            IsMaxLevel = true;
        }
        else IsMaxLevel = false;
        if (lvl != _currentlevel)
        {
            _currentlevel = lvl;
            Text.text = ProgressLevels.Manager.NextLevelToDescription();
        }
    }
}
