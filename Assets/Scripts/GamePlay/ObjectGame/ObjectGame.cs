using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGame : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent OnLose;
    public UnityEngine.Events.UnityEvent OnShow;
    public UnityEngine.Events.UnityEvent OnHide;

    public UnityEngine.Events.UnityEvent OnFinishShow;
    public UnityEngine.Events.UnityEvent OnFinishHide;

    public UnityEngine.Events.UnityEvent OnPlay;
    public UnityEngine.Events.UnityEvent OnPause;
    public UnityEngine.Events.UnityEvent OnScoundChance;
    // public UnityEngine.Events.UnityEvent OnDemo;

    public event System.Action EventSwipe;

    [SerializeField]
    private TransformManager Transform;

    [SerializeField]
    private PaletteSelector TargetPalette;

    [SerializeField]
    private SidesPool SidesPool;

    [SerializeField]
    private ActionManager ActionManager;

    [SerializeField]
    private Transform SideSetupTarget;

    [SerializeField]
    private TouchAnimationController TouchAnimationController;

    [SerializeField]
    DefuseManager DefuseManager;

    [SerializeField]
    TargetFollowScript FollowScript;

    public bool SendMessageSwipeToGlobal = true;

    public bool IsDemo()
    {
        return !(SendLoseMessage || SendMessageSwipeToGlobal);
    }

    public void ActivateDemo()
    {
        DefuseManager.ActiveDefuse = false;
        SendLoseMessage = false;
        SendMessageSwipeToGlobal = false;
        // OnDemo.Invoke();
    }
    public void DisactivateDemo()
    {
        SendLoseMessage = true;
        SendMessageSwipeToGlobal = true;
    }

    public DefuseManager GetDefuseManager()
    {
        return DefuseManager;
    }

    public void Destroy()
    {
        // FrontSide.Hide();
        // FrontSide.Destroy();
        Destroy(this.gameObject, 0.4f);
    }

    public ActionManager GetActionManager()
    {
        return ActionManager;
    }

    public TouchAnimationController GetTouchAnimationController()
    {
        return TouchAnimationController;
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        // Debug.Log("Ininit");
        //Initialize logic
        if (Transform == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Transform", name);
            // return;
        }
        if (SidesPool == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "SidesPool", name);
            // return;
        }
        if (TouchAnimationController == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TouchAnimationController", name);
            // return;
        }
        if (DefuseManager == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefuseManager", name);
        }
        if (ActionManager == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ActionManager", name);
        }
        if (FollowScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "FollowScript", name);
            enabled = false;
            return;
        }
        Transform.Initialize();
        SidesPool.Initialize();
        TouchAnimationController.Initialize();
        DefuseManager.Initialize();
        ActionManager.Initialize();
        FollowScript.Initialize();
        FollowScript.enabled = false;
        FollowScript.DisableForFinish = true;
        FollowScript.Accelerate.AccelerateValue = 10;
        EventSwipe += delegate { if (SendMessageSwipeToGlobal) WorldEther.EventSwipe.Push(this, null); };
        DefuseManager.DefuseScore = DefuseManager.MaxValueDefuse;
        DefuseManager.ActiveDefuse = true;
    }

    public TargetFollowScript GetFollowScript()
    {
        return FollowScript;
    }

    public PaletteSelector GetPaletteSelector()
    {
        return TargetPalette;
    }

    public void SetupTargetPalette()
    {
        TargetPalette.Select();
    }

    void Awake()
    {
        Initialize();
    }

    public Side FrontSide { private set; get; }
    public Side Previous { private set; get; }

    public void SetFrontSide(Side side)
    {
        if (Previous != null) Previous.Hide();
        Previous = FrontSide;
        FrontSide = side;
        if (side == null) return;
        var sideTransform = side.transform;
        sideTransform.position = SideSetupTarget.position;
        sideTransform.localRotation = SideSetupTarget.localRotation;

        if (!IsDemo())
        {
            var actions = side.GetActionComponents();
            var target = actions.Find((a) => !a.TrainingComplete);
            if (target != null)
            {
                actions.ForEach((a) => a.TrainingActive = false);
                target.TrainingActive = true;
                DefuseManager.Defusing = false;
                DefuseManager.ScopeDefuse = 1;
            }
        }
    }

    /*
        Исполняют соответствующий ActionComponent на FrontSide 
     */
    #region Swipers
    public void SwipeRight()
    {
        if (FrontSide != null)
        {
            DefuseManager.Defusing = true;
            FrontSide.OnSwipe(FrontSide.Right);
            EventSwipe();

        }
        else Debug.LogFormat("FrontSide is null, so swipe was not registering");
    }
    public void SwipeLeft()
    {
        if (FrontSide != null)
        {
            DefuseManager.Defusing = true;
            FrontSide.OnSwipe(FrontSide.Left);
            EventSwipe();
        }
        else Debug.LogFormat("FrontSide is null, so swipe was not registering");
    }
    public void SwipeDown()
    {
        if (FrontSide != null)
        {
            DefuseManager.Defusing = true;
            FrontSide.OnSwipe(FrontSide.Down);
            EventSwipe();
        }
        else Debug.LogFormat("FrontSide is null, so swipe was not registering");
    }
    public void SwipeUp()
    {
        if (FrontSide != null)
        {
            DefuseManager.Defusing = true;
            FrontSide.OnSwipe(FrontSide.Up);
            EventSwipe();
        }
        else Debug.LogFormat("FrontSide is null, so swipe was not registering");
    }
    #endregion
    /*
    Поворачивают ObjectGame на 90 градусов в указанную сторону 
    И вывзывают SetupFrontSide
     */
    #region Rotaters
    public void RotateRight()
    {
        Transform.RelativeRotateRight(90);
        SetupFrontSide();
    }
    public void RotateLeft()
    {
        Transform.RelativeRotateLeft(90);
        SetupFrontSide();
    }
    public void RotateDown()
    {
        Transform.RelativeRotateDown(90);
        SetupFrontSide();
    }
    public void RotateUp()
    {
        Transform.RelativeRotateUp(90);
        SetupFrontSide();
    }

    #endregion

    // private void ManualAction(ActionManual way)
    // {
    //     if (way == null) NotTrueSwipe.Action();
    //     else way.Action();
    // }

    public TransformManager GetTransformManager()
    {
        return Transform;
    }

    public void Show()
    {
        OnShow.Invoke();
    }

    public void Hide()
    {
        OnHide.Invoke();
    }


    public void SetupFrontSide(Side side, Side.Diraction LockDiraction = null, bool isAxis = false)
    {
        if (side == null) return;

        RandomizeSide(side, LockDiraction, isAxis);

        side.Show();

        SetFrontSide(side);
    }

    public Side CreateSide()
    {
        return SidesPool.Get();
    }

    [ContextMenu("SetupFrontSide")]
    public Side SetupFrontSide()
    {
        var side = SidesPool.Get();

        RandomizeSide(side);

        side.Show();

        SetFrontSide(side);

        return side;
    }

    private bool SendLoseMessage = true;

    public void LoseObjectGame()
    {
        if (SendLoseMessage)
            WorldEther.ObjectGameLose.Push(this, this);
    }

    public void HidePrevious()
    {
        if (Previous == null) return;
        Previous.Hide();
        Previous = null;
    }

    public void RandomizeSide(Side side, Side.Diraction LockDiraction = null, bool isAxis = false)
    {

        if (side == null) Debug.LogFormat("RandomizeSide: Side is null");

        side.Initialize();

        List<Side.Diraction> Slots;

        if (LockDiraction != null)
        {
            if (isAxis)
            {
                var inverse = side.GetInverseDiraction(LockDiraction);
                Slots = new List<Side.Diraction>(2);
                if (side.Up != LockDiraction && side.Up != inverse) Slots.Add(side.Up);
                if (side.Down != LockDiraction && side.Down != inverse) Slots.Add(side.Down);
                if (side.Right != LockDiraction && side.Right != inverse) Slots.Add(side.Right);
                if (side.Left != LockDiraction && side.Left != inverse) Slots.Add(side.Left);
            }
            else
            {
                Slots = new List<Side.Diraction>(3);
                if (side.Up != LockDiraction) Slots.Add(side.Up);
                if (side.Down != LockDiraction) Slots.Add(side.Down);
                if (side.Right != LockDiraction) Slots.Add(side.Right);
                if (side.Left != LockDiraction) Slots.Add(side.Left);
            }
        }
        else
            Slots = new List<Side.Diraction>(4) { side.Up, side.Right, side.Down, side.Left };

        var actions = ActionManager.GetRandomGroupActions((byte)Slots.Count, ActionManager.GetData());

        int slot;
        for (int i = Slots.Count - 1; i >= 0; --i)
        {
            slot = Random.Range(0, i + 1);
            side.AddActionComponent(actions[i], Slots[slot]);
            Slots.RemoveAt(slot);
        }

        bool xlock = false, ylock = false;
        for (int i = actions.Length - 1; i >= 0; --i)
        {
            if (actions[i] is ManualTransfer)
            {
                if ((side.Up == actions[i].Diraction || side.Down == actions[i].Diraction))
                    if (!xlock)
                        xlock = true;
                    else continue;
                else if (!ylock)
                    ylock = true;
                else continue;

                side.ForceDisactveActionComponents(side.GetInverseDiraction(actions[i].Diraction));
            }
        }
    }

}
