using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContenier : MonoBehaviour
{
    public static UIContenier Contenier { private set; get; }

    [SerializeField]
    private UIOrganization.Screen LoseScreen;

    [SerializeField]
    private UIOrganization.Screen GameScreen;

    [SerializeField]
    private UIOrganization.Screen PauseScreen;
    [SerializeField]
    private UIOrganization.Screen NewLevelScreen;

    // [SerializeField] UIOrganization.Screen RootScreen;

    [SerializeField] MainMenuController MainMenuController;

    [SerializeField]
    private Camera UICamera;

    [SerializeField]
    private Camera MainCamera;

    [SerializeField]
    private ScoreViewer ScoreViewer;

    public ScoreViewer GetScoreViewer()
    {
        return ScoreViewer;
    }

    public MainMenuController GetMainMenuController()
    {
        return MainMenuController;
    }

    public UIOrganization.Screen GetLoseScreen()
    {
        return LoseScreen;
    }
    public UIOrganization.Screen GetNewLevelScreen()
    {
        return NewLevelScreen;
    }

    public UIOrganization.Screen GetGameScreen()
    {
        return GameScreen;
    }

    public UIOrganization.Screen GetPauseScreen()
    {
        return PauseScreen;
    }

    public UIOrganization.Screen GetRootScreen()
    {
        return MainMenuController.GetScreen();
    }

    public Camera GetUICamera()
    {
        return UICamera;
    }
    public Camera GetMainCamera()
    {
        return MainCamera;
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Contenier == null)
        {
            Contenier = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        if (LoseScreen == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "LoseScreen", name);
            enabled = false;
            return;
        }
        if (GameScreen == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "GameScreen", name);
            enabled = false;
            return;
        }
        if (MainMenuController == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "MainMenuController", name);
            enabled = false;
            return;
        }
        if (PauseScreen == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "PauseScreen", name);
            enabled = false;
            return;
        }
        if (ScoreViewer == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ScoreViewer", name);
            enabled = false;
            return;
        }
        if (UICamera == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "UICamera", name);
            enabled = false;
            return;
        }
        if (MainCamera == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "MainCamera", name);
            enabled = false;
            return;
        }
        if (NewLevelScreen == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "NewLevelScreen", name);
            enabled = false;
            return;
        }
    }
    void Awake()
    {
        Initialize();
    }
}