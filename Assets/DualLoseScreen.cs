using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualLoseScreen : MonoBehaviour
{
    [SerializeField] Containier Top;
    [SerializeField] Containier Bottom;
    [SerializeField] UIOrganization.Screen Target;
    [SerializeField] UIOrganization.TouchComponent RestartButton;

    public UIOrganization.Screen GetScreen()
    {
        return Target;
    }

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
        if (RestartButton == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "RestartButton", name);
            enabled = false;
            return;
        }
        RestartButton.OnClick.AddListener(delegate
        {
            GamePlayContenier.ResetGameSystems();
            GamePlayContenier.Restart(null);
            GamePlayContenier.ResumeGamePlay(null);
            UIOrganization.UIController.ShowScreen(UIContenier.Contenier.GetGameScreen());
        });
    }
    void Awake()
    {
        Initialize();
    }

    public void TopWin()
    {
        Top.Win();
        Bottom.Lose();
    }

    public void BottomWin()
    {
        Top.Lose();
        Bottom.Win();
    }

    [System.Serializable]
    private class Containier
    {
        public UnityEngine.UI.Text WinText;
        public UnityEngine.UI.Text LoseText;

        public void Win()
        {
            WinText.enabled = true;
            LoseText.enabled = false;
        }
        public void Lose()
        {
            WinText.enabled = false;
            LoseText.enabled = true;
        }
    }
}
