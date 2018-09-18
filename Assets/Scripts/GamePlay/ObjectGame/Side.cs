using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : Appearance
{
    private Diraction _Up, _Left, _Right, _Down;

    [SerializeField]
    private ObjectGame Parent;

    [SerializeField]
    private Transform _SignsContenier;

    public Transform SignsContenier { get { return _SignsContenier; } }

    [SerializeField]
    private UIOrganization.AppearanceAnimation Animation;

    // public event System.Action EventSwipe;

    public List<ActionComponent> ActionComponents;

    public Diraction Up
    {
        get
        {
            if (_Up == null)
                _Up = new Diraction(this, Parent.GetTransformManager().RelativeRotateUp, Quaternion.identity, Vector3.zero);
            return _Up;
        }
    }
    public Diraction Left
    {
        get
        {
            if (_Left == null)
                _Left = new Diraction(this, Parent.GetTransformManager().RelativeRotateLeft, Quaternion.AngleAxis(90, Vector3.forward), Vector3.zero);
            return _Left;
        }
    }
    public Diraction Right
    {
        get
        {
            if (_Right == null)
                _Right = new Diraction(this, Parent.GetTransformManager().RelativeRotateRight, Quaternion.AngleAxis(90, -Vector3.forward), Vector3.zero);
            return _Right;
        }
    }
    public Diraction Down
    {
        get
        {
            if (_Down == null)
                _Down = new Diraction(this, Parent.GetTransformManager().RelativeRotateDown, Quaternion.AngleAxis(180, Vector3.forward), Vector3.zero);
            return _Down;
        }
    }

    public Diraction GetInverseDiraction(Diraction diraction)
    {
        if (diraction == null) return null;
        if (diraction == Up) return Down;
        else if (diraction == Down) return Up;
        else if (diraction == Right) return Left;
        else if (diraction == Left) return Right;
        return null;
    }

    public Diraction GetEqualsDiraction(Diraction diraction)
    {
        if (diraction == null) return null;
        if (Up.Equals(diraction)) return Up;
        else if (Left.Equals(diraction)) return Left;
        else if (Right.Equals(diraction)) return Right;
        else if (Down.Equals(diraction)) return Down;
        return null;
    }

    public ObjectGame GetObjectGame()
    {
        return Parent;
    }

    public void OnSwipe(Diraction diraction)
    {
        bool flag = false;
        for (int i = 0; i < ActionComponents.Count; ++i)
            flag = flag || ActionComponents[i].Swipe(diraction);
        if (!flag) Parent.LoseObjectGame();
        // EventSwipe();
    }

    public Diraction[] GetDiractions()
    {
        return new Diraction[] { Up, Left, Right, Down };
    }

    private void ExecuteAction(ActionComponent comp)
    {
        if (comp == null) return;
        comp.GameObject.SetActive(false);
        comp.Transform.parent = GetObjectGame().GetTransformManager().Transform;
        // ActionPool.Manager.Add(comp);
        GetObjectGame().GetActionManager().Recovery(comp);
    }

    public void Destroy()
    {
        for (int i = ActionComponents.Count - 1; i >= 0; --i)
            ExecuteAction(ActionComponents[i]);
        Destroy(gameObject, 0.45f);
    }

    protected override void OnInitialize()
    {
        if (SignsContenier == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "SignsContenier", name);
            enabled = false;
            return;
        }
        if (Parent == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Parent", name);
            enabled = false;
            return;
        }
        if (Animation == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Animation", name);
            enabled = false;
            return;
        }
        // EventSwipe += delegate { };
        // if (StartIsHide) transform.localScale = SideAnimation.HideValue;
        Animation.Initialize();
        ActionComponents = new List<ActionComponent>();
        EventShow += delegate
        {
            for (int i = ActionComponents.Count - 1; i >= 0; --i)
                ActionComponents[i].Transform.localScale = Vector3.one;
        };
        Animation.EventFinishShow += PreviousHide;
        Animation.EventFinishHide += Destroy;
    }

    public UIOrganization.AppearanceAnimation GetAnimation()
    {
        return Animation;
    }

    private void PreviousHide()
    {
        Parent.HidePrevious();
    }


    public void AddActionComponent(ActionComponent component, Diraction setup)
    {
        if (component != null)
        {
            component.Initialize();

            ActionComponents.Add(component);
            if (setup != null) setup.SetActionComponent(component);
        }
    }

    //TODO Replace foreach to for
    public void HideActionComponents(Diraction diraction, bool disactive)
    {
        if (diraction != null)
            foreach (ActionComponent component in ActionComponents)
                if (component != null && diraction == component.Diraction)
                {
                    component.Hide();
                    if (disactive) component.IsActiveAction = false;
                }
    }
    //TODO Replace foreach to for
    public void ForceDisactveActionComponents(Diraction diraction)
    {
        if (diraction != null)
            foreach (ActionComponent component in ActionComponents)
                if (component != null && diraction == component.Diraction)
                {
                    component.GameObject.SetActive(false);
                    component.IsActiveAction = false;
                }
    }

    public List<ActionComponent> GetActionComponents()
    {
        return ActionComponents;
    }

    public void RemoveActionComponents(Diraction diraction, bool Hide = false)
    {
        if (diraction == null) return;
        ActionComponent element;
        for (int i = 0; i < ActionComponents.Count; ++i)
        {
            element = ActionComponents[i];
            if (element != null && diraction == element.Diraction)
            {
                ActionComponents.RemoveAt(i);
                if (Hide) element.Hide();
            }
        }
    }

    public void RemoveActionComponent(ActionComponent element, bool Hide = false)
    {
        if (element == null) return;

        // element = ActionComponents[i];
        // if (element != null && diraction == element.Diraction)
        // {
        //     ActionComponents.RemoveAt(i);
        //     if (Hide) element.Hide();
        // }

        ActionComponents.Remove(element);
        if (Hide) element.Hide();

    }

    void Awake()
    {
        Initialize();
    }


    [System.Serializable]
    public class Diraction
    {
        public Side Parent { private set; get; }
        public Vector3 LocalPosition { private set; get; }
        public Quaternion LocalRotation { private set; get; }
        private RotateDelegate RotateFunction;
        public RotateDelegate GetRotateFunction()
        {
            return RotateFunction;
        }

        public bool ReferenceEquals(object obj)
        {
            return obj == (System.Object)this;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Diraction)) return false;

            var diraction = (Diraction)obj;

            return (diraction.LocalPosition == LocalPosition
                   && diraction.LocalRotation == LocalRotation
                   && diraction.GetRotateFunction() == GetRotateFunction());
        }

        public override int GetHashCode()
        {
            return (int)((LocalRotation.x + LocalRotation.y + LocalRotation.z) * LocalRotation.w * 100000);
        }

        public void SetTransform(Transform transform)
        {
            if (transform == null) return;
            transform.parent = Parent.SignsContenier;
            transform.localPosition = LocalPosition;
            transform.localRotation = LocalRotation;
        }
        public void SetActionComponent(ActionComponent component)
        {
            if (component == null) return;
            component.Diraction = this;
            SetTransform(component.Transform);
        }

        public Diraction(Side parent, RotateDelegate rotateFunction, Quaternion localRotation, Vector3 localPosition)
        {
            Parent = parent;
            RotateFunction = rotateFunction;
            LocalRotation = localRotation;
            LocalPosition = localPosition;
        }

        public delegate void RotateDelegate(float angle);

        // public static bool operator ==(Diraction left, Diraction right)
        // {
        //     return left != null && left.Equals(right);
        // }
        // public static bool operator !=(Diraction left, Diraction right)
        // {
        //     return left != null && !left.Equals(right);
        // }
    }
}
