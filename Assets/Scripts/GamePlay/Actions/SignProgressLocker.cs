using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignProgressLocker : MonoBehaviour
{

    public static SignProgressLocker Manager { private set; get; }

    public bool TransferLock;
    public bool SwipesLock;
    public bool MultiplierLock;
    public bool CoinsLock;
    public bool LifesLock;
    public bool LoseLock;
    public bool FreezeLock;
    public bool FullerLock;
    public bool QuestionLock;
    public bool QuestionRotatebleLock;

    public void SetFullerLock(bool value)
    {
        FullerLock = value;
    }
    public void SetQuestionLock(bool value)
    {
        QuestionLock = value;
    }
    public void SetQuestionRotatebleLock(bool value)
    {
        QuestionRotatebleLock = value;
    }
    public void SetTransferLock(bool value)
    {
        TransferLock = value;
    }

    public void SetSwipesLock(bool value)
    {
        SwipesLock = value;
    }
    public void SetMultiplierLock(bool value)
    {
        MultiplierLock = value;
    }
    public void SetCoinsLock(bool value)
    {
        CoinsLock = value;
    }
    public void SetLifesLock(bool value)
    {
        LifesLock = value;
    }
    public void SetLoseLock(bool value)
    {
        LoseLock = value;
    }
    public void SetFreezeLock(bool value)
    {
        FreezeLock = value;
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
}
