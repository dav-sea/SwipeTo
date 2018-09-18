using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreViewer : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Text ScoreText;

    [SerializeField]
    private UnityEngine.UI.Text MultiplierText;

    private RectTransform MultiplierTransform;

    private Score ScoreManager;
    private int previousScore;
    private float previousMultiplier;

    float _scopevalue;
    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.MultiplierTime > 0)
        {
            MultiplierText.enabled = true;
            _scopevalue = ScoreManager.ScopeTimeMultiplier;
            _scopevalue = Mathf.Clamp(_scopevalue, 0.05f, 1);
            MultiplierTransform.localScale = new Vector3(_scopevalue, _scopevalue, _scopevalue);
        }
        else
        {
            MultiplierText.enabled = false;
        }
    }

    public void Show()
    {
        ScoreText.enabled = MultiplierText.enabled = true;
        enabled = true;
    }

    public void Hide()
    {
        ScoreText.enabled = MultiplierText.enabled = false;
        enabled = false;
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (ScoreText == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ScoreText", name);
            enabled = false;
            return;
        }
        if (MultiplierText == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "MultiplierText", name);
            enabled = false;
            return;
        }
        MultiplierTransform = MultiplierText.GetComponent<RectTransform>();
        ScoreManager = Score.ScoreManager;
        WorldEther.ChangeScores.Subscribe(HandlerChangeScores);
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangeScores.Unsubscribe(HandlerChangeScores);
    }


    private void HandlerChangeScores(Ethers.Channel.Info info)
    {
        UpdateViewer();
    }

    public void UpdateViewer()
    {
        if (previousScore != ScoreManager.CurrentScore)
        {
            previousScore = ScoreManager.CurrentScore;
            ScoreText.text = ScoreManager.CurrentScore.ToString();
        }

        if (previousScore != 1)// TODO ???
        {
            if (previousMultiplier != ScoreManager.Multiplier)
            {
                previousMultiplier = ScoreManager.Multiplier;
                MultiplierText.text = "x" + (int)ScoreManager.Multiplier;
            }
        }
        else
        {
            MultiplierText.enabled = false;
        }
    }

    void Awake()
    {
        Initialize();
    }
}
