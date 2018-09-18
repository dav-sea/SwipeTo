using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModeSelector : MonoBehaviour
{
    public GamePlayCore CorePrefab;

    [SerializeField]
    private bool StartActiveContenier = false;

    public void SelectMode()
    {
        Initialize();

        GamePlayContenier.Active = StartActiveContenier;
        GamePlayContenier.GamePlayCore = Instantiate(CorePrefab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<GamePlayCore>();
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (CorePrefab == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "CorePrefab", name);
            enabled = false;
            return;
        }
    }
}
