using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualLife : ManualStand
{
    public const int LifeID = 3;
    private static int _TrainingCount;
    public override int TrainingCount { set { _TrainingCount = value; } get { return _TrainingCount; } }
    public override string TranslationTraining
    {
        get { return "T_Life"; }
    }
    public override int ID { get { return LifeID; } }
    protected override bool OnSwipe(Side.Diraction diraction)
    {
        if (base.OnSwipe(diraction))
        {
            if (GamePlayContenier.GamePlayCore != null)
                Lifes.LifesManager.CountLifes++;
            return true;
        }
        return false;
    }
    public override GameObject GetPrefabObject()
    {
        return PrefabsHelper.PrefabLifeObject;
    }
}