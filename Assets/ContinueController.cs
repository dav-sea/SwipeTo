using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueController : MonoBehaviour
{

    [SerializeField] UIOrganization.TouchComponent TouchComponent;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TouchComponent == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TouchComponent", name);
            enabled = false;
            return;
        }
        TouchComponent.EventClick += OnClick;
    }
    void Awake()
    {
        Initialize();
    }

    private void OnClick()
    {
        AdsManager.Manager.ShowNonSkipable(FinishAction);
    }

    private void FinishAction()
    {
        DeferredAction.Manager.AddDeferredAction(delegate
        {
            GamePlayContenier.CoreContinue();
            UIOrganization.UIController.ShowScreen(UIContenier.Contenier.GetGameScreen());
            GamePlayContenier.ResumeGamePlay(this);
        }, 0.67f);
    }
}
