using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializator : MonoBehaviour
{

    [SerializeField]
    GamePlayContenier Contenier;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Contenier == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Contenier", name);
            enabled = false;
            return;
        }
        Contenier.Initialize();
        // Resources.Load("");
    }
    void Awake()
    {
        Initialize();
    }
}
