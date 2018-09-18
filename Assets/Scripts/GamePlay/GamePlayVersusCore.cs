using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class GamePlayVersusCore : GamePlayCore
{
    public GamePlayData Data;
    public const string KEY_VERSUSCOREBEST = "BVERSUS";//Заглушка

    public float UndefuseScopeSwipe = 0.1f;

    protected bool IsPlay;
    private ObjectGame TopObject, BottomObject;
    [SerializeField] Vector3 TopPosition;
    [SerializeField] Vector3 BottomPosition;
    [SerializeField] DefuseDifficultChange DefuseDifficultScript;

    [SerializeField] DualLoseScreen CustomLoseScreen;

    public override void Dispose()
    {
        if (TopObject != null)
        {
            TopObject.EventSwipe -= SwipeTop;
            if (TopObject.FrontSide != null)
            {
                TopObject.FrontSide.Hide();
                Destroy(TopObject.FrontSide.gameObject, 0.4f);
                TopObject.SetFrontSide(null);
            }
            TopObject.GetDefuseManager().ActiveDefuse = false;
            TopObject.GetDefuseManager().UndefuseMax();

            UIContenier.Contenier.GetMainMenuController().SetMainObjectGame(TopObject);
            TopObject = null;
        }

        if (BottomObject != null)
        {
            BottomObject.EventSwipe -= SwipeBottom;
            if (BottomObject.FrontSide != null)
            {
                BottomObject.FrontSide.Hide();
                Destroy(BottomObject.FrontSide.gameObject, 0.4f);
                BottomObject.SetFrontSide(null);
            }
            BottomObject.GetDefuseManager().ActiveDefuse = false;
            BottomObject.GetDefuseManager().UndefuseMax();

            UIContenier.Contenier.GetMainMenuController().SetOtherObjectGame(BottomObject);
            BottomObject = null;
        }

        DefuseDifficultScript.SetTargets(null);
    }

    //TODO
    protected virtual void UpdateObjectsGame()
    {
        if (TopObject == null)
        {
            TopObject = UIContenier.Contenier.GetMainMenuController().GetMainObjectGame();
            UIContenier.Contenier.GetMainMenuController().SetMainObjectGame(null);
            if (TopObject == null)
                TopObject = Instantiate(PrefabsHelper.PrefabObjectGame).GetComponent<ObjectGame>();
        }
        if (BottomObject == null)
        {
            BottomObject = UIContenier.Contenier.GetMainMenuController().GetOtherObjectGame();
            UIContenier.Contenier.GetMainMenuController().SetOtherObjectGame(null);
            if (BottomObject == null)
                BottomObject = Instantiate(PrefabsHelper.PrefabObjectGame).GetComponent<ObjectGame>();
        }

        TopObject.DisactivateDemo();
        BottomObject.DisactivateDemo();



        TopObject.transform.parent = transform;
        BottomObject.transform.parent = transform;

        TopObject.GetFollowScript().SetTarget(TopPosition + transform.position);
        BottomObject.GetFollowScript().SetTarget(BottomPosition + transform.position);

        TopObject.EventSwipe += SwipeTop;
        BottomObject.EventSwipe += SwipeBottom;

        TopObject.SetupFrontSide();
        BottomObject.SetupFrontSide();

        // Debug.Log

        TopObject.GetDefuseManager().ActiveDefuse = false;
        BottomObject.GetDefuseManager().ActiveDefuse = false;

        // TopObject.GetDefuseManager().UndefuseFactor = 1;
        // BottomObject.GetDefuseManager().UndefuseFactor = 1;

        TopObject.GetDefuseManager().UndefuseScore = 0;
        BottomObject.GetDefuseManager().UndefuseScore = 0;
        TopObject.GetDefuseManager().DefuseFactor = 0;
        TopObject.GetDefuseManager().ScopeDefuse = 0.5f;
        BottomObject.GetDefuseManager().DefuseFactor = 0;
        BottomObject.GetDefuseManager().ScopeDefuse = 0.5f;

        BottomObject.GetTransformManager().GetRotationScript().Accelerate.AccelerateValue = 10f;
        TopObject.GetTransformManager().GetRotationScript().Accelerate.AccelerateValue = 10f;


        DefuseDifficultScript.SetTargets(TopObject.GetDefuseManager(), BottomObject.GetDefuseManager());
    }
    public override int GetBonus(int scores)
    {
        return 0;
    }
    private void SwipeTop()
    {
        TopObject.GetDefuseManager().UndefuseScore += UndefuseScopeSwipe;
        BottomObject.GetDefuseManager().UndefuseScore -= UndefuseScopeSwipe;
    }
    private void SwipeBottom()
    {
        TopObject.GetDefuseManager().UndefuseScore -= UndefuseScopeSwipe;
        BottomObject.GetDefuseManager().UndefuseScore += UndefuseScopeSwipe;
    }
    public override GamePlayData GetData()
    {
        return Data;
    }
    protected override string GetKeyBestPostprefix()
    {
        return KEY_VERSUSCOREBEST;
    }
    private bool _initialized;
    public override void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (DefuseDifficultScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefuseDifficultScript", name);
            enabled = false;
            return;
        }
        if (CustomLoseScreen == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "CustomLoseScreen", name);
            enabled = false;
            return;
        }

    }
    public override void OnSelect()
    {
        WorldEther.ObjectGameLose.Subscribe(ListnerLoseObjectGame);

        WorldEther.CoinsChange.Subscribe(ListnerCoins);
        WorldEther.LoseGame.Subscribe(ListnerLoseGame);

        AudioContainer.Manager.CriticalDefuseLoop.gameObject.SetActive(false);
    }
    public override void WaitFirstAction()
    {
        Debug.Log("TODO IT?");
    }
    public override void OnUnselect()
    {
        // WorldEther.ChangeLifes.Unsubscribe(ListnerLife);
        // WorldEther.RestartGame.Unsubscribe(ListnerRestart);
        WorldEther.ObjectGameLose.Unsubscribe(ListnerLoseObjectGame);
        // WorldEther.PauseGame.Unsubscribe(ListnerPause);
        // WorldEther.ResumeGame.Unsubscribe(ListnerResume);
        WorldEther.CoinsChange.Unsubscribe(ListnerCoins);
        WorldEther.LoseGame.Unsubscribe(ListnerLoseGame);
        if (UIOrganization.UIController.Controller.ActiveScreen == CustomLoseScreen.GetScreen())
        {
            UIOrganization.UIController.BackToRootScreen();
        }
        CustomLoseScreen.GetScreen().Hide();
        Destroy(CustomLoseScreen.gameObject, 3);
        AudioContainer.Manager.CriticalDefuseLoop.gameObject.SetActive(true);
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

    protected virtual void ListnerLoseObjectGame(Ethers.Channel.Info info)
    {
        if (info.Sender == (object)TopObject)
        {
            CustomLoseScreen.BottomWin();
            WorldEther.LoseGame.Push(this, null);
        }
        else if (info.Sender == (object)BottomObject)
        {
            CustomLoseScreen.TopWin();
            WorldEther.LoseGame.Push(this, null);
        }
    }
    public override void OnRestart()
    {
        UIContenier.Contenier.GetScoreViewer().Hide();
        // Debug.Log("hide ");
        Dispose();
        UpdateObjectsGame();

        // DefuseDifficultScript.SetTargets(TopObject.GetDefuseManager(), BottomObject.GetDefuseManager());
    }


    protected virtual void ListnerLoseGame(Ethers.Channel.Info info)
    {
        Dispose();
        // UIContenier.Contenier.GetGameState().Hide();
        // UIContenier.Contenier.GetLoseState().Show();

        CustomLoseScreen.transform.parent = null;
        UIOrganization.UIController.Controller.Clear(CustomLoseScreen.GetScreen());
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
            }, 3);
            DeferredAction.Manager.AddDeferredAction(ActionHideCoinsViewer);
            TopBarScreen.TopBar.ActiveViewerCoins = true;
        }
    }
    public override void OnPause()
    {
        TopObject.OnPause.Invoke();
        BottomObject.OnPause.Invoke();
        UIOrganization.UIController.ShowScreen(UIContenier.Contenier.GetPauseScreen());
        IsPlay = false;
    }
    public override void OnResume()
    {
        TopObject.gameObject.SetActive(true);
        TopObject.OnPlay.Invoke();
        BottomObject.gameObject.SetActive(true);
        BottomObject.OnPlay.Invoke();
        IsPlay = true;
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        CustomLoseScreen.gameObject.SetActive(false);
    }
}