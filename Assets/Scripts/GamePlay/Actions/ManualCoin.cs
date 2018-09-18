using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ManualCoin : ManualStand
{
    private static int _TrainingCount;
    public override int TrainingCount { set { _TrainingCount = value; } get { return _TrainingCount; } }
    public override string TranslationTraining
    {
        get { return "T_Coin"; }
    }
    public static int CountUpCoins = 1;
    public const int CoinID = 2;
    public override int ID { get { return CoinID; } }
    protected override bool OnSwipe(Side.Diraction diraction)
    {
        if (base.OnSwipe(diraction))
        {
            if (GamePlayContenier.GamePlayCore != null)
                Coins.Manager.CoinsCount += CountUpCoins;
            return true;
        }
        return false;
    }
    public override GameObject GetPrefabObject()
    {
        return PrefabsHelper.PrefabCoinObject;
    }
}