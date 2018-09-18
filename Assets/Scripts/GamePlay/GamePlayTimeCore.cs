using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class GamePlayTimeCore : GamePlaySingleCore
{
    public const string KEY_TIMECOREBEST = "BTIME";
    private float restTime;
    public float PlayTime = 60;
    public bool TimeDown = false;
    [SerializeField] UIOrganization.ScalePosition ScalePosition;
    [SerializeField] TextMesh TimeText;
    [SerializeField]
    Appearance TextAppearnce;

    public float ScopeTime { get { return restTime / PlayTime; } }

    private int _prevSeconds, _seconds;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (TimeDown)
        {
            restTime -= Time.deltaTime;
            _seconds = Mathf.RoundToInt(restTime);
            if (_seconds != _prevSeconds)
            {
                _prevSeconds = _seconds;
                TimeText.text = _seconds.ToString();
            }
            if (restTime <= 0)
            {
                restTime = 0;
                WorldEther.LoseGame.Push(this, null);
            }
            UpdateDefuse();
        }
    }

    public override void Dispose()
    {
        if (ObjectGame != null)
        {
            ObjectGame.EventSwipe -= HandlerSwipe;
        }
        base.Dispose();
    }

    private bool FirstSwipe;

    protected override void UpdateObjectGame()
    {
        base.UpdateObjectGame();
        FirstSwipe = false;
        ObjectGame.GetDefuseManager().enabled = false;
        ObjectGame.EventSwipe += HandlerSwipe;
    }
    private void HandlerSwipe()
    {
        FirstSwipe = true;
        TimeDown = true;
    }
    public override int GetBonus(int scores)
    {
        return (scores * scores) / 1000;
    }
    protected override string GetKeyBestPostprefix()
    {
        return KEY_TIMECOREBEST;
    }
    public override void OnPause()
    {
        ObjectGame.OnPause.Invoke();
        UIOrganization.UIController.ShowScreen(UIContenier.Contenier.GetPauseScreen());
        IsPlay = false;
        TextAppearnce.Hide();
        TimeDown = false;

    }
    public override void OnResume()
    {
        ObjectGame.gameObject.SetActive(true);
        ObjectGame.OnPlay.Invoke();
        IsPlay = true;
        TimeDown = FirstSwipe;
        TextAppearnce.Show();
    }
    public override void OnRestart()
    {
        restTime = PlayTime;
        TimeText.text = PlayTime.ToString();
        base.OnRestart();
        Continuable = false;
        UpdateDefuse();
    }

    public void UpdateDefuse()
    {
        if (ObjectGame != null)
            ObjectGame.GetDefuseManager().ScopeDefuse = ScopeTime;
    }

    protected override void ListnerLoseGame(Ethers.Channel.Info info)
    {
        base.ListnerLoseGame(info);
        TimeDown = false;
        TextAppearnce.Hide();
    }

    public override void Initialize()
    {
        base.Initialize();
        if (TimeText == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TextTime", name);
            enabled = false;
            return;
        }
        if (ScalePosition == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ScalePosition", name);
            enabled = false;
            return;
        }
        Continuable = false;
        ScalePosition.Camera = UIContenier.Contenier.GetMainCamera();
        ScalePosition.Initialize();
    }
}