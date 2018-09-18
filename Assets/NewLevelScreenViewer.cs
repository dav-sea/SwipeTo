using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLevelScreenViewer : MonoBehaviour
{
    [SerializeField] private UIOrganization.Screen Target;
    [SerializeField] private UnityEngine.UI.Text NumberLevel;
    [SerializeField] private UnityEngine.UI.Text LevelInfo;

    private int _coins;

    public void UpdateViewer()
    {
        var lvl = ProgressLevels.Manager.CurrentLevelToNumber();
        if (lvl == 0)//Max Level
            lvl = ProgressLevels.Manager.CountLevels;

        NumberLevel.text = lvl.ToString();
        LevelInfo.text = ProgressLevels.Manager.CurrentLevelToDescription();
        _coins = lvl * 5;
    }

    public void OnTakeCoins()
    {
        Coins.Manager.CoinsCount += _coins;
        _coins = 0;
        UIOrganization.UIController.BackScreen();
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
        if (NumberLevel == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "NumberLevel", name);
            enabled = false;
            return;
        }
        if (LevelInfo == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "LevelInfo", name);
            enabled = false;
            return;
        }

        Target.EventShow += UpdateViewer;
    }
    void Awake()
    {
        Initialize();
    }
}
