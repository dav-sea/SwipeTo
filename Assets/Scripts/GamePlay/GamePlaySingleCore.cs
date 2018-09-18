using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySingleCore : GamePlayCore
{
    public const string KEY_SINGLECOREBEST = "BSINGLE";
    public const string KEY_TRAINING_CUBECOMPLETE = "TCRCUBE";
    protected ObjectGame ObjectGame;

    [SerializeField] protected DefuseDifficultChange DefuseDifficultScript;

    [SerializeField]
    protected Vector3 LocalPosition;

    [SerializeField]
    private float TimeShowViewerScores = 3;

    public GamePlayData Data;

    protected bool IsPlay;

    public void SetObjectGame(ObjectGame objectGame)
    {
        ObjectGame = objectGame;
    }

    public override void Dispose()
    {
        if (ObjectGame == null) return;
        if (ObjectGame.FrontSide != null)
        {
            ObjectGame.FrontSide.Hide();
            Destroy(ObjectGame.FrontSide.gameObject, 0.4f);
            ObjectGame.SetFrontSide(null);
        }
        ObjectGame.GetDefuseManager().ActiveDefuse = false;
        ObjectGame.GetDefuseManager().UndefuseMax();

        UIContenier.Contenier.GetMainMenuController().SetMainObjectGame(ObjectGame);
        ObjectGame = null;
    }

    public override void WaitFirstAction()
    {
        if (ObjectGame != null)
        {
            ObjectGame.GetDefuseManager().DefuseFactor = 0;
        }
    }

    protected override string GetKeyBestPostprefix()
    {
        return KEY_SINGLECOREBEST;
    }

    DeferredAction.CyclicalAction LevelChecker;
    private bool _initialized;
    public override void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        Continuable = true;

        if (DefuseDifficultScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefuseDifficultScript", name);
            enabled = false;
            return;
        }


    }

    protected override void OnContinue()
    {
        OnRestart();
        Continuable = false;
    }

    public override void OnSelect()
    {
        Initialize();

        WorldEther.ObjectGameLose.Subscribe(ListnerLoseObjectGame);
        WorldEther.CoinsChange.Subscribe(ListnerCoins);
        WorldEther.LoseGame.Subscribe(ListnerLoseGame);
        WorldEther.ChangeLevel.Subscribe(ChangeLevel);
        WorldEther.ChangeScores.Subscribe(ChangeScoes);

        scoreleft = 0;



        FreezeHelper.EventFullLeft += HandlerFreeze;
    }

    private void HandlerFreeze()
    {
        if (!IsPlay) return;
        ObjectGame.GetDefuseManager().ActiveDefuse = true;
        ObjectGame.GetDefuseManager().UndefuseMax();
    }

    /// <summary>
    /// Callback sent to all game objects when the player pauses.
    /// </summary>
    /// <param name="pauseStatus">The pause state of the application.</param>
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && IsPlay)
        {
            GamePlayContenier.PauseGamePlay(this);
        }
    }

    // public override void SetVisible(bool value)
    // {
    //     if (ObjectGame != null)
    //         ObjectGame.gameObject.SetActive(value);
    // }

    void Awake()
    {
        Initialize();
    }

    protected virtual void UpdateObjectGame()
    {
        if (ObjectGame == null)
        {
            ObjectGame = UIContenier.Contenier.GetMainMenuController().GetMainObjectGame();
            UIContenier.Contenier.GetMainMenuController().SetMainObjectGame(null);
            if (ObjectGame == null)
                ObjectGame = Instantiate(PrefabsHelper.PrefabObjectGame).GetComponent<ObjectGame>();
            else ObjectGame.DisactivateDemo();
        }
        var transf = ObjectGame.transform;
        transf.parent = transform;
        // transf.localPosition = LocalPosition;
        ObjectGame.GetFollowScript().SetTarget(LocalPosition);
        ObjectGame.GetTransformManager().ForceBackRotation();
        ObjectGame.GetTouchAnimationController().DragAnimation = true;
        ObjectGame.GetTransformManager().GetRotationScript().Accelerate.AccelerateValue = 10f;
        ObjectGame.gameObject.SetActive(true);
        DefuseDifficultScript.SetTargets(ObjectGame.GetDefuseManager());
        ObjectGame.SetupFrontSide();
    }
    public override GamePlayData GetData()
    {
        return Data;
    }

    public override void OnUnselect()
    {
        WorldEther.ObjectGameLose.Unsubscribe(ListnerLoseObjectGame);
        WorldEther.CoinsChange.Unsubscribe(ListnerCoins);
        WorldEther.LoseGame.Unsubscribe(ListnerLoseGame);
        WorldEther.ChangeLevel.Unsubscribe(ChangeLevel);
        WorldEther.ChangeScores.Unsubscribe(ChangeScoes);

        LevelChecker.Cancel();

        FreezeHelper.EventFullLeft -= HandlerFreeze;
    }

    private bool? _wasTRC;
    public bool TrainingCubeComplete
    {
        set { _wasTRC = value; PlayerPrefs.SetInt(KEY_TRAINING_CUBECOMPLETE, value ? 1 : 0); }
        get
        {
            if (_wasTRC == null)
                _wasTRC = PlayerPrefs.GetInt(KEY_TRAINING_CUBECOMPLETE, 0) == 0 ? false : true;
            return (bool)_wasTRC;
        }
    }

    protected virtual void ChangeScoes(Ethers.Channel.Info inf)
    {
        if (!TrainingCubeComplete && Score.ScoreManager.CurrentScore >= 10)
        {
            TrainingCubeComplete = true;
            GamePlayContenier.ActiveTrainingReducingCube = true;
            DeferredAction.Manager.AddDeferredAction(() => GamePlayContenier.ActiveTrainingReducingCube = false, 20);
        }
    }

    //=======================================//
    protected virtual void ListnerLoseObjectGame(Ethers.Channel.Info info)
    {
        if (info.Sender == (object)ObjectGame)
        {
            // Lifes.CountLifes--;
            if (Lifes.LifesManager.CountLifes > 0)
            {
                Lifes.LifesManager.CountLifes--;
                // ObjectGame.GetDefuseManager().Undefuse(2);
                ObjectGame.OnScoundChance.Invoke();
                //Add Animation
            }
            else
            {
                WorldEther.LoseGame.Push(this, null);
            }

        }
    }
    public override void OnRestart()
    {
        UIContenier.Contenier.GetScoreViewer().Show();
        scoreleft = 0;
        LevelChecker = new DeferredAction.CyclicalAction(CheckLevel, 5);
        DeferredAction.Manager.AddDeferredAction(LevelChecker);
        UpdateObjectGame();
        FreezeHelper.FreezeTime = 0;
        Continuable = true;
    }

    private int scoreleft;
    protected void CheckLevel()
    {
        PlayerProgress.Manager.ProgressScore += Score.ScoreManager.CurrentScore - scoreleft;
        scoreleft = Score.ScoreManager.CurrentScore;
        // Debug.Log("" + PlayerProgress.Manager.ProgressScore);
    }

    protected virtual void ChangeLevel(Ethers.Channel.Info info)
    {
        MessageManager.ShowMessage(TranslationManager.GetText("Message_NewLevel"), 3);
    }

    protected virtual void ListnerLoseGame(Ethers.Channel.Info info)
    {
        ObjectGame.OnPause.Invoke();

        Coins.Manager.CoinsCount += GetBonus(Score.ScoreManager.CurrentScore);

        Dispose();

        // Debug.Log("c: " + Score.ScoreManager.CurrentScore + "s: " + scoreleft);

        PlayerProgress.Manager.ProgressScore += Score.ScoreManager.CurrentScore - scoreleft;

        LevelChecker.Cancel();

        scoreleft = 0;
        // Debug.Log(PlayerProgress.Manager.ProgressScore + "");
        UIOrganization.UIController.Controller.Clear(UIContenier.Contenier.GetLoseScreen());
        Lifes.LifesManager.CountLifes = 0;
        FreezeHelper.FreezeTime = 0;
        IsPlay = false;
    }
    // protected virtual void ListnerLife(Ethers.Channel.Info info)
    // {

    // }
    private DeferredAction.OnceAction ActionHideCoinsViewer;
    protected virtual void ListnerCoins(Ethers.Channel.Info info)
    {
        if (IsPlay)
        {
            if (ActionHideCoinsViewer != null)
                ActionHideCoinsViewer.Cancel();
            ActionHideCoinsViewer = new DeferredAction.OnceAction(delegate
            {
                if (IsPlay)
                    TopBarScreen.TopBar.ActiveViewerCoins = false;
            }, TimeShowViewerScores);
            DeferredAction.Manager.AddDeferredAction(ActionHideCoinsViewer);
            TopBarScreen.TopBar.ActiveViewerCoins = true;
        }
    }
    public override void OnPause()
    {
        ObjectGame.OnPause.Invoke();
        ObjectGame.GetDefuseManager().enabled = false;
        UIOrganization.UIController.ShowScreen(UIContenier.Contenier.GetPauseScreen());
        IsPlay = false;
        FreezeHelper.FreezeTime = 0;
    }
    // protected virtual void ListnerPause(Ethers.Channel.Info info)
    // {

    // }
    public override void OnResume()
    {
        UIContenier.Contenier.GetScoreViewer().Show();
        ObjectGame.gameObject.SetActive(true);
        ObjectGame.GetDefuseManager().enabled = true;
        ObjectGame.OnPlay.Invoke();
        IsPlay = true;
    }
    // protected virtual void ListnerResume(Ethers.Channel.Info info)
    // {

    // }
    //=======================================//
}
