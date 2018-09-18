using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ManualQuestionRotateble : ManualKit
{
    public override int TrainingCount { set { ManualQuestionRandom._TrainingCount = value; } get { return ManualQuestionRandom._TrainingCount; } }
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
    public const int QuestionRotatebleID = 11;
    public override int ID { get { return QuestionRotatebleID; } }
    public override bool IsRotater() { return true; }
    public override bool IsSwipeble() { return true; }

    public override void Hide()
    {
        Appearance.Hide();
    }

    protected override bool OnSwipe(Side.Diraction diraction)
    {
        if (Diraction == diraction)
        {
            KitManual();
            PlaySoundEffect();
            ManualQuestionRandom.QuestionAction(this, GetRandomRotatble(Diraction.Parent.GetObjectGame().GetActionManager()));
            return true;
        }
        return false;
    }
    protected virtual void PlaySoundEffect()
    {
        AudioContainer.Manager.DefaultQuestionSound.Play();
    }

    protected ActionComponent GetRandomRotatble(ActionManager manager)
    {
        return manager.GetRandomRoteter(manager.GetData());
    }
}