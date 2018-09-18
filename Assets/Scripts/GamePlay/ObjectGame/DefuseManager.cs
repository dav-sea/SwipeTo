using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefuseManager : MonoBehaviour
{
    public float MinValueDefuse = 0.33f;

    public float MaxValueDefuse = 1;

    // public UnityEngine.Events.UnityEvent OnIntialize;

    [SerializeField] private bool SendEventChange = false;
    [SerializeField] private UnityEngine.Events.UnityEvent ChangeDefuse;

    public event System.Action EventChangeDefuse;

    [SerializeField] private bool SendEventFinish = true;
    [SerializeField] private UnityEngine.Events.UnityEvent FinishDefuse;

    public float DefuseFactor = 0.1f;

    public float UndefuseMaxFactor = 2f;

    public bool StopUndefuseWhenFullDefuse = true;

    private float _DefuseScore, _PreviousScore;
    public float UndefuseScore { set; get; }

    public static float UndefuseFactor = 3.5f;

    public float IntervalDefuse
    {
        get
        {
            return MaxValueDefuse - MinValueDefuse;
        }
    }

    // return scoped value at 0..1 	
    public float ScopeDefuse
    {
        set
        {
            _PreviousScore = _DefuseScore;
            _DefuseScore = value * IntervalDefuse + MinValueDefuse;
            if (_DefuseScore != _PreviousScore)
            {
                if (SendEventChange)
                    ChangeDefuse.Invoke();
                if (EventChangeDefuse != null)
                    EventChangeDefuse();
                if (_DefuseScore == MinValueDefuse)
                {
                    if (SendEventFinish)
                        FinishDefuse.Invoke();
                }
                else if (StopUndefuseWhenFullDefuse && _DefuseScore >= MaxValueDefuse)
                    UndefuseScore = 0;
            }
        }
        get
        {
            return (_DefuseScore - MinValueDefuse) / IntervalDefuse;
        }
    }

    public float DefuseScore
    {
        set
        {
            _PreviousScore = _DefuseScore;
            _DefuseScore = Mathf.Clamp(value, MinValueDefuse, MaxValueDefuse);
            if (_DefuseScore != _PreviousScore)
            {
                if (SendEventChange)
                    ChangeDefuse.Invoke();
                if (EventChangeDefuse != null)
                    EventChangeDefuse();
                if (_DefuseScore == MinValueDefuse)
                {
                    if (SendEventFinish)
                        FinishDefuse.Invoke();
                }
                else if (StopUndefuseWhenFullDefuse && _DefuseScore >= MaxValueDefuse)
                    UndefuseScore = 0;
            }
        }
        get { return _DefuseScore; }
    }

    public void Undefuse(float value)
    {
        UndefuseScore += value;
        if (UndefuseScore > DefuseFactor * UndefuseMaxFactor)
            UndefuseScore = DefuseFactor * UndefuseMaxFactor;
    }

    public void UndefuseMax()
    {
        UndefuseScore = MaxValueDefuse * 1.5f;
    }

    public bool ActiveDefuse
    {
        set { enabled = value; }
        get { return enabled; }
    }

    private float _diff;

    public bool Defusing = true;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Defusing)
        {
            _diff = UndefuseScore * Time.deltaTime * UndefuseFactor;
            UndefuseScore -= _diff;
            DefuseScore += _diff - (DefuseFactor * Time.deltaTime);
        }
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        // OnIntialize.Invoke();
    }

    void Awake()
    {
        Initialize();
    }
}
