using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Pool<T> where T : Object
{
    public delegate void ObjectAction(T obj);

    public event ObjectAction OnCreate;
    public event ObjectAction OnPush;
    public event ObjectAction OnPop;
    public event ObjectAction OnDestroy;

    [SerializeField]
    protected Object Prefab;

    protected void InvokeOnCreate(T obj)
    {
        OnCreate(obj);
    }
    protected void InvokeOnDestroy(T obj)
    {
        OnDestroy(obj);
    }

    public Object GetPrefab()
    {
        return Prefab;
    }
    public void SetPrefab(Object prefab)
    {
        Prefab = prefab;
    }

    private Queue<T> StackObjects = new Queue<T>();

    public int Count { get { return StackObjects.Count; } }

    public static void RePushingEvent(Pool<T> pool)
    {
        if (pool == null) return;
        var objs = pool.GetObjects();
        for (int i = objs.Length - 1; i >= 0; --i)
            pool.OnPush(objs[i]);
    }
    public static void InsideDebuging(Pool<T> pool, string context)
    {
        if (pool == null) return;

        pool.OnCreate += delegate (T obj) { Debug.Log(context + "OnCreate.\nCount: " + pool.Count); };
        pool.OnPush += delegate (T obj) { Debug.Log(context + "OnPush.\nCount: " + pool.Count); };
        pool.OnPop += delegate (T obj) { Debug.Log(context + "OnPop.\nCount: " + pool.Count); };
        pool.OnDestroy += delegate (T obj) { Debug.Log(context + "OnDestroy.\nCount: " + pool.Count); };
    }
    public void Push(T obj)
    {
        StackObjects.Enqueue(obj);
        OnPush(obj);
    }
    public void Push(params T[] obj)
    {
        for (int i = obj.Length - 1; i >= 0; --i)
            Push(obj[i]);
    }
    public void Unload(int count)
    {
        count = Mathf.Clamp(count, 0, Count);

        for (int i = count; i > 0; --i)
            Destroy(Pop(), 0.5f * i);
    }
    public void Reduce(int target)
    {
        Unload(Count - target);
    }
    public void Target(int target)
    {
        int diff = Count - target;
        if (diff > 0) Unload(diff);
        else Reserve(-diff);
    }
    public void Fill(int target)
    {
        Reserve(Mathf.Clamp(target - Count, 0, target));
    }
    public void Reserve(int count)
    {
        for (int i = count; i > 0; --i)
            Push(Factory());
    }
    public T Pop()
    {
        T result = Count > 0 ? StackObjects.Dequeue() : Factory();
        OnPop(result);
        return result;
    }
    protected virtual T Factory()
    {
        T obj = (T)Object.Instantiate(Prefab);
        OnCreate(obj);
        return obj;
    }
    protected virtual void Destroy(T obj, float time = 0.5f)
    {
        OnDestroy(obj);
        Object.Destroy(obj, time);
    }
    private T[] GetObjects()
    {
        return StackObjects.ToArray();
    }

    private Pool()
    {
        ObjectAction EmptyAction = delegate (T obj) { };
        OnCreate += EmptyAction;
        OnPush += EmptyAction;
        OnPop += EmptyAction;
        OnDestroy += EmptyAction;
    }
    public Pool(Object prefab, int target = 1)
    : this()
    {
        SetPrefab(prefab);
        Target(target);
    }
    public Pool(Object prefab, params T[] obj)
    : this()
    {
        SetPrefab(prefab);
        Push(obj);
    }
}
[System.Serializable]
public class GameObjectPool : Pool<GameObject>
{
    public GameObjectPool(GameObject prefab, int target = 1)
    : base(prefab, target) { }
    public GameObjectPool(GameObject prefab, params GameObject[] obj)
    : base(prefab, obj) { }
}
[System.Serializable]
public class ComponentObjectPool<C> : Pool<C> where C : Component
{
    protected override C Factory()
    {
        C value = (Object.Instantiate(Prefab) as GameObject).GetComponent<C>();
        InvokeOnCreate(value);
        return value;
    }

    protected override void Destroy(C obj, float time = 0.5f)
    {
        InvokeOnDestroy(obj);
        Object.Destroy(obj.gameObject, time);
    }

    public ComponentObjectPool(GameObject prefab, int target = 1)
    : base(prefab, target) { }
    public ComponentObjectPool(GameObject prefab, params C[] obj)
    : base(prefab, obj) { }
}
