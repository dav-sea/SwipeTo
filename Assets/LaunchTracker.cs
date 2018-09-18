using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaunchTracker : MonoBehaviour
{
    public const int SECONDS_IN_DAY = 86400;//24 * 60 * 60
    private const string KEY_PREVIOUS = "PDATE";
    public static LaunchTracker Manager { private set; get; }
    public static TimeSpan TimeBetweenStarts { get { return DateTime.Now - PreviousLaunch; } }
    public static TimeSpan ActiveTime { get { return DateTime.Now - LaunchDate; } }
    public static DateTime LaunchDate { private set; get; }
    public static DateTime PreviousLaunch { private set; get; }
    public static DateTime Now { get { return DateTime.Now; } }
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
        LaunchDate = DateTime.Now;
        PreviousLaunch = LoadDate(KEY_PREVIOUS);
        SaveDate(KEY_PREVIOUS, LaunchDate);

        Debug.Log("Previous launch: " + PreviousLaunch + "\nBetweenStarts :" + TimeBetweenStarts);
        Debug.Log("Version: " + Application.version);
    }

    public static void SaveDate(string key, DateTime date)
    {
        PlayerPrefs.SetInt(key + "year", date.Year);
        PlayerPrefs.SetInt(key + "month", date.Month);
        PlayerPrefs.SetInt(key + "day", date.Day);
        PlayerPrefs.SetInt(key + "hour", date.Hour);
        PlayerPrefs.SetInt(key + "minute", date.Minute);
        PlayerPrefs.SetInt(key + "second", date.Second);
        PlayerPrefs.Save();
    }

    public static DateTime LoadDate(string key)
    {
        return new DateTime(
            PlayerPrefs.GetInt(key + "year", DateTime.Now.Year),
            PlayerPrefs.GetInt(key + "month", DateTime.Now.Month),
            PlayerPrefs.GetInt(key + "day", DateTime.Now.Day),
            PlayerPrefs.GetInt(key + "hour", DateTime.Now.Hour),
            PlayerPrefs.GetInt(key + "minute", DateTime.Now.Minute),
            PlayerPrefs.GetInt(key + "second", DateTime.Now.Second)
        );
    }

    void Awake()
    {
        Initialize();
    }
}
