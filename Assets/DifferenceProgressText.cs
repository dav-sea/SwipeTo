using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferenceProgressText : MonoBehaviour
{
    [SerializeField] TextMesh Text;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Text == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Text", name);
            enabled = false;
            return;
        }
    }

    public void UpdateViewer()
    {
        Text.text = "" + (ProgressLevels.Manager.NextLevelToTargetScore() - PlayerProgress.Manager.ProgressScore);
    }

    void Awake()
    {
        Initialize();
    }
}
