using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Manager { private set; get; }

    [SerializeField] private Appearance MessageAppearance;
    [SerializeField] private UIOrganization.AppearanceAnimation MessageAnimation;
    [SerializeField] private UnityEngine.UI.Text Text;

    private Leaf Current;

    private Queue<Leaf> Messages = new Queue<Leaf>(1);

    public static IHideMessage ShowMessage(string message, float interval)
    {
        return Manager.Show(message, interval);
    }
    public static IHideMessage ShowMessage(string message)
    {
        return Manager.Show(message);
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Manager != null) { Destroy(this); return; }
        Manager = this;
        if (MessageAppearance == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "MessageAppearance", name);
            enabled = false;
            return;
        }
        if (MessageAnimation == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", " MessageAnimation", name);
            enabled = false;
            return;
        }
        if (Text == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Text", name);
            enabled = false;
            return;
        }
        MessageAppearance.Initialize();
        MessageAnimation.EventFinishHide += IsHideHandler;
    }

    private void IsHideHandler()
    {
        UpdateMessage();
    }

    private void UpdateMessage()
    {
        if (Messages.Count > 0)
        {
            Current = Messages.Dequeue();
            Text.text = Current.Message;
            Text.fontSize = Mathf.Clamp(Mathf.RoundToInt(144 - Current.Message.Length * 6), 24, 600);
            MessageAppearance.Show();
        }
    }

    void Awake()
    {
        Initialize();
    }
    public IHideMessage Show(string message, float interval, System.Action click)
    {
        var leaf = new TimeLeaf(message, MessageAppearance.Hide, click, interval);
        Messages.Enqueue(leaf);
        if (!MessageAppearance.IsAppearance) UpdateMessage();
        return leaf;
    }
    public IHideMessage Show(string message, float interval)
    {
        var leaf = new TimeLeaf(message, MessageAppearance.Hide, null, interval);
        Messages.Enqueue(leaf);
        if (!MessageAppearance.IsAppearance) UpdateMessage();
        return leaf;
    }
    public IHideMessage Show(string message)
    {
        var leaf = new Leaf(message, MessageAppearance.Hide, null);
        Messages.Enqueue(leaf);
        if (!MessageAppearance.IsAppearance) UpdateMessage();
        return leaf;
    }

    private class Leaf : IHideMessage
    {
        public string Message { private set; get; }
        System.Action HideMethod;
        System.Action ClickMethod;
        private bool wasClick;

        public virtual void Hide()
        {
            HideMethod();
        }

        public virtual void Click()
        {
            if (ClickMethod != null && !wasClick) ClickMethod();
            wasClick = true;
        }

        public Leaf(string message, System.Action methodHide, System.Action click)
        {
            Message = message;
            HideMethod = methodHide;
            ClickMethod = click;
        }
    }

    public void OnClick()
    {
        if (Current != null && MessageAppearance.IsAppearance) Current.Click();
    }

    private class TimeLeaf : Leaf
    {
        public float Interval { private set; get; }
        private DeferredAction.IAction DeferredHide;
        public TimeLeaf(string message, System.Action methodHide, System.Action click, float interval)
        : base(message, methodHide, click)
        {
            Interval = interval;
            DeferredHide = new DeferredAction.OnceAction(delegate { methodHide(); }, interval);
            DeferredAction.Manager.AddDeferredAction(DeferredHide);
        }
        public override void Hide()
        {
            DeferredHide.Cancel();
            base.Hide();
        }
    }

    public interface IHideMessage
    {
        void Hide();
    }
}
