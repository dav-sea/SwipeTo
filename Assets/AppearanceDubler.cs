using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIOrganization;

public class AppearanceDubler : MonoBehaviour
{
    [SerializeField] private Appearance Target;
    [SerializeField] private Leaf[] Doubles;

    public bool ActiveModule = true;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Target == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Target", name);
            enabled = false;
            return;
        }
        Target.EventShow += delegate
        {
            if (ActiveModule) foreach (Leaf item in Doubles) item.OnShow();
        };
        Target.EventHide += delegate
        {
            if (ActiveModule) foreach (Leaf item in Doubles) item.OnHide();
        };
    }
    void Awake()
    {
        Initialize();
    }

    [System.Serializable]
    public class Leaf
    {
        [SerializeField] private bool ShowDuble = true;
        [SerializeField] private bool HideDuble = true;
        [SerializeField] private Appearance Target;

        public void OnShow()
        {
            if (ShowDuble) Target.Show();
        }

        public void OnHide()
        {
            if (HideDuble) Target.Hide();
        }
    }
}
