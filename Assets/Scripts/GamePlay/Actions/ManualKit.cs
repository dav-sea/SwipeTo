using UnityEngine;

public abstract class ManualKit : ActionComponent
{
    #region PUBLIC FIELDS
    //====================================================================================//
    [HideInInspector] public int UpScoreValue = 1;
    [HideInInspector] public float AddTimeMultiplier = 0f;
    [HideInInspector] public float RollbackValue = 0.4f;
    //====================================================================================//
    #endregion

    #region  PUBLIC METHODS
    protected override bool OnSwipe(Side.Diraction diraction)
    {
        KitManual();
        return true;
    }
    protected void KitManual()
    {
        Undefuse();
        UpScore();
    }
    public void Undefuse()
    {
        Undefuse(RollbackValue);
    }
    public void Undefuse(float rollback)
    {
        if (Diraction != null)
        {
            Diraction.Parent.GetObjectGame().GetDefuseManager().Undefuse(rollback);
        }
    }
    //====================================================================================//
    public void UpScore(int value)
    {
        if (GamePlayContenier.GamePlayCore == null) return;
        Score.ScoreManager.AddScores(value);
        Score.ScoreManager.MultiplierTime += AddTimeMultiplier;
    }
    public void UpScore()
    {
        UpScore(UpScoreValue);
    }
    //====================================================================================//
    #endregion
}