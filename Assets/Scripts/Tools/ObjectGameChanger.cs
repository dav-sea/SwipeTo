using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ObjectGameChanger : MonoBehaviour
{
    public static ObjectGameChanger Manager { private set; get; }

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
    void Start()
    {
        Initialize();
    }

    public void SetObjectGamePrefab(GameObject objectGame)
    {
        PrefabsHelper.PrefabObjectGame = objectGame;
    }
}