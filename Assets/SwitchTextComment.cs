using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTextComment : MonoBehaviour
{
    public string[] Commnets;
    public string Prefix;
    [SerializeField] private SwitcherObject Switcher;
    [SerializeField] private TextMesh Text;
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Switcher == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Switcher", name);
            enabled = false;
            return;
        }
        Switcher.Initialize();
        Switcher.EventChangeSwitchPosition += Update;
    }
    void Awake()
    {
        Initialize();
    }

    private int _prev = -1;
    public void Update()
    {
        int current = Switcher.SwitchPosition;
        if (_prev != current)
        {
            _prev = current;
            if (current < Commnets.Length)
            {
                Text.text = Prefix + Commnets[current];
            }
            else Text.text = "";
        }
    }

    [ContextMenu("Sync")]
    private void Sync()
    {
        Commnets = new string[Switcher.CountPositions];
    }
}
