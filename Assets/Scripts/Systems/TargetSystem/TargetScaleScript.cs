using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TargetChange;

public class TargetScaleScript : TargetScript<Vector3>
{
    #region  Private Fields

    private Transform _transform;

    #endregion

    #region  Public Methods
    public void SetTarget(Transform target, bool ClearTargetWhenFinish = false)
    {
        this.SetTarget(new TransformTarget(target, ClearTargetWhenFinish));
    }

    public void SetTarget(Vector3 target)
    {
        this.SetTarget(new PointTarget(target));
    }

    public bool TargetIsTransform()
    {
        return Target is TransformTarget;
    }

    protected override Vector3 GetCurrent()
    {
        return _transform.localScale;
    }

    protected override Vector3 GetDifference(Vector3 current, Vector3 target, float delta)
    {
        _difference = target - current;
        if (FilterDifference.Active) FilterDifference.Filter(ref _difference);
        return _difference * delta;
    }

    public Transform TryGetTransformTarget()
    {
        if (TargetIsTransform())
            return (Target as TransformTarget).Transform;
        return null;
    }

    public Transform GetTransform()
    {
        return _transform;
    }

    public override bool IsFinish()
    {
        return GetTarget() == GetCurrent();
    }
    #endregion

    protected override void SetCurrent(Vector3 value)
    {
        _transform.localScale = value;
        // Debug.Log("Set" + value);
    }

    protected override Vector3 Add(Vector3 left, Vector3 right)
    {
        return left + right;
    }

    protected override bool OnInitialize()
    {
        _transform = transform;

        Corrective.Active = true;
        Accelerate.Active = true;
        ZeroLimiter.Active = true;

        Accelerate.AccelerateValue = 3f;

        FilterDifference.Active = true;
        FilterPost.Active = true;
        FilterTarget.Active = true;

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

    public ZeroLimitFilter ZeroLimiter
    {
        get
        {
            var filter = FilterTarget.GetFilter<ZeroLimitFilter>();
            if (filter == null)
            {
                filter = new ZeroLimitFilter();
                FilterTarget.AddFilter(filter);
            }
            return filter;
        }
    }

    #region  Filters
    [System.Serializable]
    public abstract class FilterVectorKit : FilterKit<Vector3> { }


    [System.Serializable]
    public class AccelerateFilter : FilterVectorKit, IDifferenceFilter<Vector3>
    {
        [SerializeField]
        public float AccelerateValue = 1;
        public override void Filter(ref Vector3 value)
        {
            value *= AccelerateValue;
        }
        public AccelerateFilter(float accelerateValue = 1)
        {
            AccelerateValue = accelerateValue;
        }
    }
    [System.Serializable]
    public class OffsetFilter : FilterVectorKit, ITargetFilter<Vector3>
    {
        [SerializeField]
        public Vector3 Offset;
        public override void Filter(ref Vector3 value)
        {
            value += Offset;
        }
    }
    public class CorrectiveFilter : FilterVectorKit, IPostFilter<Vector3>
    {
        private TargetScaleScript TargetScale;
        public float ValidAmendment = 0.002f;
        Vector3 target;
        public override void Filter(ref Vector3 value)
        {
            if (TargetScale == null) return;

            target = TargetScale.GetTarget();

            value.x = Get(value.x, target.x);
            value.y = Get(value.y, target.y);
            value.z = Get(value.z, target.z);
        }
        private float Get(float value, float target)
        {
            return Mathf.Abs(Mathf.Abs(value) - Mathf.Abs(target)) <= ValidAmendment ? target : value;
        }
        private CorrectiveFilter() { }
        public CorrectiveFilter(TargetScaleScript targetScale)
        {
            TargetScale = targetScale;
        }
    }

    public class ZeroLimitFilter : FilterVectorKit, ITargetFilter<Vector3>
    {
        public override void Filter(ref Vector3 value)
        {
            Limit(ref value.x);
            Limit(ref value.y);
            Limit(ref value.z);
        }

        private void Limit(ref float value)
        {
            value = value < 0 ? 0 : value;
        }
    }


    #endregion

    #region  Target

    [System.Serializable]
    public class TransformTarget : ITarget<Vector3>
    {
        public Transform Transform { set; get; }
        public bool ClearFinish { set; get; }
        private Vector3 previous;
        public Vector3 Get()
        {
            if (Transform != null)
                previous = Transform.localScale;
            return previous;
        }

        public bool ClearForFinish()
        {
            return ClearFinish;
        }
        private TransformTarget() { }
        public TransformTarget(Transform transform, bool clearFinish = false)
        {
            // if (transform == null) throw new System.ArgumentNullException("transform");
            Transform = transform;
            ClearFinish = clearFinish;
        }

        public TransformTarget(Vector3 def)
        {
            previous = def;
        }
    }
    [System.Serializable]
    public class PointTarget : ITarget<Vector3>
    {
        public Vector3 Point { set; get; }
        public Vector3 Get()
        {
            return Point;
        }
        public virtual bool ClearForFinish()
        {
            return false;
        }
        private PointTarget() { }
        public PointTarget(Vector3 point)
        {
            Point = point;
        }
    }
    #endregion
}