using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidesPool : MonoBehaviour
{

    [SerializeField]
    private GameObject PrefabSide;

    [SerializeField]
    private Transform Parent;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (PrefabSide == null || PrefabSide.GetComponent<Side>() == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null or not have Side component", "PrefabSide", name);
            enabled = false;
            return;
        }
    }
    void Awake()
    {
        Initialize();
    }

    public Side Get()
    {
        var res = Instantiate(PrefabSide, Vector3.zero, Quaternion.identity, Parent != null ? Parent : transform).GetComponent<Side>();
        res.gameObject.SetActive(true);
        return res;
    }
}
