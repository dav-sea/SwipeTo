using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIOrganization;
using Screen = UIOrganization.Screen;

public class TopBarScreen : MonoBehaviour
{
    public static TopBarScreen TopBar { private set; get; }

    [SerializeField]
    private Appearance BackScreen;

    [SerializeField]
    private Appearance Coins;

    [SerializeField] LockButton DailyLocker;

    [SerializeField]
    private Appearance ProgressViewer;
    [SerializeField]
    private bool HandleBackButton;

    public bool AutomaticSetupBack = true;

    private bool _lockDaily = true;

    // public bool LockProgressViewer { set { _lockProgress = value; } get { return _lockProgress; } }
    public bool LockDaily { set { DailyLocker.Lock = value; } get { return DailyLocker.Lock; } }



    public bool ActiveBack
    {
        set
        {
            if (value) BackScreen.Show();
            else BackScreen.Hide();
        }
        get { return BackScreen.IsShow(); }
    }
    public bool ActiveViewerCoins
    {
        set
        {
            if (value) Coins.Show();
            else Coins.Hide();
        }
        get { return Coins.IsShow(); }
    }

    public bool ActiveViewerProgress
    {
        set
        {
            ProgressViewer.IsAppearance = value;
        }
        get { return ProgressViewer.IsAppearance; }
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        if (TopBar != null)
        {
            Destroy(this);
            return;
        }
        TopBar = this;
        //Initialize logic
        if (BackScreen == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "BackScreen", name);
            enabled = false;
            return;
        }
        if (Coins == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Coins", name);
            enabled = false;
            return;
        }
        if (ProgressViewer == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ProgressViewer", name);
            enabled = false;
            return;
        }
        if (DailyLocker == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DailyLocker", name);
            enabled = false;
            return;
        }

        WorldEther.ChangeScreen.Subscribe(HandlerChangeScreen);

    }
    public void SetAutomaticSetupBack(bool value)
    {
        AutomaticSetupBack = value;
    }
    public void SetActiveBack(bool value)
    {
        ActiveBack = value;
    }
    void Start()
    {
        Initialize();
    }
    public void SetViewerCoin(bool value)
    {
        ActiveViewerCoins = value;
    }
    public void SetProgressViewer(bool value)
    {
        ActiveViewerProgress = value;
    }
    public void UpdateStateBack()
    {
        ActiveBack = UIController.Controller.CountScreens > 1;
    }

    private void HandlerChangeScreen(Ethers.Channel.Info info)
    {
        if (AutomaticSetupBack)
            UpdateStateBack();
    }
    private bool _pressBackFlag;
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (HandleBackButton)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                _pressBackFlag = false;
            }
            else if (!_pressBackFlag && Input.GetKeyDown(KeyCode.Escape))
            {
                _pressBackFlag = true;
                if (ActiveBack)
                    UIOrganization.UIController.BackScreen();
            }

        }
    }
}
