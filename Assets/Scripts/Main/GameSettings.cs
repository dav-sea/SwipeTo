using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Manager { private set; get; }

    public const string KEY_EFFECTS = "SEFFECTS";
    public const string KEY_ANTIALIS = "SANTIALIS";

    private QualitySettings Effects;
    private QualitySettings Antialis;

    public void SaveEffects()
    {
        // PlayerPrefs.SetInt(KEY_EFFECTS, QualitySettingsToInt(Effects));
    }

    public void SaveAntialis()
    {
        // PlayerPrefs.SetInt(KEY_ANTIALIS, QualitySettingsToInt(Antialis));
    }

    public void Save()
    {
        // SaveEffects();
        // SaveAntialis();
    }

    public void Load()
    {
        Effects = QualitySettings.Max;
        Antialis = QualitySettings.Avg;

        // if (Antialis == QualitySettings.Low) UnityEngine.QualitySettings.antiAliasing = 0;
        // else if (Antialis == QualitySettings.Avg) UnityEngine.QualitySettings.antiAliasing = 2;
        // else UnityEngine.QualitySettings.antiAliasing = 4;
    }

    public QualitySettings GetEffects()
    {
        return Effects;
    }

    public QualitySettings GetAntialis()
    {
        return Antialis;
    }

    public static int QualitySettingsToInt(QualitySettings quality)
    {
        return quality == QualitySettings.Max ? 1 : quality == QualitySettings.Avg ? 0 : -1;
    }

    public static QualitySettings IntToQualitySettings(int quality)
    {
        return quality >= 1 ? QualitySettings.Max : quality == 0 ? QualitySettings.Avg : QualitySettings.Low;
    }

    public void SetEffects(QualitySettings quality)
    {
        // Effects = quality;
        // SaveEffects();
        // UpdateSettings();
    }

    public void SetAntialis(QualitySettings quality)
    {
        // Antialis = quality;
        // if (quality == QualitySettings.Low) UnityEngine.QualitySettings.antiAliasing = 0;
        // else if (quality == QualitySettings.Avg) UnityEngine.QualitySettings.antiAliasing = 2;
        // else UnityEngine.QualitySettings.antiAliasing = 4;
        // SaveAntialis();
        // UpdateSettings();
    }

    public void UpdateSettings()
    {
        // WorldEther.ChangeGameSettings.Push(null, null);
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
        // Load();
        UnityEngine.QualitySettings.antiAliasing = 1;
    }
    void Awake()
    {
        Initialize();
    }

    public enum QualitySettings { Low, Avg, Max }
}