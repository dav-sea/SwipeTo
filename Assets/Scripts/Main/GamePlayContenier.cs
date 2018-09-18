using UnityEngine;

public class GamePlayContenier : MonoBehaviour
{
    private static GamePlayContenier contenier;

    [SerializeField]
    private Appearance TrainingReducingCube;

    [SerializeField]
    private bool DestroyPreviousCore;

    [SerializeField]
    private bool InitializeNewCore;

    [SerializeField]
    private Transform CoreContenier;

    private Transform _transform;

    // [SerializeField]
    // private Score ScoreManager;

    // [SerializeField]
    // private Lifes Lifes;

    public void Resume()
    {
        GamePlayContenier.ResumeGamePlay(this);
        // GetScoreManager().enabled = true;
    }

    public void DisposeCore()
    {
        if (_core != null) _core.Dispose();
    }

    public void WaitFirstAction()
    {
        if (_core != null) _core.WaitFirstAction();
    }
    public void ResetDefaultSystems()
    {
        Lifes.LifesManager.CountLifes = 0;

        Score.ScoreManager.CurrentScore = 0;
        Score.ScoreManager.Multiplier = 1;
        Score.ScoreManager.MultiplierTime = 0;
    }

    // public void SetVisible(bool value)
    // {
    //     GamePlayContenier.SetVisibleCore(value);
    // }

    public void ContinueCore()
    {
        if (_core != null)
        {
            _core.ContinueCore();
        }
    }

    public void Pause()
    {
        GamePlayContenier.PauseGamePlay(this);
        // GetScoreManager().enabled = false;
    }

    public void RestartGamePlayCore()
    {
        GamePlayContenier.Restart(this);
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (CoreContenier == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "CoreContenier", name);
            enabled = false;
            return;
        }
        if (TrainingReducingCube == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TrainingReducingCube", name);
            enabled = false;
            return;
        }

        _transform = transform;

        if (contenier == null)
            contenier = this;
    }

    public static bool ActiveTrainingReducingCube
    {
        set { contenier.TrainingReducingCubeActive = value; }
        get { return contenier.TrainingReducingCubeActive; }
    }

    public bool TrainingReducingCubeActive
    {
        set { TrainingReducingCube.IsAppearance = value; }
        get { return TrainingReducingCube.IsAppearance; }
    }

    void Awake()
    {
        Initialize();
    }

    public void FlashCore()
    {
        GamePlayCore = null;
    }

    public void SetContinueCore(bool value)
    {
        // if (_core != null) _core.Continuable = value;
    }

    private static GamePlayCore _core;
    public static GamePlayCore GamePlayCore
    {
        set
        {
            ResetGameSystems();
            if (_core != null)
            {
                _core.OnUnselect();
                if (contenier.DestroyPreviousCore)
                    Destroy(_core.gameObject, 1);
                _core.gameObject.SetActive(false);
            }
            _core = value;
            if (_core != null)
            {
                _core.OnSelect();
                if (contenier.InitializeNewCore)
                {
                    _core.Initialize();

                    var transf = _core.transform;
                    transf.parent = contenier.CoreContenier;
                    transf.localPosition = Vector3.zero;
                    transf.localRotation = Quaternion.identity;
                    Restart(_core);
                }
            }
        }
        get
        {
            return _core;
        }
    }

    // public static void SetContinue(bool value)
    // {

    // }
    public static void CoreDispose()
    {
        if (_core != null) _core.Dispose();
    }
    public static void CoreContinue()
    {
        contenier.ContinueCore();
    }

    public static void ResumeGamePlay(object source)
    {
        if (_core != null) _core.OnResume();
        WorldEther.ResumeGame.Push(source, null);
    }


    public static void ResetGameSystems()
    {
        contenier.ResetDefaultSystems();
    }

    public static void PauseGamePlay(object source)
    {
        if (_core != null) _core.OnPause();
        WorldEther.PauseGame.Push(source, null);
    }

    public static void Restart(object source)
    {
        if (_core != null) _core.OnRestart();
        WorldEther.RestartGame.Push(source, null);
    }

    public static Transform Transform
    {
        get
        {
            return contenier._transform;
        }
    }

    public static bool Active
    {
        set
        {
            contenier.gameObject.SetActive(value);
        }
        get
        {
            return contenier.gameObject.activeSelf;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("FlashBest")]
    private void Editor_FlashBest()
    {
        if (_core != null)
            _core.SetBestResult(0);
        else Debug.Log("Core is null");
    }
#endif

    // public static void ActivateLoseScreen()
    // {
    //     contenier.LoseScreen.Show();
    // }
}