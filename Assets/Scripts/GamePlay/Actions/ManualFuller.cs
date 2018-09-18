using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ManualFuller : ManualStand
{
    public const int FullerID = 9;
    private static int _TrainingCount;
    public override int TrainingCount { set { _TrainingCount = value; } get { return _TrainingCount; } }
    public override int ID { get { return FullerID; } }
    public override string TranslationTraining
    {
        get { return "T_Fuller"; }
    }

    protected override bool OnSwipe(Side.Diraction diraction)
    {
        if (base.OnSwipe(diraction))
        {
            diraction.Parent.GetObjectGame().GetDefuseManager().UndefuseMax();
            return true;
        }
        return false;
    }
    public override GameObject GetPrefabObject()
    {
        return PrefabsHelper.PrefabFullerObject;
    }
}