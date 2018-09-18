using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBombDaily : MonoBehaviour
{
    [SerializeField] private ProgressBar Bar;
    [SerializeField] private TextMesh Text;
    [SerializeField] private UnityEngine.Events.UnityEvent OnBomb;
    private bool wasBomb;
    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Bar == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Bar", name);
            enabled = false;
            return;
        }
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
    public void UpdateTime()
    {
        UpdateTime(DailyAds.TIME_ADS, DailyAds.Manager.GetLeftTime());
    }
    public void UpdateTime(int targetTime, int currentTime)
    {
        if (currentTime <= targetTime)
        {
            wasBomb = false;
            currentTime = targetTime - currentTime;
            Bar.Progress = 1 - (float)currentTime / (float)targetTime;

            if (currentTime >= 60)
                if (currentTime >= 3600) // 3600 = 60*60
                {
                    SetHours(Mathf.RoundToInt(currentTime / (float)3600));
                }
                else
                {
                    SetMinutes(Mathf.RoundToInt(currentTime / (float)60));
                }
            else
            {
                SetSeconds(currentTime);
            }

        }
        else
        {
            if (!wasBomb)
            {
                OnBomb.Invoke();
                Bar.Progress = 1;
                SetText("-");
                wasBomb = true;
            }
        }
    }

    private void SetMinutes(int minutes)
    {
        SetText(minutes + "<size=25>" + TranslationManager.GetText("UI_Minutes") + "</size>");
    }
    private void SetHours(int hours)
    {
        SetText(hours + "<size=25>" + TranslationManager.GetText("UI_Hours") + "</size>");
    }
    private void SetSeconds(int seconds)
    {
        SetText(seconds + "<size=25>" + TranslationManager.GetText("UI_Seconds") + "</size>");
    }

    private void SetText(string text)
    {
        Text.text = text;
    }

    private float left;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        left += Time.deltaTime;
        if (left >= 1)
        {
            left = 0;
            UpdateTime();
        }
    }
}
