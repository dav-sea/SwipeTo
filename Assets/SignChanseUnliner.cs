using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignChanseUnliner : MonoBehaviour
{
    [SerializeField] GamePlayCore Core;

    [Range(5, 200)] public int ScoresUpdateInterval = 50;


    [Space(5)]
    [SerializeField]
    AnimationCurve Arrow;
    [SerializeField] AnimationCurve Swipes;
    [SerializeField] AnimationCurve Lose;
    [SerializeField] AnimationCurve Empty;
    [SerializeField] AnimationCurve Transfer;
    [SerializeField] AnimationCurve Multiplier;
    [SerializeField] AnimationCurve Life;
    [SerializeField] AnimationCurve Coin;
    [Space(5)]
    [SerializeField]
    AnimationCurve SwipesMin;
    [SerializeField] AnimationCurve SwipesMax;

    private bool _initialized;

    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Core == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", " Core", name);
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
        WorldEther.ChangeScores.Subscribe(ScoreHandler);
        UpdateChances(Score.ScoreManager.CurrentScore);
        WorldEther.RestartGame.Subscribe(RestartHandler);
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangeScores.Unsubscribe(ScoreHandler);
        WorldEther.RestartGame.Unsubscribe(RestartHandler);
    }

    private int _previousScoreUpdate;

    public void ResetCounterScores()
    {
        _previousScoreUpdate = 0;
    }

    private void ScoreHandler(Ethers.Channel.Info inf)
    {
        if (_previousScoreUpdate > Score.ScoreManager.CurrentScore)
            ResetCounterScores();
        else if (Score.ScoreManager.CurrentScore - _previousScoreUpdate >= ScoresUpdateInterval)
            UpdateChances(Score.ScoreManager.CurrentScore);
    }

    private void RestartHandler(Ethers.Channel.Info inf)
    {
        UpdateChances(0);
    }

    public void UpdateChances(int scores)
    {
        float coff = Mathf.Clamp((float)scores / (float)1000, 0, 1);

        var data = Core.GetData();
        var locker = SignProgressLocker.Manager;

        Debug.Log("TODO");


        data.ChanceSimpleArrow.Value = Arrow.Evaluate(coff);
        data.ChanceMultiSwipes.Value = !locker.SwipesLock ? Swipes.Evaluate(coff) : 0;
        data.ChanceLose.Value = !locker.LoseLock ? Lose.Evaluate(coff) : 0;
        data.ChanceEmpty.Value = Empty.Evaluate(coff);
        data.ChanceTransfer.Value = !locker.TransferLock ? Transfer.Evaluate(coff) : 0;
        data.ChanceMultiplier.Value = !locker.MultiplierLock ? Multiplier.Evaluate(coff) : 0;
        data.ChanceLife.Value = !locker.LifesLock ? Life.Evaluate(coff) : 0;
        data.ChanceCoin.Value = !locker.CoinsLock ? Coin.Evaluate(coff) : 0;
        data.SwipesRandomCount.DefaultMaxValue.Value = SwipesMax.Evaluate(coff);
        data.SwipesRandomCount.DefaultMinValue.Value = SwipesMin.Evaluate(coff);

        _previousScoreUpdate = scores;
    }

    // private UpdateParametr(Parameter param, )

}
