using UnityEngine;
using System.Collections.Generic;
using TargetChange;

public class TargetRotationScript : TargetScript<Quaternion>
{
    public bool Local;
    #region  Private Fields

    private Transform _transform;

    #endregion

    #region  Public Methods
    public void SetTarget(Transform target, bool ClearTargetWhenFinish = false)
    {
        this.SetTarget(new TransformTarget(target, ClearTargetWhenFinish, Local));
    }
    public void SetTarget(Quaternion target)
    {
        this.SetTarget(new QuaternionTarget(target));
    }

    public bool TargetIsTransform()
    {
        return Target is TransformTarget;
    }

    protected override Quaternion GetCurrent()
    {
        return Local ? _transform.localRotation : _transform.rotation;
    }

    protected override Quaternion GetDifference(Quaternion current, Quaternion target, float delta)
    {
        _difference = new Quaternion(target.x - current.x, target.y - current.y, target.z - current.z, target.w - current.w);
        if (FilterDifference.Active) FilterDifference.Filter(ref _difference);
        _difference.Set(_difference.x * delta, _difference.y * delta, _difference.z * delta, _difference.w * delta);
        return _difference;
    }

    public Transform TryGetTransformTarget()
    {
        if (TargetIsTransform())
            return (Target as TransformTarget).Transform;
        return null;
    }

    public override bool IsFinish()
    {
        return GetTarget() == GetCurrent();
    }

    #endregion

    protected override Quaternion Add(Quaternion right, Quaternion left)
    {
        return new Quaternion(right.x + left.x, right.y + left.y, right.z + left.z, right.w + left.w);
    }

    protected override void SetCurrent(Quaternion value)
    {
        if (Local) _transform.localRotation = value;
        else _transform.rotation = value;
    }


    protected override bool OnInitialize()
    {
        _transform = transform;

        Corrective.Active = true;
        FilterPost.Active = true;

        return true;
    }

    public CorrectiveFilter Corrective
    {
        get
        {
            var filter = FilterPost.GetFilter<CorrectiveFilter>();
            if (filter == null)
            {
                filter = new CorrectiveFilter(this);
                FilterPost.AddFilter(filter);
            }
            return filter;
        }
    }

    public OffsetFilter Offset
    {
        get
        {
            var filter = FilterTarget.GetFilter<OffsetFilter>();
            if (filter == null)
            {
                filter = new OffsetFilter();
                FilterTarget.AddFilter(filter);
            }
            return filter;
        }
    }

    public AccelerateFilter Accelerate
    {
        get
        {
            var filter = FilterDifference.GetFilter<AccelerateFilter>();
            if (filter == null)
            {
                filter = new AccelerateFilter();
                FilterDifference.AddFilter(filter);
            }
            return filter;
        }
    }

    #region  Filters
    [System.Serializable]
    public abstract class FilterQuaternionKit : FilterKit<Quaternion> { }


    [System.Serializable]
    public class AccelerateFilter : FilterQuaternionKit, IDifferenceFilter<Quaternion>
    {
        [SerializeField]
        public float AccelerateValue = 1;
        public override void Filter(ref Quaternion value)
        {
            value.Set(value.x * AccelerateValue, value.y * AccelerateValue, value.z * AccelerateValue, value.w * AccelerateValue);
        }
        public AccelerateFilter(float accelerateValue = 1)
        {
            AccelerateValue = accelerateValue;
        }
    }
    [System.Serializable]
    public class OffsetFilter : FilterQuaternionKit, ITargetFilter<Quaternion>
    {
        [SerializeField]
        public Quaternion Offset;
        public override void Filter(ref Quaternion value)
        {
            value = Offset * value;
        }
    }
    [System.Serializable]
    public class CorrectiveFilter : FilterQuaternionKit, IPostFilter<Quaternion>
    {
        private TargetRotationScript TargetRotation;
        public float ValidAmendment = 0.002f;
        Quaternion target;
        public override void Filter(ref Quaternion value)
        {
            if (TargetRotation == null) return;

            target = TargetRotation.GetTarget();

            value.x = Get(value.x, target.x);
            value.y = Get(value.y, target.y);
            value.z = Get(value.z, target.z);
            value.w = Get(value.w, target.w);
        }
        private float Get(float value, float target)
        {
            return Mathf.Abs(Mathf.Abs(value) - Mathf.Abs(target)) <= ValidAmendment ? target : value;
        }
        private CorrectiveFilter() { }
        public CorrectiveFilter(TargetRotationScript targetRotation)
        {
            TargetRotation = targetRotation;
        }
    }

    #endregion

    #region  Target

    [System.Serializable]
    public class TransformTarget : ITarget<Quaternion>
    {
        public bool LocalTarget;
        public Transform Transform { set; get; }
        public bool ClearFinish { set; get; }
        private Quaternion previous;
        public Quaternion Get()
        {
            if (Transform != null)
            {
                previous = LocalTarget ? Transform.localRotation : Transform.rotation;
            }
            return previous;
        }

        public bool ClearForFinish()
        {
            return ClearFinish;
        }
        private TransformTarget() { }
        public TransformTarget(Transform transform, bool clearFinish = false, bool local = false)
        {
            // if (transform == null) throw new System.ArgumentNullException("transform");
            Transform = transform;
            ClearFinish = clearFinish;
            LocalTarget = local;
        }

        public TransformTarget(Quaternion def)
        {
            previous = def;
        }
    }
    [System.Serializable]
    public class QuaternionTarget : ITarget<Quaternion>
    {
        public Quaternion Quaternion { set; get; }
        public Quaternion Get()
        {
            return Quaternion;
        }
        public virtual bool ClearForFinish()
        {
            return true;
        }
        private QuaternionTarget() { }
        public QuaternionTarget(Quaternion quaternion)
        {
            Quaternion = quaternion;
        }
    }
    #endregion
}