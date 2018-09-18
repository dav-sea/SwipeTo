using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualLose : ActionComponent
{
    public const int LoseID = 4;
    private static int _TrainingCount;
    public override int TrainingCount { set { _TrainingCount = value; } get { return _TrainingCount; } }
    public override string TranslationTraining
    {
        get { return ""; }
    }
    public override int ID { get { return LoseID; } }
    public override bool IsRotater()
    {
        return false;
    }
    public override bool IsSwipeble()
    {
        return false;
    }
    protected override bool OnSwipe(Side.Diraction diraction)
    {
        return false;
    }
    protected override void InitializeTraining()
    {
        Training.CountParticles = 0;
    }
    public override bool TrainingActive
    {
        get { return false; }
        set { }
    }
    public override bool TrainingComplete
    {
        get { return true; }
        set { }
    }
    // protected override bool OnInitialize()
    // {
    //     base.OnInitialize();
    //     return true;
    // }

    // public override void ResetAction()
    // {
    //     base.ResetAction();
    // }
}