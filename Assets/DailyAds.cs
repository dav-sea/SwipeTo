using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyAds : MonoBehaviour
{
    public static DailyAds Manager { private set; get; }
    private const string KEY_FLASHDATE = "FDATE";

    public const int TIME_ADS = LaunchTracker.SECONDS_IN_DAY / 96;

    System.DateTime FlashDate;

    public int GetLeftTime()
    {
        return Mathf.RoundToInt((float)(LaunchTracker.Now - FlashDate).TotalSeconds);
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
        Manager = this;
        FlashDate = LaunchTracker.LoadDate(KEY_FLASHDATE);
        LaunchTracker.SaveDate(KEY_FLASHDATE, FlashDate);
    }

    void Awake()
    {
        Initialize();
    }

    public bool IsReady()
    {
        return GetLeftTime() >= TIME_ADS;
    }

    public void Flash()
    {
        FlashDate = LaunchTracker.Now;
        SaveFlashDate();
    }

    protected void SaveFlashDate()
    {
        LaunchTracker.SaveDate(KEY_FLASHDATE, FlashDate);
    }

#if UNITY_EDITOR
    [ContextMenu("Add 1h")]
    private void Editor_add1h()
    {
        FlashDate -= new System.TimeSpan(1, 0, 0);
        SaveFlashDate();

    }
    [ContextMenu("Add 50m")]
    private void Editor_add50m()
    {
        FlashDate -= new System.TimeSpan(0, 50, 0);
        SaveFlashDate();
    }
    [ContextMenu("Add 1m")]
    private void Editor_add1m()
    {
        FlashDate -= new System.TimeSpan(0, 1, 0);
        SaveFlashDate();
    }

    [ContextMenu("Back 1m")]
    private void Editor_back1m()
    {
        FlashDate += new System.TimeSpan(0, 1, 0);
        SaveFlashDate();
    }

    [ContextMenu("ToFinish")]
    private void Editor_ready1m()
    {
        FlashDate = LaunchTracker.Now - new System.TimeSpan(0, 0, TIME_ADS - 60);
        SaveFlashDate();
    }


    [ContextMenu("Flash")]
    private void Editor_flash()
    {
        Flash();
    }

#endif
}
