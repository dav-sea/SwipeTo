using System;

public interface IChangebleStack<T>
{
    int Count { get; }
    void Push(T obj);
    T Pop();
    T Peek();
    void Clear();
    bool Delete(T obj);
    bool Contains(T obj);
    T[] ToArray();
}
public class ChangebleStack<T> : IChangebleStack<T>
{
    private Leaf Current;
    public int Length { private set; get; }
    public int Count
    {
        get
        {
            int count;
            Leaf current = Current;
            for (count = 0; current != null; ++count)
                current = current.Previous;
            Length = count;
            return count;
        }
    }

    public bool IsEmpty()
    {
        return Current == null;
    }

    public void Push(T obj)
    {
        if (obj == null) return;
        // UnityEngine.Debug.Log("here1");
        if (Current != null)
        {
            Current.Next = new Leaf(obj, Current);
            Current = Current.Next;
        }
        else
            Current = new Leaf(obj, null);

        ++Length;
    }

    public void Clear()
    {
        while (!IsEmpty())
            Pop();
    }

    public T Pop()
    {
        if (IsEmpty()) throw new ChangbleStackException("Stack dont have leafs");
        T result = Current.Contents;
        Current = Current.Previous;
        if (Current != null)
            Current.Next = null;
        --Length;
        return result;
    }
    public T Peek()
    {
        if (IsEmpty()) throw new ChangbleStackException("Stack dont have leafs");
        return Current.Contents;
    }

    public bool Delete(T obj)
    {
        var leaf = ContainsToLeaf(obj);
        if (leaf != null)
        {
            if (leaf.Previous != null && leaf.Previous.Next != null)
                leaf.Previous.Next = leaf.Next;
            if (leaf.Next != null && leaf.Next.Previous != null)
                leaf.Next.Previous = leaf.Previous;
            --Length;
            return true;
        }
        return false;
    }

    public bool Contains(T obj)
    {
        return ContainsToLeaf(obj) != null;
    }

    private Leaf ContainsToLeaf(T obj)
    {
        if (obj == null) return null;
        Leaf targetleaf = Current;
        while (targetleaf != null && !EqualsContents(targetleaf.Contents, obj))
            targetleaf = targetleaf.Previous;
        return targetleaf;
    }

    //Begin is First In
    public T[] ToArray()
    {
        var result = new T[Count];

        Leaf current = Current;
        for (int i = result.Length - 1; current != null; i--)
        {
            result[i] = current.Contents;
            current = current.Previous;
        }

        return result;
    }

    private class Leaf
    {
        public Leaf Next;
        public T Contents;
        public Leaf Previous;

        public Leaf(T contents, Leaf previous)
        {
            Contents = contents;
            Previous = previous;
        }
    }

    private EqualsContentsDelegate EqualsContents;
    private delegate bool EqualsContentsDelegate(T left, T right);

    public ChangebleStack()
    {
        if (typeof(T).IsClass)
            EqualsContents = delegate (T left, T right)
            {
                return Object.ReferenceEquals(left, right);
            };
        else
            EqualsContents = delegate (T left, T right)
            {
                return left != null && left.Equals(right);
            };
    }

    public class ChangbleStackException : Exception
    {
        public ChangbleStackException(string message)
        : base(message) { }
    }
}

public class RootChangebleStack<T> : IChangebleStack<T>
{
    public int Count { get { return PopCount + (Root != null ? 1 : 0); } }
    public int PopCount { get { return Stack.Count; } }
    ChangebleStack<T> Stack;
    public T Root { set; get; }
    private RootChangebleStack() { }
    public RootChangebleStack(T root)
    : base() { Root = root; Stack = new ChangebleStack<T>(); }

    public void Clear()
    {
        Stack.Clear();
    }

    public void Push(T obj)
    {
        if (Root == null) Root = obj;
        else Stack.Push(obj);
    }
    public bool Delete(T obj)
    {
        return Stack.Delete(obj);
    }
    public bool Contains(T obj)
    {
        if (obj == null) return false;
        return Stack.Contains(obj) || obj.Equals(Root);
    }
    public bool CurrentIsRoot()
    {
        return Stack.IsEmpty();
    }
    public T Pop()
    {
        if (Stack.IsEmpty()) throw new ChangebleStack<T>.ChangbleStackException("Stack dont have leafs");
        return Stack.Pop();
    }
    public T Peek()
    {
        if (Stack.IsEmpty()) throw new ChangebleStack<T>.ChangbleStackException("Stack dont have leafs");
        return Stack.Peek();
    }
    public T[] ToArray()
    {
        T[] stack = Stack.ToArray();
        if (Root == null) return stack;

        var result = new T[Count];
        result[0] = Root;
        for (int i = 1; i < result.Length; ++i)
            result[i] = stack[i - 1];
        return result;
    }
    public T Get()
    {
        if (Stack.IsEmpty()) return Root;
        return Stack.Peek();
    }
}