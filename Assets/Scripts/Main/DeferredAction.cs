using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeferredAction : MonoBehaviour
{
    public delegate void ActionDelegate();

    public static DeferredAction Manager { private set; get; }

    List<IAction> Actions = new List<IAction>(5);

    [SerializeField]
    private float UpdateInterval = 0.125f;

    public float GetUpdateInterval()
    {
        return UpdateInterval;
    }

    public void AddDeferredAction(ActionDelegate action, float time)
    {
        AddDeferredAction(new OnceAction(action, time));
    }
    public void AddDeferredActionCycles(ActionDelegate action, float interval)
    {
        AddDeferredAction(new CyclicalAction(action, interval));
    }
    public void AddDeferredActionCycles(ActionDelegate action, float interval, int countCycles)
    {
        AddDeferredAction(new CountCyclicalAction(action, interval, countCycles));
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        Debug.LogWarning("Attention! The system deferred action - off");
    }

    [ContextMenu("Log")]
    void Log()
    {
        Debug.Log("Count actions: " + Actions.Count);
    }

    public void AddDeferredAction(IAction action)
    {
        Actions.Add(action);
    }

    private float leftTime;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        leftTime += Time.deltaTime;
        if (leftTime >= UpdateInterval)
        {
            for (int i = Actions.Count - 1; i >= 0; --i)
                if (Actions[i].Update(leftTime))
                    Actions.RemoveAt(i);
            leftTime = 0;
        }
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Manager != null)
        {
            Destroy(this);
            return;
        }
        Manager = this;
    }
    void Awake()
    {
        Initialize();
    }

    public interface IAction
    {
        bool Update(float left);
        void Cancel();
    }

    public class OnceAction : IAction
    {
        protected bool Active = true;
        public float RestTime { protected set; get; }
        protected ActionDelegate Method;

        public bool Update(float left)
        {
            if (!Active) return false;
            RestTime -= left;
            if (RestTime > 0) return false;

            return OnAction();
        }

        public virtual void Cancel()
        {
            Method = null;
        }

        protected virtual bool OnAction()
        {
            if (Method != null)
            {
                Method();
                Method = null;
            }
            return true;
        }
        public OnceAction(ActionDelegate method, float targetTime)
        {
            Method = method;
            RestTime = targetTime;
        }
        private OnceAction() { }
    }

    public class CyclicalAction : OnceAction
    {
        public float Interval { set; get; }
        protected bool flagCancel;
        protected override bool OnAction()
        {
            if (Method == null || flagCancel) return true;
            Method();
            RestTime = Interval;
            return false;
        }
        public override void Cancel()
        {
            base.Cancel();
            flagCancel = true;
        }
        public CyclicalAction(ActionDelegate method, float interval)
        : base(method, interval) { Interval = interval; }
    }

    public class CountCyclicalAction : CyclicalAction
    {
        public int CountCycles { set; get; }
        protected override bool OnAction()
        {
            if (Method == null) return true;
            if (CountCycles < 0) return true;
            --CountCycles;
            Method();
            RestTime = Interval;
            return false;
        }
        public override void Cancel()
        {
            base.Cancel();
            CountCycles = 0;
        }
        public CountCyclicalAction(ActionDelegate method, float interval, int countCycles)
        : base(method, interval) { CountCycles = countCycles; }
    }

    public class RestartableAction : CountCyclicalAction
    {
        public int DefaultCountCycles { set; get; }
        public RestartableAction(ActionDelegate method, float interval, int cycles)
        : base(method, interval, cycles) { DefaultCountCycles = cycles; }
        public RestartableAction(ActionDelegate method, float interval)
        : this(method, interval, 1) { }
        protected override bool OnAction()
        {
            if (Method == null || flagCancel) return true;
            --CountCycles;
            Method();
            RestTime = Interval;
            if (CountCycles < 0)
            {
                Active = false;
                return false;
            }
            return false;
        }

        public void Restart()
        {
            Active = true;
            CountCycles = DefaultCountCycles;
            RestTime = Interval;
        }
    }
}
