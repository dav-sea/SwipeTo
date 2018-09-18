using UnityEngine;
using System.Collections.Generic;

namespace TargetChange
{
    public interface IFilter<T>
    {
        void SetActive(bool value);
        bool IsActive();
        void Filter(ref T value);
    }
    public interface ITargetFilter<T> : IFilter<T> { }

    public interface IDifferenceFilter<T> : IFilter<T> { }

    public interface IPostFilter<T> : IFilter<T> { }

    [System.Serializable]
    public abstract class FilterKit<T> : IFilter<T>
    {
        public bool Active { set; get; }
        public void SetActive(bool value)
        {
            Active = value;
        }

        public bool IsActive()
        {
            return Active;
        }

        public abstract void Filter(ref T value);
    }
    public class FilterClaster<TF, F> : FilterKit<F> where TF : IFilter<F>
    {
        [SerializeField]
        List<TF> Filters = new List<TF>(1);

        public override void Filter(ref F value)
        {
            foreach (IFilter<F> filter in Filters)
                if (filter.IsActive())
                    filter.Filter(ref value);
        }

        public T GetFilter<T>() where T : class, TF
        {
            foreach (IFilter<F> filter in Filters)
                if (filter is T) return (T)filter;
            return null;
        }

        public void AddFilter<T>(T filter) where T : class, TF
        {
            Filters.Add(filter);
        }

        public bool RemoveFilter<T>() where T : class, TF
        {
            foreach (IFilter<F> filter in Filters)
                if (filter is T) return Filters.Remove((T)filter);
            return false;
        }

        public FilterClaster(params TF[] filters)
        {
            foreach (TF filter in filters)
                Filters.Add(filter);
        }
    }

    public interface ITarget<T> where T : struct
    {
        T Get();
        bool ClearForFinish();
    }

    public abstract class TargetScript<T> : MonoBehaviour where T : struct
    {
        #region Public Fields
        public bool DisableForFinish;

        public UnityEngine.Events.UnityEvent OnFinish = new UnityEngine.Events.UnityEvent();
        #endregion

        public ITarget<T> Target { private set; get; }

        public FilterClaster<ITargetFilter<T>, T> FilterTarget
        {
            set { _FilterTarget = value; }
            get { return _FilterTarget; }
        }
        public FilterClaster<IDifferenceFilter<T>, T> FilterDifference
        {
            set { _FilterDifference = value; }
            get { return _FilterDifference; }
        }

        public FilterClaster<IPostFilter<T>, T> FilterPost
        {
            set { _FilterPost = value; }
            get { return _FilterPost; }
        }

        [SerializeField]
        private FilterClaster<ITargetFilter<T>, T> _FilterTarget = new FilterClaster<ITargetFilter<T>, T>();
        [SerializeField]
        private FilterClaster<IDifferenceFilter<T>, T> _FilterDifference = new FilterClaster<IDifferenceFilter<T>, T>();
        [SerializeField]
        private FilterClaster<IPostFilter<T>, T> _FilterPost = new FilterClaster<IPostFilter<T>, T>();

        public void SetTarget(ITarget<T> target)
        {
            Target = target;
            enabled = true;
        }
        protected T _target, _current, _difference;
        public T GetTargetPosition()
        {
            return Target != null ? Target.Get() : GetCurrent();
        }

        protected T GetTarget()
        {
            if (Target == null) return GetCurrent();

            _target = Target.Get();
            if (_FilterTarget.Active) _FilterTarget.Filter(ref _target);
            return _target;
        }

        protected abstract T GetCurrent();

        protected abstract T Add(T right, T left);

        protected abstract void SetCurrent(T value);

        protected abstract T GetDifference(T current, T target, float delta);

        public abstract bool IsFinish();

        bool wasFinish;

        protected void UpdateStep()
        {
            _target = GetTarget();
            _current = GetCurrent();
            if (_target.Equals(_current))
            {
                if (!wasFinish)
                {
                    wasFinish = true;
                    OnFinish.Invoke();
                }
                if (Target != null && Target.ClearForFinish())
                    Target = null;
                if (DisableForFinish)
                    enabled = false;
                return;
            }
            wasFinish = false;
            _current = Add(GetDifference(_current, _target, Time.deltaTime), _current);
            if (_FilterPost.Active) _FilterPost.Filter(ref _current);
            SetCurrent(_current);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            UpdateStep();
        }

        private bool _initialized;
        public void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            //Initialize logic
            OnInitialize();

        }
        protected virtual bool OnInitialize() { return true; }
        void Awake()
        {
            Initialize();
        }
    }
}