using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsHelper : MonoBehaviour
{
    private static PrefabsHelper instance;

    [SerializeField]
    private GameObject LifeObjectGamePrefab;
    [SerializeField]
    private GameObject MultiplierObjectPrefab;
    [SerializeField]
    private GameObject CoinObjectPrefab;
    [SerializeField]
    private GameObject FreezeObjectPrefab;
    [SerializeField]
    private GameObject ObjectGamePrefab;
    [SerializeField]
    private GameObject ObjectFullerPrefab;
    [SerializeField]
    private GameObject TrainingObject;

    public static GameObject PrefabLifeObject { get { return instance.LifeObjectGamePrefab; } }
    public static GameObject PrefabObjectGame { set { instance.SetObjectGamePrefab(value); } get { return instance.ObjectGamePrefab; } }
    public static GameObject PrefabMultiplierObject { get { return instance.MultiplierObjectPrefab; } }
    public static GameObject PrefabCoinObject { get { return instance.CoinObjectPrefab; } }
    public static GameObject PrefabFreezeObject { get { return instance.FreezeObjectPrefab; } }
    public static GameObject PrefabFullerObject { get { return instance.ObjectFullerPrefab; } }
    public static GameObject PrefabTrainingObject { get { return instance.TrainingObject; } }
    public void SetObjectGamePrefab(GameObject value)
    {
        ObjectGamePrefab = value;
        WorldEther.ChangePrefabObjectGame.Push(this, null);
    }


    [ContextMenu("Apply Prefabs")]
    public void ApplyPrefabs()
    {
        WorldEther.ApplyAllPrefabs.Push(this, null);
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

    }
    void Awake()
    {
        Initialize();
    }
}
