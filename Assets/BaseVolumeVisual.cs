using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseVolumeVisual : MonoBehaviour
{
    [SerializeField] Appearance[] Segments;
    [SerializeField] TextMesh Text;

    public int SegmentsCount { get { return Segments.Length; } }

    private void SetVisual(float volume)
    {
        int i;
        for (i = 0; i < ((float)Segments.Length * volume); ++i)
        {
            Segments[i].Show();
        }
        for (; i < Segments.Length; ++i)
        {
            Segments[i].Hide();
        }
        Text.text = "" + Mathf.RoundToInt(volume * 100);
    }

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
    void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        WorldEther.ChnageVolume.Subscribe(delegate (Ethers.Channel.Info inf) { UpdateVisual(); });
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        SetVisual(AudioManager.BaseVolume);
    }

}
