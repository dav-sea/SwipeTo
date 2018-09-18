using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class AdsManager : MonoBehaviour, INonSkippableVideoAdListener
{
    public static AdsManager Manager { private set; get; }

    [SerializeField] private UIOrganization.Screen AdsScreen;

    public const string APP_KEY = "f8d3c2fd744fd5a480c1d6297e6dae77c3a75e86accfc10f";

    public void InitializeAppodeal()
    {
        if (_initappodeal) return;
        _initappodeal = true;
        Appodeal.disableLocationPermissionCheck();
        Appodeal.setAutoCache(Appodeal.NON_SKIPPABLE_VIDEO, false);
        // Appodeal.cache
        Appodeal.initialize(APP_KEY, Appodeal.NON_SKIPPABLE_VIDEO);
        // Appodeal.setAutoCache(Appodeal.NON_SKIPPABLE_VIDEO, false);
        // string appKey = "805cfe3539ed1ab8fc4624ecaef3a73e8f332745679144ac";
        Appodeal.setNonSkippableVideoCallbacks(this);
        // Appodeal.initialize(appKey, Appodeal.NON_SKIPPABLE_VIDEO);
        // Appodeal.
    }
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Appodeal.onResume();
        }
    }

    DeferredAction.OnceAction DeferredHide;

    public bool ShowNonSkipable(System.Action FinishAction)
    {
        // if (_NonSkippableVideoIsShow) return false;

        UIOrganization.UIController.ShowScreen(AdsScreen);

        FinishInterstitialAction = FinishAction;

        if (!Appodeal.isLoaded(Appodeal.NON_SKIPPABLE_VIDEO))
        {
            Appodeal.cache(Appodeal.NON_SKIPPABLE_VIDEO);
            // Appodeal.
            if (DeferredHide != null) DeferredHide.Cancel();
            DeferredHide = new DeferredAction.OnceAction(delegate
            {
                if (UIOrganization.UIController.Controller.ActiveScreen == AdsScreen)
                {
                    UIOrganization.UIController.BackScreen();
                    MessageManager.ShowMessage("Не удалось загрузить видео =(", 5f);
                }
            }, 60);
            DeferredAction.Manager.AddDeferredAction(DeferredHide);
            return true;
        }
        // Appodeal.ca
        // Appodeal.show()
        Appodeal.show(Appodeal.NON_SKIPPABLE_VIDEO);

        return true;
    }

    public bool ShowNonSkipableIsLoad()
    {
        return Appodeal.isLoaded(Appodeal.NON_SKIPPABLE_VIDEO);
    }

    public void onNonSkippableVideoLoaded()
    {
        if (DeferredHide != null) DeferredHide.Cancel();
        if (AdsScreen.IsAppearance)
            Appodeal.show(Appodeal.NON_SKIPPABLE_VIDEO);
    }
    public void onNonSkippableVideoFailedToLoad()
    {
        Debug.Log("Failed"); if (UIOrganization.UIController.Controller.ActiveScreen == AdsScreen)
            UIOrganization.UIController.BackScreen();
    }
    public void onNonSkippableVideoShown() { _NonSkippableVideoIsShow = true; }
    public void onNonSkippableVideoClosed(bool finihsed)
    {
        if (UIOrganization.UIController.Controller.ActiveScreen == AdsScreen)
            UIOrganization.UIController.BackScreen();

        if (FinishInterstitialAction != null)
            FinishInterstitialAction();

        _NonSkippableVideoIsShow = false;
    }
    public void onNonSkippableVideoFinished()
    {

    }

    System.Action FinishInterstitialAction;
    private bool _NonSkippableVideoIsShow;
    private bool _initialized, _initappodeal;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Manager != null) { Destroy(this); return; }
        Manager = this;
        if (AdsScreen == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "AdsScreen", name);
            enabled = false;
            return;
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        InitializeAppodeal();

    }

    void Awake()
    {
        Initialize();
    }
}