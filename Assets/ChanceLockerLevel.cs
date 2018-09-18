using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceLockerLevel : MonoBehaviour
{
    [SerializeField] private GamePlayCore TargetCore;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TargetCore == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TargetCore", name);
            enabled = false;
            return;
        }
    }

    public void UpdateLock()
    {
        var locker = SignProgressLocker.Manager;
        var data = TargetCore.GetData();
        // if (locker.SwipesLock) data.ChanceMultiSwipes.Value = 0; else data.ChanceMultiSwipes.Reset();

    }

    void Awake()
    {
        Initialize();
    }
}
