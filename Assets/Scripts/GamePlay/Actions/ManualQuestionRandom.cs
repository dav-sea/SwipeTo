using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ManualQuestionRandom : ManualKit
{
    internal static int _TrainingCount;
    public override int TrainingCount { set { _TrainingCount = value; } get { return _TrainingCount; } }
    public override string TranslationTraining
    {
        get { return "T_Question"; }
    }
    public override bool TrainingComplete
    {
        set
        {
            PlayerPrefs.SetInt("1011tc", value ? 1 : 0);
        }
        get
        {
            return PlayerPrefs.GetInt("1011tc", 0) == 0 ? false : true;
        }
    }
    public const int QuestionID = 10;
    public override int ID { get { return QuestionID; } }
    public override bool IsRotater() { return false; }
    public override bool IsSwipeble() { return true; }

    protected override bool OnSwipe(Side.Diraction diraction)
    {
        if (Diraction == diraction)
        {
            KitManual();
            PlaySoundEffect();
            QuestionAction(this, GetRandomActionComponent(Diraction.Parent.GetObjectGame().GetActionManager()));
            return true;
        }
        return false;
    }

    protected ActionComponent GetRandomActionComponent(ActionManager manager)
    {
        return manager.GetRandomAction(manager.GetData());
    }

    protected virtual void PlaySoundEffect()
    {
        AudioContainer.Manager.DefaultQuestionSound.Play();
    }

    public override void Hide()
    {
        Appearance.Hide();
    }

    internal static void QuestionAction(ActionComponent manual, ActionComponent newComp)
    {
        var side = manual.Diraction.Parent;
        var objectGame = side.GetObjectGame();
        // newComp.ResetAction();
        // newComp.Hide();
        manual.IsActiveAction = false;
        manual.Hide();
        if (newComp != null)
        {
            side.AddActionComponent(newComp, manual.Diraction);
            newComp.Hide();
            newComp.Transform.localScale = Vector3.zero;
            newComp.Show();
        }
    }
}