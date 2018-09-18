using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ManualFreezeDefuse : ManualStand
{
    public static float UpFreezeTime = 5;
    private static int _TrainingCount;
    public override int TrainingCount { set { _TrainingCount = value; } get { return _TrainingCount; } }

    public override string TranslationTraining
    {
        get { return "T_Freezer"; }
    }
    public const int FreezeID = 8;
    public override int ID { get { return FreezeID; } }
    // private FreezeObject FreezeObject;
    protected override bool OnSwipe(Side.Diraction diraction)
    {
        if (base.OnSwipe(diraction))
        {
            if (diraction.Parent.GetObjectGame().IsDemo()) return true;
            diraction.Parent.GetObjectGame().GetDefuseManager().ActiveDefuse = false;
            FreezeHelper.FreezeTime += UpFreezeTime;
            return true;
        }
        return false;
    }
    // protected override Appearance CreateAppearanceObject()
    // {
    //     var result = base.CreateAppearanceObject();
    //     FreezeObject = result.gameObject.GetComponent<FreezeObject>();
    //     return result;
    // }
    public override GameObject GetPrefabObject()
    {
        return PrefabsHelper.PrefabFreezeObject;
    }
}