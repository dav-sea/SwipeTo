using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualArrow : ManualKit
{
    public const int ArrowID = 1;
    private static int _TrainingCount;
    public override int TrainingCount { set { _TrainingCount = value; } get { return _TrainingCount; } }

    public override string TranslationTraining
    {
        get { return "T_Arrow"; }
    }

    public override bool IsRotater()
    {
        return true;
    }
    public override bool IsSwipeble()
    {
        return true;
    }

    public override int ID { get { return ArrowID; } }

    protected override bool OnSwipe(Side.Diraction diraction)
    {
        if (diraction == Diraction)
        {
            KitManual();
            ArrowManual(diraction);
        }
        else return false;

        return true;
    }

    protected void ArrowManual(Side.Diraction diraction)
    {
        diraction.GetRotateFunction().Invoke(90);
        diraction.Parent.GetObjectGame().SetupFrontSide();
        PlaySoundEffect();
    }

    protected virtual void PlaySoundEffect()
    {
        AudioContainer.Manager.DefaultArrowSound.Play();
    }

    // protected override bool OnInitialize()
    // {
    //     if (!base.OnInitialize()) return false;
    //     return true;
    // }

    // public override void ResetAction()
    // {
    //     base.ResetAction();
    //     Animation.Stop();
    //     Animation.Set(null);
    // }
}