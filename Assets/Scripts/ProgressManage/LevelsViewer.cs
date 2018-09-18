using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelsViewer : MonoBehaviour
{
    [SerializeField] private ProgressBar Bar;
    [SerializeField] private Text TextViewer;
    [SerializeField] private TextMesh NeedScores;


    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
    }
    public void UpdateViewer()
    {
        // Debug.Log("update");
        var nextscores = ProgressLevels.Manager.NextLevelToTargetScore();
        var lvl = ProgressLevels.Manager.CurrentLevelToNumber();

        Bar.Progress = lvl == 0 ? 1 : (float)(PlayerProgress.Manager.ProgressScore - ProgressLevels.Manager.CurrentLevelToTargetScore()) / (float)nextscores;
        if (lvl == 0) lvl = ProgressLevels.Manager.CountLevels;
        TextViewer.text = lvl + "<size=50> " + TranslationManager.GetText("UI_Lvl") + "</size> / " + ProgressLevels.Manager.CountLevels;
        if (NeedScores != null)
        {
            // Debug.Log(TranslationManager.GetText("Format_NeedScores"));
            NeedScores.text = string.Format(TranslationManager.GetText("Format_NeedScores"), (Mathf.Clamp(nextscores - PlayerProgress.Manager.ProgressScore, 0, nextscores)));
        }
        // Debug.Log("" + Bar.Progress);
    }

    private void Handler(Ethers.Channel.Info info)
    {
        UpdateViewer();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        WorldEther.ProgressScoreChagne.Subscribe(Handler);
        UpdateViewer();
    }

    void Awake()
    {
        Initialize();
    }
}
