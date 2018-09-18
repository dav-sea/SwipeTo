using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    // public UnityEngine.Events.UnityEvent ChangeScore
    public static Score ScoreManager { private set; get; }

    [SerializeField]
    private bool OnlyIntegerMultiplier = true;

    public bool DefuseMultiplierTime = true;

    public float MaxMultiplerTime = 15;

    [SerializeField]
    private float MaxMultiplier = 8;

    public int CurrentScore
    {
        set
        {
            _Score = value; WorldEther.ChangeScores.Push(this, null);
        }
        get { return (int)Mathf.Round(_Score); }
    }

    public void AddScores(float value)
    {
        CurrentScore += (int)Mathf.Round(value * Multiplier);
    }

    [SerializeField]
    private float _Multiplier;
    public float Multiplier
    {
        set
        {
            _Multiplier = Mathf.Clamp(OnlyIntegerMultiplier ? Mathf.Round(value) : value, 0, MaxMultiplier);
            WorldEther.ChangeScores.Push(this, null);

        }
        get { return _Multiplier; }
    }

    public float ScopeMaxFactor = 2;

    public float ScopeTimeMultiplier
    {
        get { return Mathf.Clamp(MultiplierTime / ScopeMaxFactor, 0, 1); }
    }

    private float _MultiplierTime;
    public float MultiplierTime
    {
        set { _MultiplierTime = Mathf.Clamp(value, 0, MaxMultiplerTime); }
        get { return _MultiplierTime; }
    }

    private float _Score;

    float _diff;
    int previous;
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (MultiplierTime > 0 && DefuseMultiplierTime)
        {
            MultiplierTime -= Time.unscaledDeltaTime;
            if (MultiplierTime <= 0) Multiplier = 1;
        }
    }

    [ContextMenu("Add 250")]
    private void EditorAddScores()
    {
        CurrentScore += 250;
    }

    [ContextMenu("Push to 1000")]
    private void EditorPushScores()
    {
        CurrentScore = 1000;
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (ScoreManager == null)
        {
            ScoreManager = this;
        }
        else
        {
            Destroy(this);
        }

        WorldEther.PauseGame.Subscribe(delegate (Ethers.Channel.Info info)
        {
            DefuseMultiplierTime = false;
        });
        WorldEther.ResumeGame.Subscribe(delegate (Ethers.Channel.Info info)
        {
            DefuseMultiplierTime = true;
        });
    }
    void Awake()
    {
        Initialize();
    }
}
