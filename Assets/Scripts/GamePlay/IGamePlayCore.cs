using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public interface IGamePlayCore
{
    GamePlayData GetData();

    bool Continuable { get; }

    int BestResult();
    void SetBestResult(int best);

    void WaitFirstAction();

    int GetBonus(int scores);

    void Initialize();
    void OnSelect();
    void OnRestart();
    void OnPause();
    void OnResume();
    void OnUnselect();
    bool ContinueCore();
    void Dispose();
}

public abstract class GamePlayCore : UnityEngine.MonoBehaviour, IGamePlayCore
{
    public bool Continuable { protected set; get; }
    public const string KEY_BEST = "BEST";
    // public abstract void NoticeLoseStream(IGamePlayStream stream);
    // public abstract void Restart();
    public abstract void Initialize();

    public abstract GamePlayData GetData();

    protected abstract string GetKeyBestPostprefix();

    public abstract void Dispose();

    public abstract void WaitFirstAction();

    public virtual int GetBonus(int scores)
    {
        return scores / 100;
    }

    public int BestResult()
    {
        return PlayerPrefs.GetInt(KEY_BEST + GetKeyBestPostprefix(), 0);
    }
    public void SetBestResult(int best)
    {
        PlayerPrefs.SetInt(KEY_BEST + GetKeyBestPostprefix(), best);
    }

    protected virtual void OnContinue() { }

    public virtual bool ContinueCore()
    {
        if (Continuable) OnContinue();
        return Continuable;
    }
    public virtual void OnSelect() { }
    public virtual void OnRestart() { }
    public virtual void OnPause() { }
    public virtual void OnResume() { }
    public virtual void OnUnselect() { }
}