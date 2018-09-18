using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PoolObjects<C> where C : Object
{
    #region Fields

    private Stack<C> QueueObjects = new Stack<C>();
    public int Count { get { return QueueObjects.Count; } }

    public event ActionHandler EnqueueObject;
    public event ActionHandler DequeueObject;


    #endregion

    public PoolObjects()
    {
        EnqueueObject += delegate (C obj) { };
        DequeueObject += delegate (C obj) { };
    }

    public void AddObject(C obj)
    {
        Enqueue(obj);
    }

    public void DeleteObject(C obj)
    {
        var list = new List<C>(QueueObjects.ToArray());
        if (!list.Remove(obj))
            return;
        QueueObjects.Clear();
        QueueObjects = new Stack<C>(list.Count);

        for (int i = list.Count; i >= 0; --i)
            QueueObjects.Push(list[i]);

    }

    public void Enqueue(C obj)
    {
        EnqueueObject(obj);
        QueueObjects.Push(obj);
    }

    public C Dequeue()
    {
        if (Count == 0) return null;
        var obj = QueueObjects.Pop();
        DequeueObject(obj);
        return obj;
    }

    public delegate void ActionHandler(C obj);
}
