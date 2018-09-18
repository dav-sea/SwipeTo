using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBestScoreViewer : MonoBehaviour
{
    [SerializeField] TextMesh Text;
    [SerializeField] ModeSelector Mode;
    [SerializeField] Appearance Appearance;

    // int targetScores

    public bool Affect = true;
    public void UpdateScores()
    {
        if (!(Mode.CorePrefab is GamePlaySingleCore) || Mode.CorePrefab is GamePlayTimeCore) { Appearance.Hide(); return; }
        {
            if (Affect) Text.text = Mode.CorePrefab.BestResult() / 2 + ""; else Score.ScoreManager.CurrentScore = 0;
            Appearance.Show();
        }
    }

    public void SetupScores()
    {
        if (Mode.CorePrefab is GamePlaySingleCore && !(Mode.CorePrefab is GamePlayTimeCore))
        {
            if (Affect)
            {
                Score.ScoreManager.CurrentScore = Mode.CorePrefab.BestResult() / 2;
            }
            else Score.ScoreManager.CurrentScore = 0;
        }
    }

    public void SwitchAffect()
    {
        Affect = !Affect;
        Text.text = Affect ? Mode.CorePrefab.BestResult() / 2 + "" : "0";
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Text == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Text", name);
            enabled = false;
            return;
        }
        if (Mode == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Mode", name);
            enabled = false;
            return;
        }
        if (Appearance == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Appearance", name);
            enabled = false;
            return;
        }
    }
    void Awake()
    {
        Initialize();
    }
}
