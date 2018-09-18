using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ActionManager : MonoBehaviour
{
    // private ActionsClaster Claster;
    [SerializeField]
    private ActionPrefabsContenier Actions;

    public bool UseBalanceGamePlayData;

    private SignProgressLocker _cacheLocker;
    private SignProgressLocker Locker
    {
        get
        {
            if (_cacheLocker == null) _cacheLocker = SignProgressLocker.Manager;
            return _cacheLocker;
        }
    }

    private ActionsFabricator Fabricator;

    public bool ActivateLevelToLock = true;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        SyncActionsContenier();
    }
    [System.Obsolete]
    public void Fill(int count)
    {
        Fabricator.FillTargetActions(count);
    }
    public void SyncActionsContenier()
    {
        if (Fabricator == null) Fabricator = new ActionsFabricator();
        Fabricator.ManualArrow.SetPrefab(Actions.ArrowPrefab);
        Fabricator.ManualSwipes.SetPrefab(Actions.SwipesPrefab);
        Fabricator.ManualLose.SetPrefab(Actions.LosePrefab);
        Fabricator.ManualTransfer.SetPrefab(Actions.TransferPrefab);
        Fabricator.ManualMultiplier.SetPrefab(Actions.MultiplierPrefab);
        Fabricator.ManualLife.SetPrefab(Actions.LifePrefab);
        Fabricator.ManualCoin.SetPrefab(Actions.CoinPrefab);
        Fabricator.ManualFreeze.SetPrefab(Actions.FreezePrefab);
        Fabricator.ManualFuller.SetPrefab(Actions.FullerPrefab);
        Fabricator.ManualQuestionRandom.SetPrefab(Actions.QuestionRandomPrefab);
        Fabricator.ManualQuestionRotateble.SetPrefab(Actions.QuestionRotateblePrefab);
    }
    void Awake()
    {
        Initialize();
    }

    private GamePlayData _balanceData;
    private GamePlayData balanceData
    {
        get
        {
            if (_balanceData == null)
            {
                _balanceData = new GamePlayData();

                _balanceData.ForceInitialize();

                _balanceData.ChanceCoin.Value = _balanceData.ChanceEmpty.Value = _balanceData.ChanceLife.Value = _balanceData.ChanceLose.Value =
                    _balanceData.ChanceMultiplier.Value = _balanceData.ChanceMultiSwipes.Value = _balanceData.ChanceSimpleArrow.Value =
                    _balanceData.ChanceTransfer.Value = _balanceData.ChanceFreeze.Value = _balanceData.ChanceFuller.Value = _balanceData.ChanceQuestionRandom.Value =
                    _balanceData.ChanceQuestionRotateble.Value = 1;

                _balanceData.SwipesRandomCount.DefaultMinValue.Value = _balanceData.SwipesRandomCount.DefaultMaxValue.Value = 2;
            }
            return _balanceData;
        }
    }
    public GamePlayData GetData()
    {
        // Debug.Log("" + GamePlayContenier.GamePlayCore.GetData().ChanceMultiplier.Value);
        // Debug.Log("" + GamePlayContenier.GamePlayCore.GetData().ChanceSimpleArrow.Value);
        return UseBalanceGamePlayData ? balanceData : GamePlayContenier.GamePlayCore.GetData();
    }

    public ActionComponent[] GetRandomGroupActions(byte count, GamePlayData data)
    {

        ActionComponent[] result = new ActionComponent[count];
        if (count > 0)
        {
            //1. Гарантируем, что в массиве будет содержаться хотя бы 1 Rotater
            result[0] = GetRandomRoteter(data);

            //Заполняем массив
            for (byte i = 1; i < count; ++i)
                result[i] = GetRandomActionOrNull(data);
        }

        return result;
    }

    public ActionComponent GetRandomActionOrNull(GamePlayData data)
    {
        float coff = 1 / data.SummChances;
        // float left = 0;
        float random = Random.value;

        if (random >= 0 && random < data.ChanceEmpty.Value * coff)
            return null;
        return GetRandomAction(data);
    }
    public ActionComponent GetRandomAction(GamePlayData data)
    {
        float coff = 1 / (data.SummChances - data.ChanceEmpty.Value);
        // float left = 0;
        float random = Random.value;

        if (random >= 0 && random < data.ChanceLose.Value * coff && !SignProgressLocker.Manager.LoseLock)
            return Fabricator.ManualLose.GetAction();
        return GetRandomSwipeble(data);
    }

    public ActionComponent GetRandomSwipeble(GamePlayData data)
    {
        float coff = 1 / data.SummSwipebles;
        float left = 0;
        float random = Random.value;

        if (TryChance(ref left, random, data.ChanceLife.Value * coff, Locker.LifesLock))
            return Fabricator.ManualLife.GetAction();
        if (TryChance(ref left, random, data.ChanceTransfer.Value * coff, Locker.TransferLock))
            return Fabricator.ManualTransfer.GetAction();
        if (TryChance(ref left, random, data.ChanceMultiSwipes.Value * coff, Locker.SwipesLock))
            return Fabricator.ManualSwipes.GetAction();
        if (TryChance(ref left, random, data.ChanceQuestionRotateble.Value * coff, Locker.QuestionRotatebleLock))
            return Fabricator.ManualQuestionRotateble.GetAction();
        // if (TryChance(ref left, random, data.SummRotaters * coff))
        //     return GetRandomRoteter(data);

        if (TryChance(ref left, random, data.ChanceCoin.Value * coff, Locker.CoinsLock))
            return Fabricator.ManualCoin.GetAction();
        if (TryChance(ref left, random, data.ChanceMultiplier.Value * coff, Locker.MultiplierLock))
            return Fabricator.ManualMultiplier.GetAction();
        if (TryChance(ref left, random, data.ChanceFreeze.Value * coff, Locker.FreezeLock))
            return Fabricator.ManualFreeze.GetAction();
        if (TryChance(ref left, random, data.ChanceFuller.Value * coff, Locker.FullerLock))
            return Fabricator.ManualFuller.GetAction();
        if (TryChance(ref left, random, data.ChanceQuestionRandom.Value * coff, Locker.QuestionLock))
            return Fabricator.ManualQuestionRandom.GetAction();

        return Fabricator.ManualArrow.GetAction();
    }

    public ActionComponent GetRandomRoteter(GamePlayData data)
    {
        float coff = 1 / data.SummRotaters;
        float left = 0;
        float random = Random.value;

        if (TryChance(ref left, random, data.ChanceTransfer.Value * coff, Locker.TransferLock))
            return Fabricator.ManualTransfer.GetAction();
        if (TryChance(ref left, random, data.ChanceMultiSwipes.Value * coff, Locker.SwipesLock))
            return Fabricator.ManualSwipes.GetAction();
        if (TryChance(ref left, random, data.ChanceQuestionRotateble.Value * coff, Locker.QuestionRotatebleLock))
            return Fabricator.ManualQuestionRotateble.GetAction();
        return Fabricator.ManualArrow.GetAction();
    }

    public ActionComponent GetArrow()
    {
        return Fabricator.ManualArrow.GetAction();
    }
    public ActionComponent GetTransfer()
    {
        return Fabricator.ManualTransfer.GetAction();
    }
    public ActionComponent GetSwipes()
    {
        return Fabricator.ManualSwipes.GetAction();
    }
    public ActionComponent GetCoin()
    {
        return Fabricator.ManualCoin.GetAction();
    }


    private bool TryChance(ref float left, float random, float chace, bool isLock = false)
    {
        left += chace;
        return ActivateLevelToLock ? (!isLock && random >= left - chace && random < left) : (random >= left - chace && random < left);
    }

    public void Recovery(ActionComponent action)
    {
        Fabricator.RecoverAction(action);
    }


    public class ActionsFabricator
    {
        public LeafFabricator<ManualArrow> ManualArrow = new LeafFabricator<global::ManualArrow>();
        public LeafFabricator<ManualMultiSwipes> ManualSwipes = new LeafFabricator<global::ManualMultiSwipes>();
        public LeafFabricator<ManualLose> ManualLose = new LeafFabricator<global::ManualLose>();
        public LeafFabricator<ManualTransfer> ManualTransfer = new LeafFabricator<global::ManualTransfer>();
        public LeafFabricator<ManualMultiplier> ManualMultiplier = new LeafFabricator<global::ManualMultiplier>();
        public LeafFabricator<ManualLife> ManualLife = new LeafFabricator<global::ManualLife>();
        public LeafFabricator<ManualCoin> ManualCoin = new LeafFabricator<global::ManualCoin>();
        public LeafFabricator<ManualFreezeDefuse> ManualFreeze = new LeafFabricator<global::ManualFreezeDefuse>();
        public LeafFabricator<ManualFuller> ManualFuller = new LeafFabricator<global::ManualFuller>();
        public LeafFabricator<ManualQuestionRandom> ManualQuestionRandom = new LeafFabricator<global::ManualQuestionRandom>();
        public LeafFabricator<ManualQuestionRotateble> ManualQuestionRotateble = new LeafFabricator<global::ManualQuestionRotateble>();


        public void RecoverAction(ActionComponent action)
        {
            if (action == null) return;

            switch (action.ID)
            {
                case global::ManualArrow.ArrowID:
                    ManualArrow.AddAction(action as ManualArrow);
                    break;
                case global::ManualTransfer.TransferID:
                    ManualTransfer.AddAction(action as ManualTransfer);
                    break;
                case global::ManualMultiSwipes.SwipesID:
                    ManualSwipes.AddAction(action as ManualMultiSwipes);
                    break;
                case global::ManualLose.LoseID:
                    ManualLose.AddAction(action as ManualLose);
                    break;
                case global::ManualMultiplier.MultiplierID:
                    ManualMultiplier.AddAction(action as ManualMultiplier);
                    break;
                case global::ManualQuestionRandom.QuestionID:
                    ManualQuestionRandom.AddAction(action as ManualQuestionRandom);
                    break;
                case global::ManualQuestionRotateble.QuestionRotatebleID:
                    ManualQuestionRotateble.AddAction(action as ManualQuestionRotateble);
                    break;
                case global::ManualCoin.CoinID:
                    ManualCoin.AddAction(action as ManualCoin);
                    break;
                case global::ManualLife.LifeID:
                    ManualLife.AddAction(action as ManualLife);
                    break;
                case global::ManualFuller.FullerID:
                    ManualFuller.AddAction(action as ManualFuller);
                    break;
                case global::ManualFreezeDefuse.FreezeID:
                    ManualFreeze.AddAction(action as ManualFreezeDefuse);
                    break;

                default:
                    Debug.LogWarning("Not normal behaviour!");

                    if (!action.IsSwipeble() && action is ManualLose) ManualLose.AddAction((ManualLose)action);
                    else if (action.IsRotater())
                    {
                        if (action is ManualMultiSwipes) ManualSwipes.AddAction((ManualMultiSwipes)action);
                        else if (action is ManualTransfer) ManualTransfer.AddAction((ManualTransfer)action);
                        else if (action is ManualArrow) ManualArrow.AddAction((ManualArrow)action);
                        else if (action is ManualQuestionRotateble) ManualQuestionRotateble.AddAction((ManualQuestionRotateble)action);
                        else Debug.Log("Recovery was failed! type: " + action.GetType().ToString());
                    }
                    else
                    {
                        if (action is ManualCoin) ManualCoin.AddAction((ManualCoin)action);
                        else if (action is ManualLife) ManualLife.AddAction((ManualLife)action);
                        else if (action is ManualMultiplier) ManualMultiplier.AddAction((ManualMultiplier)action);
                        else if (action is ManualFreezeDefuse) ManualFreeze.AddAction((ManualFreezeDefuse)action);
                        else if (action is ManualQuestionRandom) ManualQuestionRandom.AddAction((ManualQuestionRandom)action);
                        else if (action is ManualFuller) ManualFuller.AddAction((ManualFuller)action);
                        else Debug.Log("Recovery was failed! type: " + action.GetType().ToString());
                    }
                    break;
            }


        }

        public void FillTargetActions(int count)
        {
            ManualArrow.TargetCount(count);
            ManualSwipes.TargetCount(count);
            ManualLose.TargetCount(count);
            ManualTransfer.TargetCount(count);
            ManualMultiplier.TargetCount(count);
            ManualLife.TargetCount(count);
            ManualCoin.TargetCount(count);
            ManualFreeze.TargetCount(count);
            ManualFuller.TargetCount(count);
            ManualQuestionRandom.TargetCount(count);
            ManualQuestionRotateble.TargetCount(count);
        }
    }

    [System.Serializable]
    public class ActionPrefabsContenier
    {
        [SerializeField] private GameObject _Arrow;
        public GameObject ArrowPrefab { get { return _Arrow; } }
        [SerializeField] private GameObject _Swipes;
        public GameObject SwipesPrefab { get { return _Swipes; } }
        [SerializeField] private GameObject _Lose;
        public GameObject LosePrefab { get { return _Lose; } }
        [SerializeField] private GameObject _Transfer;
        public GameObject TransferPrefab { get { return _Transfer; } }
        [SerializeField] private GameObject _Multiplier;
        public GameObject MultiplierPrefab { get { return _Multiplier; } }
        [SerializeField] private GameObject _Life;
        public GameObject LifePrefab { get { return _Life; } }
        [SerializeField] private GameObject _Coin;
        public GameObject CoinPrefab { get { return _Coin; } }
        [SerializeField] private GameObject _Freezer;
        public GameObject FreezePrefab { get { return _Freezer; } }
        [SerializeField] private GameObject _Fuller;
        public GameObject FullerPrefab { get { return _Fuller; } }
        [SerializeField] private GameObject _QuestionRandom;
        public GameObject QuestionRandomPrefab { get { return _QuestionRandom; } }
        [SerializeField] private GameObject _QuestionRotateble;
        public GameObject QuestionRotateblePrefab { get { return _QuestionRotateble; } }

    }

    public class LeafFabricator<ActionType> where ActionType : ActionComponent
    {
        ComponentObjectPool<ActionType> Pool;

        public int Count { get { return Pool.Count; } }

        private void CreatePool(GameObject prefab)
        {
            if (Pool != null) Pool.Target(0);

            Pool = new ComponentObjectPool<ActionType>(prefab, 0);
            Pool.OnCreate += delegate (ActionType comp) { comp.Initialize(); };
            Pool.OnPop += delegate (ActionType comp)
            {
                comp.ResetAction();
                comp.GameObject.SetActive(true);
                comp.IsActiveAction = true;
            };
        }

        public void SetPrefab(GameObject prefab)
        {
            CreatePool(prefab);
        }

        public void TargetCount(int target)
        {
            if (Pool != null)
                Pool.Target(target);
        }

        public ActionType GetAction()
        {
            if (Pool == null) return null;
            return Pool.Pop();
        }

        public void AddAction(ActionType action)
        {
            if (Pool != null)
                Pool.Push(action);
        }

        public LeafFabricator() { }

        public bool IsSwipebleAction()
        {
            return (Pool.GetPrefab() as GameObject).GetComponent<ActionComponent>().IsSwipeble();
        }

        public bool IsRotatebleAction()
        {
            return (Pool.GetPrefab() as GameObject).GetComponent<ActionComponent>().IsRotater();
        }

        public LeafFabricator(GameObject prefab)
        {
            SetPrefab(prefab);
        }
    }

}