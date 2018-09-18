using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefuseDifficultChange : MonoBehaviour
{
    private float LimitScores = 1000;

    private DefuseManager[] Targets;

    [SerializeField]
    AnimationCurve DifficultCurve;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        // if (Target == null)
        // {
        //     Debug.LogWarningFormat("{0} (in {1}) is null", "Target", name);
        //     enabled = false;
        //     return;
        // }
        // Target.Initialize();
        WorldEther.ChangeScores.Subscribe(HandlerChangeScores);
        UpdateDefuseFactor();
    }
    public void SetTargets(params DefuseManager[] managers)
    {
        Targets = managers;
    }

    public void Flash()
    {
        Targets = null;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangeScores.Unsubscribe(HandlerChangeScores);
    }

    void Awake()
    {
        Initialize();
    }

    public void UpdateDefuseFactor()
    {
        if (Targets == null) return;
        float factor = DifficultCurve.Evaluate(Mathf.Clamp(Score.ScoreManager.CurrentScore / LimitScores, 0, 1));
        foreach (DefuseManager manager in Targets)
            manager.DefuseFactor = factor;
    }

    private void HandlerChangeScores(Ethers.Channel.Info info)
    {
        UpdateDefuseFactor();
    }
}
