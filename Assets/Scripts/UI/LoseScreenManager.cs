using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class LoseScreenManager : MonoBehaviour
{
    public static float TimeToContinue = 3f;

    [Header("Main")]
    [SerializeField]
    UIOrganization.Screen LoseScreen;
    [Header("Scores")]
    [Space(5)]
    [SerializeField]
    Text CurrentScore;
    [SerializeField]
    Text BestScores;
    [SerializeField]
    UnityEngine.UI.Text BonusText;

    [Header("Continue")]
    [Space(5)]
    [SerializeField]
    Appearance Continue;
    [SerializeField]
    Appearance Restart;
    [SerializeField]
    UIOrganization.TouchComponent Button;

    [SerializeField]
    UnityEngine.UI.Image CircleImageContinue;

    [SerializeField]
    ProgressBar ContinueTime;

    [Header("Other")]
    [Space(5)]
    [SerializeField]
    private ParticleManager NewScoreParticles;
    private bool _needUpdateScores;
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (CurrentScore == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "CurrentScore", name);
            enabled = false;
            return;
        }
        if (Restart == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Restart", name);
            enabled = false;
            return;
        }
        if (Button == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Button", name);
            enabled = false;
            return;
        }
        if (ContinueTime == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ContinueTime", name);
            enabled = false;
            return;
        }
        if (CircleImageContinue == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "CircleImageContinue", name);
            enabled = false;
            return;
        }
        if (BestScores == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "BestScores", name);
            enabled = false;
            return;
        }
        if (LoseScreen == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "LoseScreen", name);
            enabled = false;
            return;
        }
        if (NewScoreParticles == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "NewScoreParticles", name);
            enabled = false;
            return;
        }
        if (Continue == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ButtonContinue", name);
            enabled = false;
            return;
        }


        System.Action FinishAction = delegate
        {
            DeferredAction.Manager.AddDeferredAction(delegate
            {
                GamePlayContenier.CoreContinue();
                UIOrganization.UIController.ShowScreen(UIContenier.Contenier.GetGameScreen());
                GamePlayContenier.ResumeGamePlay(this);
            }, 0.67f);
        };

        Button.EventClick += delegate
        {
            if (IsContinueStateButton)
            {
                AdsManager.Manager.ShowNonSkipable(FinishAction);
                // IsContinueStateButton = false;
                enabled = false;
            }
            else
            {
                UIOrganization.UIController.ShowScreen(UIContenier.Contenier.GetGameScreen());
                GamePlayContenier.Restart(null);
                GamePlayContenier.ResetGameSystems();
                GamePlayContenier.ResumeGamePlay(null);
            }
        };

        MessageManager.IHideMessage BestMessage = null;

        LoseScreen.EventShow += delegate
        {
            UpdateBonus();
            IsContinueStateButton = GamePlayContenier.GamePlayCore.Continuable;
            if (ChangeLevelFlag)
            {
                ChangeLevelFlag = false;
                // LoseScreen.Hide();
                LoseScreen.GameObject.SetActive(false);
                UIOrganization.UIController.ShowScreen(UIContenier.Contenier.GetNewLevelScreen());
            }
            else
            {
                if (Score.ScoreManager.CurrentScore > GamePlayContenier.GamePlayCore.BestResult())
                {
                    var main = NewScoreParticles.Main;
                    main.prewarm = true;
                    NewScoreParticles.EmissonActive = true;
                    GamePlayContenier.GamePlayCore.SetBestResult(Score.ScoreManager.CurrentScore);
                    if (BestMessage != null) LoseScreen.EventHide -= BestMessage.Hide;
                    BestMessage = MessageManager.ShowMessage(TranslationManager.GetText("Message_NewBest"));
                    LoseScreen.EventHide += BestMessage.Hide;
                }
                UpdateScores();
                UpdateBest();
            }
        };


    }

    public bool IsContinueStateButton
    {
        set
        {
            Continue.IsAppearance = value;
            Restart.IsAppearance = !value;
            if (value)
            {
                _ContinueTime = TimeToContinue;
                enabled = true;
            }
        }
        get { return Continue.IsAppearance; }
    }

    // public void UpdateColors()
    // {
    //     // CircleImageContinue.color = Palette.PaletteManager.PaletteConfiguration.GetLoseColor();
    //     // CurrentScore.color = Palette.PaletteManager.PaletteConfiguration.GetLoseColor();
    //     // BestScores.color = Palette.PaletteManager.PaletteConfiguration.GetUITextColor();
    // }
    public void UpdateScores()
    {
        CurrentScore.text = Score.ScoreManager.CurrentScore.ToString();
    }
    public void UpdateBest()
    {
        BestScores.text = TranslationManager.GetText("UI_Best") + ": " + GamePlayContenier.GamePlayCore.BestResult();
    }
    public void ContinueCore()
    {
        GamePlayContenier.CoreContinue();
    }

    public void UpdateBonus()
    {
        var bonus = GamePlayContenier.GamePlayCore.GetBonus(Score.ScoreManager.CurrentScore);
        BonusText.text = bonus > 0 ? string.Format(TranslationManager.GetText("UI_Bonus"), "+" + bonus) : "";
    }

    void Start()
    {
        Initialize();
        // WorldEther.ChangePalette.Subscribe(HandlerPaletteChange);
        WorldEther.ChangeLevel.Subscribe(HandlerChangeLevel);
    }
    // private void HandlerPaletteChange(Ethers.Channel.Info info)
    // {
    //     UpdateColors();
    // }
    private bool ChangeLevelFlag;
    private void HandlerChangeLevel(Ethers.Channel.Info info)
    {
        ChangeLevelFlag = true;
    }

    private float _ContinueTime = TimeToContinue;
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        UpdateContinueView();
    }

    private void UpdateContinueView()
    {
        _ContinueTime -= Time.deltaTime;
        if (_ContinueTime <= 0)
        {
            _ContinueTime = 0;
            enabled = false;
            IsContinueStateButton = false;
        }
        ContinueTime.Progress = Mathf.Clamp(_ContinueTime / TimeToContinue, 0, 1);

    }
}
