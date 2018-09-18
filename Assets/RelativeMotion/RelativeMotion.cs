using System;
using UnityEngine;

// Version 1.0
namespace RelativeMotion
{
    public delegate T CurrentWorker<T>(T start, T target, float condition);//c(t) = S + t(T - S)

    public static class RelatveMotionWorkers
    {
        public static float CurrentWorkerFloat(float start, float target, float condition)
        {
            return start + condition * (target - start);
        }
        public static Vector2 CurrentWorkerVector2(Vector2 start, Vector2 target, float condition)
        {
            return start + condition * (target - start);
        }
        public static Vector3 CurrentWorkerVector3(Vector3 start, Vector3 target, float condition)
        {
            return start + condition * (target - start);
        }
        public static Vector4 CurrentWorkerVector4(Vector4 start, Vector4 target, float condition)
        {
            return start + condition * (target - start);
        }
        public static Quaternion CurrentWorkerQuaternion(Quaternion start, Quaternion target, float condition)
        {
            return new Quaternion(start.x + condition * (target.x - start.x),
                                    start.y + condition * (target.y - start.y),
                                    start.z + condition * (target.z - start.z),
                                    start.w + condition * (target.w - start.w)
            );
        }
    }

    public enum LoopMode { Once, Loop, PingPong };

    public interface IRelativeMotionTime
    {
        float CurrentTime { set; get; }
        float TimeScale { set; get; }
        void AddTime(float delta);
    }
    public interface IRelativeMotionCondition
    {
        float GetCondition(float time);
    }

    public class CurveCondition : IRelativeMotionCondition
    {
        private readonly static AnimationCurve DefaultCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
        [SerializeField] private AnimationCurve _Curve;
        public AnimationCurve Curve
        {
            set { _Curve = value; }
            get { return _Curve; }
        }

        public float GetCondition(float time)
        {
            return _Curve.Evaluate(time);
        }

        public CurveCondition(AnimationCurve curve)
        {
            _Curve = curve;
        }

        public CurveCondition()
        {
            _Curve = new AnimationCurve(DefaultCurve.keys);
        }
    }
    public class TimeMotion : IRelativeMotionTime
    {
        public float CurrentTime { set; get; }
        public float TimeScale { set; get; }

        public void AddTime(float delta)
        {
            CurrentTime += delta * TimeScale;
        }
    }
    public delegate void FinishDelegate<T>(T target);
    public interface IRelativeMotionEngine
    {
        bool Active { set; get; }
        bool Update(float delta);
    }
    public interface IRelativeMotionEngineDataInterface<T>
    {
        void SetupMotionData(MotionData<T> data, bool activate);
        MotionData<T> CreateMotionData();
    }
    public interface IRelativeMotionEngineWork<T> : IRelativeMotionEngine
    {
        event FinishDelegate<T> EventFinish;
        LoopMode LoopMode { set; get; }
        T Start { set; get; }
        T Target { set; get; }
        T GetCurrent();
    }

    public interface IRelativeMotionEngineTime : IRelativeMotionEngine
    {
        float TimeScale { set; get; }
        float TimeCurrent { set; get; }
    }

    public class RelativeMotionEngine<T> : IRelativeMotionEngineWork<T>, IRelativeMotionEngineTime, IRelativeMotionEngineDataInterface<T>
    {
        public event FinishDelegate<T> EventFinish;

        public LoopMode LoopMode { set; get; }

        // public
        public bool Active { set { _Active = value; } get { return _Active; } }
        public T Start { set { _Start = value; } get { return _Start; } }
        public T Target { set { _Target = value; } get { return _Target; } }

        public float TimeScale
        {
            set { Time.TimeScale = value; }
            get { return Time.TimeScale; }
        }

        public float TimeCurrent
        {
            set { Time.CurrentTime = value; }
            get { return Time.CurrentTime; }
        }

