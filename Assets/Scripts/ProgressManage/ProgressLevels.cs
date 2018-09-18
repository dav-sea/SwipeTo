using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLevels : MonoBehaviour
{
    public static ProgressLevels Manager { private set; get; }
    [SerializeField] ProgressLevel[] Levels;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Manager != null)
        {
            Destroy(this);
            return;
        }
        Manager = this;
    }

    public int CountLevels { get { return Levels.Length; } }

    public bool UpdateLevels()
    {
        bool result = false;
        var progress = PlayerProgress.Manager.ProgressScore;
        foreach (ProgressLevel lvl in Levels)
            result = lvl.Check(progress) || result;
        if (result != false) WorldEther.ChangeLevel.Push(null, null);
        return result;
    }

    public int NextLevelToTargetScore()
    {
        var lvl = GetNext();
        return lvl != null ? lvl.targetProgressScores : PlayerProgress.Manager.ProgressScore;
    }
    public int CurrentLevelToTargetScore()
    {
        var lvl = GetCurrent();
        return lvl != null ? lvl.targetProgressScores : PlayerProgress.Manager.ProgressScore;
    }
    public string CurrentLevelToDescription()
    {
        var lvl = GetCurrent();
        return lvl != null ? TranslationManager.GetText(lvl.DescriptionNameTranslation) : "";
    }
    public string NextLevelToDescription()
    {
        var lvl = GetNext();
        return lvl != null ? TranslationManager.GetText(lvl.DescriptionNameTranslation) : "";
    }
    public int CurrentLevelToNumber()
    {
        var lvl = GetCurrent();
        return lvl != null ? lvl.Number : 0;
    }

    private ProgressLevel GetCurrent()
    {
        ProgressLevel prev = Levels[0];
        foreach (ProgressLevel lvl in Levels)
            if (lvl.targetProgressScores > PlayerProgress.Manager.ProgressScore)
                return prev;
            else prev = lvl;
        return prev;
    }

    private ProgressLevel GetNext()
    {
        foreach (ProgressLevel lvl in Levels)
            if (lvl.targetProgressScores > PlayerProgress.Manager.ProgressScore)
                return lvl;
        return null;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        var progress = PlayerProgress.Manager.ProgressScore;
        foreach (ProgressLevel lvl in Levels)
            lvl.Check(progress);
        WorldEther.ProgressScoreChagne.Subscribe(HandlerChangeProgress);
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ProgressScoreChagne.Unsubscribe(HandlerChangeProgress);
    }

    private void HandlerChangeProgress(Ethers.Channel.Info info)
    {
        UpdateLevels();
    }



    void Awake()
    {
        Initialize();
    }
    [System.Serializable]
    private class ProgressLevel
    {
        public bool wasAction;
        public int targetProgressScores;
        public int Number;
        public string DescriptionNameTranslation;
        [SerializeField] UnityEngine.Events.UnityEvent OnLevel;

        public bool Check(int progress)
        {
            if (wasAction || targetProgressScores > progress) return false;
            OnLevel.Invoke();
            wasAction = true;
            return true;
        }

    }
}
