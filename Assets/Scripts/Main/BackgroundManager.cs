using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Transform TargetContenier;
    [SerializeField] private float GlobalOffsetZ = 650;

    public static BackgroundManager Manager { private set; get; }

    public Background CurrentBackground { private set; get; }


    public void SetBackground(Background background)
    {
        background.Initialize();
        if (CurrentBackground != null)
        {
            CurrentBackground.Hide();
            Destroy(CurrentBackground.gameObject);
        }
        CurrentBackground = background;
        var transf = CurrentBackground.transform;
        transf.parent = TargetContenier;
        transf.position = new Vector3(0, 0, GlobalOffsetZ);
        CurrentBackground.Show();
    }

    public void SetBackground(GameObject prefab)
    {
        SetBackground(Instantiate(prefab).GetComponent<Background>());
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
        if (TargetContenier == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TargetContenier", name);
        }
        Manager = this;
    }
    void Awake()
    {
        Initialize();
    }
}