        public void SetupMotionData(MotionData<T> data, bool activate = true)
        {
            if (data == null) return;
            _Start = data.Start;
            _Target = data.Target;

            if (data.OffsetTime != 0)
                TimeCurrent = 1 / data.OffsetTime;
            else TimeCurrent = 0;

            if (data.TargetTime != 0)
                TimeScale = 1 / data.TargetTime;
            else TimeCurrent = 1;

            if (data.CurveCondition != null)
                Condition.Curve = data.CurveCondition;

            // Debug.Log(TimeCurrent + "");
            Active = activate || Active;
        }

        public MotionData<T> CreateMotionData()
        {
            return new MotionData<T>(_Start, _Target, 1 / TimeScale, 1 / TimeCurrent, new AnimationCurve(Condition.Curve.keys));
        }

        public IRelativeMotionTime Time;
        public CurveCondition Condition;

        private CurrentWorker<T> Worker;
        [SerializeField] private bool _Active;
        [SerializeField] private T _Start;
        [SerializeField] private T _Target;

        public bool Update(float delta)
        {
            if (!_Active || Time == null || Condition == null) return false;
            Time.AddTime(delta * _deltaVector);
            if (_deltaVector >= 1 && Time.CurrentTime >= 1 || _deltaVector <= -1 && Time.CurrentTime == 0)
            {
                OnFinish();
            }
            return true;
        }

        private float _deltaVector = 1;

        private void OnFinish()
        {
            switch (LoopMode)
            {
                case LoopMode.Loop:
                    Time.CurrentTime = _deltaVector == 1 ? 0 : 1;

                    break;
                case LoopMode.PingPong:
                    _deltaVector = -_deltaVector;
                    Time.CurrentTime = _deltaVector == 1 ? 0 : 1;
                    break;
                default:
                    EventFinish(_Target);
                    _Active = false;
                    break;
            }

        }

        public T GetCurrent()
        {
            if (Condition == null || Time == null) return default(T);
            return Worker(_Start, _Target, Condition.GetCondition(Time.CurrentTime));
        }

        private RelativeMotionEngine() { }
        public RelativeMotionEngine(CurrentWorker<T> worker, CurveCondition condition, IRelativeMotionTime time, T start, T target, bool active = false)
        {
            Worker = worker;
            Condition = condition;
            Time = time;
            _Start = start;
            _Target = target;
            _Active = active;
            EventFinish += delegate { };
        }

        public RelativeMotionEngine(CurrentWorker<T> worker, CurveCondition condition, IRelativeMotionTime time)
        {
            Worker = worker;
            Condition = condition;
            Time = time;
            EventFinish += delegate { };
        }

        public RelativeMotionEngine(CurrentWorker<T> worker)
        {
            Worker = worker;
            EventFinish += delegate { };
        }

    }
    public class MotionData<T>
    {
        public T Target;
        public T Start;
        public float TargetTime = 1;
        public float OffsetTime;
        public AnimationCurve CurveCondition;

        private MotionData() { }
        public MotionData(T start, T target, float targetTime, float offsetTime, AnimationCurve curve)
        {
            Target = target;
            Start = start;
            TargetTime = targetTime;
            OffsetTime = offsetTime;
            CurveCondition = curve;
        }
        public MotionData(T start, T target, float targetTime = 1, float offsetTime = 0)
        : this(start, target, targetTime, offsetTime, null) { }
    }

    // public class MotionTransformPoint
    // {
    //     public readonly Vector3 Position;
    //     public readonly Quaternion Rotation;
    //     public readonly Vector3 Scale;

    //     private MotionTransformPoint() { }
    //     public MotionTransformPoint(Vector3 position, Quaternion rotation, Vector3 scale)
    //     {
    //         Position = position;
    //         Rotation = rotation;
    //         Scale = scale;
    //     }
    // }
    [Serializable]
    public class RelativeMotionTransformEngine : IRelativeMotionEngine
    {
        public bool Active { set { _Active = value; } get { return _Active; } }
        public bool LocalPosition;
        public bool LocalRotation;

        [SerializeField] private bool _Active;
        [SerializeField] private Transform Transform;

