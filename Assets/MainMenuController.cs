using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Screen = UIOrganization.Screen;
using UIOrganization;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Screen Target;
    [SerializeField] private Transform BlockContainier;
    private bool _lockModes = true, _lockCutom = true;
    public bool LockModes { set { _lockModes = value; } get { return _lockModes; } }
    public bool LockCustomize { set { ButtonLocker.Lock = value; } get { return ButtonLocker.Lock; } }
    [Space(5)]
    [Header("Environment")]

    [SerializeField]
    private Appearance LeftArrow;
    [SerializeField]
    private Appearance RightArrow;
    [SerializeField]
    private Appearance TextAppearance;
    [SerializeField]
    private LockButton ButtonLocker;
    [SerializeField]
    private GameObject TrainingModes;
    [SerializeField]
    private GameObject CustomizeModes;
    // [SerializeField]
    // private Appearance CustomizeButton;
    [SerializeField]
    private TextMesh Text;
    [Space(5)]
    [Header("Cores")]
    [SerializeField]
    private GamePlaySingleCore SingleCorePrefab;
    [SerializeField]
    private GamePlayTimeCore TimeCorePrefab;
    [SerializeField]
    private GamePlayVersusCore VersusCorePrefab;


    private GameModes _mode = GameModes.Single;
    public GameModes Mode
    {
        set
        {
            _mode = value; UpdateObjectGame();
            if (Target.IsAppearance) UpdateEnvironment();
        }
        get
        {
            return _mode;
        }
    }

    // [SerializeField] private Appearance Left


    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Target == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Target", name);
            enabled = false;
            return;
        }
        if (BlockContainier == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "BlockContainier", name);
            enabled = false;
            return;
        }
        if (LeftArrow == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "LeftArrow", name);
            enabled = false;
            return;
        }
        if (TrainingModes == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TrainingModes", name);
            enabled = false;
            return;
        }
        if (RightArrow == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "RightArrow", name);
            enabled = false;
            return;
        }
        if (Text == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Text", name);
            enabled = false;
            return;
        }
        if (CustomizeModes == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "CustomizeModes", name);
            enabled = false;
            return;
        }
        // Debug.Log("" + LockCustomize);
        LockCustomize = true;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        ButtonLocker.Subscribe();
        Target.EventShow += delegate
        {
            UpdateObjectGame();
            UpdateEnvironment();
            // if (!LockCustomize) CustomizeButton.Show();
        };
        Target.EventHide += delegate
        {
            if (!LockModes)
            {
                RightArrow.IsAppearance = LeftArrow.IsAppearance = TextAppearance.IsAppearance = false;
                TrainingModes.SetActive(false);
            }
            // if (!LockCustomize) CustomizeButton.Hide();
        };
    }

    public void SetGameVersusGameMode()
    {
        Mode = GameModes.Versus;
    }

    public void SwitchLeft()
    {
        switch (_mode)
        {
            case GameModes.Single:
                Mode = GameModes.Time;
                break;
            case GameModes.Time:
                Mode = GameModes.Versus;
                break;
        }
    }

    public void SwitchRight()
    {
        switch (_mode)
        {
            case GameModes.Versus:
                Mode = GameModes.Time;
                break;
            case GameModes.Time:
                Mode = GameModes.Single;
                break;
        }
    }

    public void StartPlay()
    {
        GamePlayContenier.Active = true;
        switch (_mode)
        {
            case GameModes.Single:
                var single = Instantiate(SingleCorePrefab);
                MainObjectGame.DisactivateDemo();
                single.SetObjectGame(MainObjectGame);
                MainObjectGame = null;
                // MainObjectGame.GetComponent<TargetFollowScript>().enabled = false;
                GamePlayContenier.GamePlayCore = single;
                break;
            case GameModes.Time:
                var time = Instantiate(TimeCorePrefab);
                MainObjectGame.DisactivateDemo();
                time.SetObjectGame(MainObjectGame);
                MainObjectGame = null;
                // MainObjectGame.GetComponent<TargetFollowScript>().enabled = false;
                GamePlayContenier.GamePlayCore = time;
                break;
            case GameModes.Versus:
                var versus = Instantiate(VersusCorePrefab);
                MainObjectGame.DisactivateDemo();
                OtherObjectGame.DisactivateDemo();
                // ver
                GamePlayContenier.GamePlayCore = versus;
                break;
        }
        UIOrganization.UIController.ShowScreen(UIContenier.Contenier.GetGameScreen());
        GamePlayContenier.ResumeGamePlay(null);
    }

    public const string KEY_NAME_CLASSIC = "UI_MainMenu_ModeName_Classic";
    public const string KEY_NAME_TIME = "UI_MainMenu_ModeName_Time";
    public const string KEY_NAME_VERSUS = "UI_MainMenu_ModeName_Versus";

    public const string KEY_DESCRIPTION_CLASSIC = "UI_MainMenu_ModeDescription_Classic";
    public const string KEY_DESCRIPTION_TIME = "UI_MainMenu_ModeDescription_Time";
    public const string KEY_DESCRIPTION_VERSUS = "UI_MainMenu_ModeDescription_Versus";

    public void UpdateEnvironment()
    {
        if (!LockModes)
        {
            if (_mode == GameModes.Single) { RightArrow.Hide(); LeftArrow.Show(); }
            else if (_mode == GameModes.Versus) { LeftArrow.Hide(); RightArrow.Show(); }
            else { LeftArrow.Show(); RightArrow.Show(); }
            // TranslationManager.GetText();
            TextAppearance.IsAppearance = Target.IsAppearance;
            if (_mode == GameModes.Single) { Text.text = TranslationManager.GetText(KEY_NAME_CLASSIC) + "\n\n<size=" + (Text.fontSize / 2) + ">" + TranslationManager.GetText(KEY_DESCRIPTION_CLASSIC) + "</size>"; }
            else if (_mode == GameModes.Time) { Text.text = TranslationManager.GetText(KEY_NAME_TIME) + "\n\n<size=" + (Text.fontSize / 2) + ">" + TranslationManager.GetText(KEY_DESCRIPTION_TIME) + "</size>"; }
            else { Text.text = TranslationManager.GetText(KEY_NAME_VERSUS) + "\n\n<size=" + (Text.fontSize / 2) + ">" + TranslationManager.GetText(KEY_DESCRIPTION_VERSUS) + "</size>"; }

            if (!TrainingManager.Manager.CompleteModes) TrainingModes.SetActive(true);
            else TrainingModes.SetActive(false);
        }

        if (!TrainingManager.Manager.CompleteCustomize && !LockCustomize) CustomizeModes.SetActive(true);
        else CustomizeModes.SetActive(false);
    }

    #region OBJECT GAME LOGIC
    // private bool FlagRecreate;
    private GameObject CurrentPrefabObjectGame;
    private ObjectGame MainObjectGame;
    private ObjectGame OtherObjectGame;
    public void UpdateObjectGame()
    {
        switch (_mode)
        {
            case GameModes.Versus:
                if (CurrentPrefabObjectGame != PrefabsHelper.PrefabObjectGame)
                {
                    if (MainObjectGame != null)
                    {
                        MainObjectGame.Destroy();
                        MainObjectGame.gameObject.SetActive(false);
                    }
                    if (OtherObjectGame != null)
                    {
                        OtherObjectGame.Destroy();
                        OtherObjectGame.gameObject.SetActive(false);
                    }

                    CurrentPrefabObjectGame = PrefabsHelper.PrefabObjectGame;

                    MainObjectGame = OtherObjectGame = null;
                }

                if (MainObjectGame == null) { SetMainObjectGame(CreateObjectGame()); }
                if (OtherObjectGame == null)
                {
                    SetOtherObjectGame(CreateObjectGame());
                    OtherObjectGame.transform.position = ScalePosition.PositionScale(UIContenier.Contenier.GetUICamera(), 0.5f, 0.2f, 300, Vector2.zero) + new Vector3(-50, 0, -240);
                }

                MainObjectGame.gameObject.GetComponent<TargetFollowScript>().SetTarget(ScalePosition.PositionScale(UIContenier.Contenier.GetUICamera(), 0.5f, 0.65f, 300, Vector2.zero) + new Vector3(0, 0, -240));
                OtherObjectGame.gameObject.GetComponent<TargetFollowScript>().SetTarget(ScalePosition.PositionScale(UIContenier.Contenier.GetUICamera(), 0.5f, 0.35f, 300, Vector2.zero) + new Vector3(0, 0, -240));
                break;
            default:
                if (CurrentPrefabObjectGame != PrefabsHelper.PrefabObjectGame)
                {
                    if (MainObjectGame != null) { MainObjectGame.Destroy(); MainObjectGame.gameObject.SetActive(false); }
                    if (OtherObjectGame != null) { OtherObjectGame.Destroy(); OtherObjectGame.gameObject.SetActive(false); }

                    CurrentPrefabObjectGame = PrefabsHelper.PrefabObjectGame;

                    MainObjectGame = OtherObjectGame = null;
                }
                if (MainObjectGame == null) { SetMainObjectGame(CreateObjectGame()); }
                MainObjectGame.gameObject.GetComponent<TargetFollowScript>().SetTarget(ScalePosition.PositionScale(UIContenier.Contenier.GetUICamera(), 0.5f, 0.5f, 300, Vector2.zero) + new Vector3(0, 0, -260));
                if (OtherObjectGame != null) { OtherObjectGame.gameObject.GetComponent<TargetFollowScript>().SetTarget(ScalePosition.PositionScale(UIContenier.Contenier.GetUICamera(), 0.5f, 0.2f, 300, Vector2.zero) + new Vector3(-50, 0, -240)); }
                break;
        }
    }
    public Screen GetScreen()
    {
        return Target;
    }
    private ObjectGame CreateObjectGame()
    {
        var gameObj = Instantiate(PrefabsHelper.PrefabObjectGame);
        var objectGame = gameObj.GetComponent<ObjectGame>();
        var follow = objectGame.GetFollowScript();

        gameObj.transform.position = BlockContainier.position;

        follow.Initialize();
        follow.Accelerate.AccelerateValue = 10;
        follow.DisableForFinish = true;

        return objectGame;
    }

    public void SetMainObjectGame(ObjectGame objectGame)
    {
        // if (MainObjectGame != null)
        // {
        //     MainObjectGame.Destroy();
        //     MainObjectGame.Hide();
        //     MainObjectGame = null;
        // }
        MainObjectGame = objectGame;
        if (objectGame == null) return;

        var transfObj = objectGame.transform;
        var follow = objectGame.GetFollowScript();

        follow.Initialize();

        transfObj.parent = BlockContainier;
        // transfObj.localPosition = Vector3.zero;
        if (_mode == GameModes.Versus)
            objectGame.GetFollowScript().SetTarget(ScalePosition.PositionScale(UIContenier.Contenier.GetUICamera(), 0.5f, 0.8f, 300, Vector2.zero) + new Vector3(0, 0, -240));
        else
            objectGame.GetFollowScript().SetTarget(BlockContainier.position);
        // transfObj.localRotation = Quaternion.identity;

        objectGame.Initialize();
        objectGame.GetDefuseManager().ScopeDefuse = 1;
        objectGame.ActivateDemo();

        objectGame.GetTransformManager().ForceBackRotation();
        objectGame.GetTouchAnimationController().DragAnimation = true;

        objectGame.GetTransformManager().GetRotationScript().Accelerate.AccelerateValue = 0.15f;

        objectGame.gameObject.SetActive(true);


    }
    public void SetOtherObjectGame(ObjectGame objectGame)
    {
        // if (OtherObjectGame != null)
        // {
        //     OtherObjectGame.Destroy();
        //     OtherObjectGame.Hide();
        //     OtherObjectGame = null;
        // }
        OtherObjectGame = objectGame;
        if (objectGame == null) return;

        var transfObj = objectGame.transform;
        var follow = objectGame.GetFollowScript();

        follow.Initialize();

        transfObj.parent = BlockContainier;
        // transfObj.localPosition = Vector3.zero;
        if (_mode == GameModes.Versus)
        {
            objectGame.GetFollowScript().SetTarget(ScalePosition.PositionScale(UIContenier.Contenier.GetUICamera(), 0.5f, 0.2f, 300, Vector2.zero) + new Vector3(0, 0, -240));
        }
        // objectGame.GetFollowScript().SetTarget(BlockContainier.position);
        // transfObj.localRotation = Quaternion.identity;

        objectGame.Initialize();
        objectGame.GetDefuseManager().UndefuseMax();
        objectGame.ActivateDemo();

        objectGame.GetTransformManager().ForceBackRotation();
        objectGame.GetTouchAnimationController().DragAnimation = true;

        objectGame.GetTransformManager().GetRotationScript().Accelerate.AccelerateValue = 0.15f;

        objectGame.gameObject.SetActive(true);


    }

    public ObjectGame GetMainObjectGame()
    {
        return MainObjectGame;
    }
    public ObjectGame GetOtherObjectGame()
    {
        return OtherObjectGame;
    }
    #endregion

    void Awake()
    {
        Initialize();
    }

    public enum GameModes { Single, Time, Versus }
}
