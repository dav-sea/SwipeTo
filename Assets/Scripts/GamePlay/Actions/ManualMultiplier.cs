using UnityEngine;

public class ManualMultiplier : ManualStand
{
    private static int _TrainingCount;
    public override int TrainingCount { set { _TrainingCount = value; } get { return _TrainingCount; } }
    public override string TranslationTraining
    {
        get { return "T_Multiplier"; }
    }
    public const int MultiplierID = 5;
    public override int ID { get { return MultiplierID; } }
    protected override bool OnSwipe(Side.Diraction diraction)
    {
        if (base.OnSwipe(diraction))
        {
            if (GamePlayContenier.GamePlayCore != null)
                Score.ScoreManager.Multiplier *= 2;
            return true;
        }
        return false;
    }
    public override GameObject GetPrefabObject()
    {
        return PrefabsHelper.PrefabMultiplierObject;
    }
}