        private RelativeMotionEngine<Vector3> _PositionEngine = new RelativeMotionEngine<Vector3>(RelatveMotionWorkers.CurrentWorkerVector3, new CurveCondition(), new TimeMotion());
        private RelativeMotionEngine<Quaternion> _RotationEngine = new RelativeMotionEngine<Quaternion>(RelatveMotionWorkers.CurrentWorkerQuaternion, new CurveCondition(), new TimeMotion());
        private RelativeMotionEngine<Vector3> _ScaleEngine = new RelativeMotionEngine<Vector3>(RelatveMotionWorkers.CurrentWorkerVector3, new CurveCondition(), new TimeMotion());

        public RelativeMotionEngine<Vector3> PositionEngine { get { return _PositionEngine; } }
        public RelativeMotionEngine<Quaternion> RotationEngine { get { return _RotationEngine; } }
        public RelativeMotionEngine<Vector3> ScaleEngine { get { return _ScaleEngine; } }

        bool _ChangeFlag;
        public bool Update(float delta)
        {
            if (!_Active) return false;
            _ChangeFlag = false;

            if (_PositionEngine.Update(delta))
            {
                if (LocalPosition) Transform.localPosition = _PositionEngine.GetCurrent();
                else Transform.position = _PositionEngine.GetCurrent();
                _ChangeFlag = true;
            }
            if (_RotationEngine.Update(delta))
            {
                if (LocalRotation) Transform.localRotation = _RotationEngine.GetCurrent();
                else Transform.rotation = _RotationEngine.GetCurrent();
                _ChangeFlag = true;
            }
            if (_ScaleEngine.Update(delta))
            {
                Transform.localScale = _ScaleEngine.GetCurrent();
                _ChangeFlag = true;
            }

            if (!_ChangeFlag) _Active = false;

            return _ChangeFlag;
        }
        private void NewMotion<T>(RelativeMotionEngine<T> engine, T start, T target, float targetTime = 1, float offsetTime = 0, AnimationCurve curve = null)
        {
            engine.Start = start;
            engine.Target = target;
            if (targetTime != 0)
            {
                engine.TimeCurrent = offsetTime / targetTime;
                engine.TimeScale = 1 / targetTime;
            }
            else
            {
                engine.TimeCurrent = 1;
                engine.TimeScale = 1;
            }
            if (curve != null)
                engine.Condition.Curve = curve;

            engine.Active = true;
        }
        public void NewMotionPosition(Vector3 start, Vector3 target, float targetTime = 1, float offsetTime = 0)
        {
            NewMotion(_PositionEngine, start, target, targetTime, offsetTime);
        }
        public void NewMotionPosition(MotionData<Vector3> data)
        {
            _PositionEngine.SetupMotionData(data);
        }
        public void NewMotionRotation(Quaternion start, Quaternion target, float targetTime = 1, float offsetTime = 0)
        {
            NewMotion(_RotationEngine, start, target, targetTime, offsetTime);
        }
        public void NewMotionRotation(MotionData<Quaternion> data)
        {
            _RotationEngine.SetupMotionData(data);
        }
        public void NewMotionScale(Vector3 start, Vector3 target, float targetTime = 1, float offsetTime = 0)
        {
            NewMotion(_ScaleEngine, start, target, targetTime, offsetTime);
        }
        public void NewMotionScale(MotionData<Vector3> data)
        {
            _ScaleEngine.SetupMotionData(data);
        }

        public Vector3 GetPosition()
        {
            return LocalPosition ? Transform.localPosition : Transform.position;
        }
        public Quaternion GetRotation()
        {
            return LocalRotation ? Transform.localRotation : Transform.rotation;
        }
        public Vector3 GetScale()
        {
            return Transform.localScale;
        }
        public Transform GetTransform()
        {
            return Transform;
        }
    }

    // [Serializable]
    // public class RelativeMotionVolumeEngine : RelativeMotionEngine<float>
    // {
    //     public RelativeMotionVolumeEngine()
    //     :base(RelatveMotionWorkers.CurrentWorkerFloat){}}
    // }
}