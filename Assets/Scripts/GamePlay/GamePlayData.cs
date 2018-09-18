using UnityEngine;

[System.Serializable]
public class GamePlayData : IParameter
{
    [SerializeField]
    public Parameter ChanceSimpleArrow;
    [SerializeField]
    public Parameter ChanceMultiSwipes;
    [SerializeField]
    public Parameter ChanceLose;
    [SerializeField]
    public Parameter ChanceEmpty;
    [SerializeField]
    public Parameter ChanceTransfer;
    [SerializeField]
    public Parameter ChanceMultiplier;
    [SerializeField]
    public Parameter ChanceLife;
    [SerializeField]
    public Parameter ChanceCoin;
    [SerializeField]
    public Parameter ChanceFreeze;
    [SerializeField]
    public Parameter ChanceFuller;
    [SerializeField]
    public Parameter ChanceQuestionRandom;
    [SerializeField]
    public Parameter ChanceQuestionRotateble;

    public void ForceInitialize()
    {
        ChanceSimpleArrow = new Parameter();
        ChanceMultiSwipes = new Parameter();
        ChanceLose = new Parameter();
        ChanceEmpty = new Parameter();
        ChanceTransfer = new Parameter();
        ChanceLife = new Parameter();
        ChanceMultiplier = new Parameter();
        ChanceCoin = new Parameter();
        SwipesRandomCount = new RandomValueParameter();
        ChanceFreeze = new Parameter();
        ChanceFuller = new Parameter();
        ChanceQuestionRandom = new Parameter();
        ChanceQuestionRotateble = new Parameter();
    }

    public RandomValueParameter SwipesRandomCount;

    public float SummRotaters
    {
        get { return ChanceSimpleArrow.Value + ChanceQuestionRotateble.Value + ChanceMultiSwipes.Value + ChanceTransfer.Value; }
    }

    public float SummSwipebles
    {
        get { return SummRotaters + ChanceQuestionRandom.Value + ChanceFuller.Value + ChanceFreeze.Value + ChanceCoin.Value + ChanceLife.Value + ChanceMultiplier.Value; }
    }

    public float SummChances
    {
        get { return SummSwipebles + ChanceLose.Value + ChanceEmpty.Value; }
    }


    public float CoffChances
    {
        get { return 1 / (SummChances != 0 ? SummChances : 1); }
    }

    public void Reset()
    {
        ChanceMultiSwipes.Reset();
        ChanceSimpleArrow.Reset();
        ChanceEmpty.Reset();
        ChanceLose.Reset();
        SwipesRandomCount.Reset();
        ChanceTransfer.Reset();
        ChanceMultiplier.Reset();
        ChanceLife.Reset();
        ChanceCoin.Reset();
        ChanceFreeze.Reset();
        ChanceFuller.Reset();
        ChanceQuestionRandom.Reset();
        ChanceQuestionRotateble.Reset();
    }
}

public interface IParameter
{
    void Reset();
}
[System.Serializable]
public class Parameter : IParameter
{
    [SerializeField]
    private float DefaultValue;

    private float _CustomValue;
    public bool IsCustom { private set; get; }

    public float Value
    {
        set { _CustomValue = value; IsCustom = value != DefaultValue; }
        get { return IsCustom ? _CustomValue : DefaultValue; }
    }

    public void Reset()
    {
        Value = DefaultValue;
    }
}
[System.Serializable]
public class RandomValueParameter : IParameter
{
    [SerializeField]
    public Parameter DefaultMinValue = new Parameter();
    [SerializeField]
    public Parameter DefaultMaxValue = new Parameter();

    public float Value { get { return Random.Range(DefaultMinValue.Value, DefaultMaxValue.Value); } }

    public void Reset()
    {
        DefaultMaxValue.Reset();
        DefaultMinValue.Reset();
    }
}