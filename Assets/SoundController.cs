using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource Target;
    [SerializeField] private bool PauseOnStop = true;
    [SerializeField] private float TimeRevolume = 2;
    [SerializeField] private bool StopOnPlay = false;

    [Range(0, 1)] [SerializeField] private float MultiplierVolume = 1;

    private RelativeMotion.RelativeMotionEngine<float> VolumeChanger = new RelativeMotion.RelativeMotionEngine<float>(RelativeMotion.RelatveMotionWorkers.CurrentWorkerFloat);



    public void Play()
    {
        if (StopOnPlay) Target.Stop();
        SoftVolume = 1;
    }
    public void Stop()
    {
        ForceVolume = 0;
    }

    public AudioSource GetTarget()
    {
        return Target;
    }

    public float SoftVolume
    {
        set { SetRelativeMotionVolume(_volume, Mathf.Clamp(value, 0, 1), TimeRevolume); }
        get { return VolumeChanger.Target; }
    }

    private float _volume;
    public float ForceVolume
    {
        set
        {
            _volume = value;

            Target.volume = value * AudioManager.BaseVolume * MultiplierVolume;

            if (!Target.enabled)
                Target.enabled = true;

            if (!Target.gameObject.activeSelf) return;

            if (Target.volume == 0)
            {
                if (PauseOnStop) Target.Pause();
                else Target.Stop();
            }
            else if (!Target.isPlaying)
                Target.Play();

        }
        get { return _volume; }
    }

    public float Volume { get { return _volume; } }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        WorldEther.ChnageVolume.Subscribe(delegate (Ethers.Channel.Info inf)
        {
            Target.volume = _volume * AudioManager.BaseVolume * MultiplierVolume;
        });
    }
    private void SetRelativeMotionVolume(float start, float target, float time)
    {
        VolumeChanger.Start = start;
        VolumeChanger.Target = target;
        VolumeChanger.Time = new RelativeMotion.TimeMotion();
        VolumeChanger.TimeScale = 1 / time;
        if (VolumeChanger.Condition == null)
            VolumeChanger.Condition = new RelativeMotion.CurveCondition();
        VolumeChanger.Active = true;
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
    }
    void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (VolumeChanger.Update(Time.deltaTime))
            ForceVolume = VolumeChanger.GetCurrent();
    }

#if UNITY_EDITOR
    [ContextMenu("Find")]
    private void Editor_FindAudioSource()
    {
        Target = GetComponent<AudioSource>();
    }

    [ContextMenu("Play")]
    private void Editor_ForcePlay()
    {
        Target.Play();
    }
#endif
}
