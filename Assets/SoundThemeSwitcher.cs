using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundThemeSwitcher : MonoBehaviour
{
    [SerializeField] Appearance Target;
    [SerializeField] Themes Collection = Themes.None;
    [SerializeField] [Range(1, 10)] int NumberInCollection = 1;
    [Space(3)]
    [SerializeField]
    [Range(0, 100)]
    float Volume = 100;

    private static float _volume;
    public static float ThemeVolume { set { _volume = value; } get { return _volume; } }
    private static Themes _Current = Themes.None;
    private static int _NumberInCollection = 0;
    private static SoundController SoundCurrent;
    public static Themes CurrentTheme
    {
        set
        {
            if (_Current != value)
            {
                // Debug.Log("onsw " + value);
                // SetGameplayTracksVolume(0);
                if (SoundCurrent != null) SoundCurrent.SoftVolume = 0;
                _Current = value;
                switch (value)
                {
                    case Themes.RandomGameplay:
                        _NumberInCollection = Random.Range(0, AudioContainer.Manager.GameplayTracks.Length);
                        goto case Themes.Gameplay;
                    case Themes.Menu:
                        SoundCurrent = NumberMenuCollectionToSound(_NumberInCollection);
                        break;
                    case Themes.Gameplay:
                        SoundCurrent = NumberGameplayCollectionToSound(_NumberInCollection);
                        break;
                    case Themes.NotChange: break;
                    default: return;
                }
            }
            if (SoundCurrent != null) SoundCurrent.SoftVolume = ThemeVolume;
        }
        get { return _Current; }
    }

    private static SoundController NumberMenuCollectionToSound(int number)
    {
        return AudioContainer.Manager.MainTheme;
    }
    private static SoundController NumberGameplayCollectionToSound(int number)
    {
        return AudioContainer.Manager.GameplayTracks[Mathf.Clamp(number, 0, AudioContainer.Manager.GameplayTracks.Length)];
    }

    private static void SetGameplayTracksVolume(float value)
    {
        var sources = AudioContainer.Manager.GameplayTracks;
        foreach (SoundController sound in sources)
            if (sound.Volume != 0 || sound.SoftVolume != 0)
                sound.SoftVolume = 0;
    }

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
        Target.EventShow += delegate { SwitchSoundTheme(); };
    }

    void Awake()
    {
        Initialize();
    }

    public void SwitchSoundTheme()
    {
        ThemeVolume = Volume / 100;
        // Debug.Log("call " + CurrentTheme);

        if (Collection != Themes.NotChange)
        {
            _NumberInCollection = NumberInCollection - 1;
            CurrentTheme = Collection;
            // Debug.Log("" + _Current);
        }
        else
        {
            SoundCurrent.SoftVolume = ThemeVolume;
        }
    }

    public enum Themes
    {
        Menu = 3,
        Gameplay = 2,
        RandomGameplay = 100,
        NotChange = 1,
        None = 0

    }

#if UNITY_EDITOR
    [ContextMenu("Find")]
    private void Editor_FindTarget()
    {
        Target = GetComponent<Appearance>();
    }
#endif
}
