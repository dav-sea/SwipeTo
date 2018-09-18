using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContainer : MonoBehaviour
{
    public static AudioContainer Manager { private set; get; }

    [SerializeField] private SoundController _MainTheme;
    [SerializeField] private SoundController[] _GameplayTracks;
    [Space(5)]
    [SerializeField]
    private SoundController _DefaultStandSound;
    [SerializeField]
    private SoundController _DefaultArrowSound;
    [SerializeField]
    private SoundController _DefaultTransferSound;
    [SerializeField]
    private SoundController _DefaultOneSwipeSound;
    [SerializeField]
    private SoundController _DefaultClocksSound;
    [SerializeField]
    private SoundController _DefaultQuestionSound;
    [SerializeField]
    private SoundController _CriticalDefuseLoop;

    public SoundController MainTheme { get { return _MainTheme; } }
    public SoundController[] GameplayTracks { get { return _GameplayTracks; } }
    public SoundController DefaultStandSound { get { return _DefaultStandSound; } }
    public SoundController DefaultArrowSound { get { return _DefaultArrowSound; } }

    public SoundController DefaultTransferSound { get { return _DefaultTransferSound; } }
    public SoundController DefaultOneSwipeSound { get { return _DefaultOneSwipeSound; } }
    public SoundController DefaultClocksSound { get { return _DefaultClocksSound; } }
    public SoundController DefaultQuestionSound { get { return _DefaultQuestionSound; } }
    public SoundController CriticalDefuseLoop { get { return _CriticalDefuseLoop; } }

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
        /* COMPONENTS CHECK NULL */
        // #if UNITY_EDITOR
        if (_MainTheme == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "MainTheme", name);
            enabled = false;
            return;
        }
        if (_DefaultClocksSound == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefaultClocksSound", name);
            enabled = false;
            return;
        }
        if (_DefaultStandSound == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefaultStandSound", name);
            enabled = false;
            return;
        }
        if (_DefaultQuestionSound == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefaultQuestionSound", name);
            enabled = false;
            return;
        }
        if (_CriticalDefuseLoop == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "CriticalDefuseLoop", name);
            enabled = false;
            return;
        }
        if (_DefaultArrowSound == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefaultArrowSound", name);
            enabled = false;
            return;
        }
        if (_DefaultOneSwipeSound == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefaultOneSwipeSound", name);
            enabled = false;
            return;
        }
        if (_DefaultTransferSound == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "DefaultTransferSound", name);
            enabled = false;
            return;
        }
        // #endif
        /********************************************* */
        Manager = this;
    }

    void Awake()
    {
        Initialize();
    }


}