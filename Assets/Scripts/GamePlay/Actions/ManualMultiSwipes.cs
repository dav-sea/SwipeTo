using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scaler = UIOrganization.ScalePosition;

public class ManualMultiSwipes : ManualArrow
{

    #region OUT SCRIPTS
    //====================================================================================//
    // [SerializeField]
    // private TextMesh TextMesh;
    public override string TranslationTraining
    {
        get { return "T_Swipes"; }
    }


    [SerializeField]
    private UnityEngine.UI.Text Text;
    // [SerializeField]
    // private Appearance TextAppearance;
    [SerializeField]
    private TargetFollowScript TextFollowScript;
    [SerializeField]
    private Transform TargetOpenText;
    //====================================================================================//
    #endregion

    public const int SwipesID = 7;

    #region  PRIVATE VARS
    //====================================================================================//
    private int _TargetCountSwipes;
    private int CountSwipes = 0;
    //====================================================================================//
    #endregion

    #region PUBLIC FIELDS
    //====================================================================================//
    public int TargetCountSwipes
    {
        set
        {
            if (Text != null && value != _TargetCountSwipes)
            {
                Text.text = (value - CountSwipes).ToString();
            }
            _TargetCountSwipes = value;
        }
        get
        {
            return _TargetCountSwipes;
        }
    }
    public static int ScoreSwipe = 1;
    private static float UndefuseSwipe = 0.11f;
    public static float AngleFacor = 0.9f;
    //====================================================================================//
    #endregion

    public override int ID { get { return SwipesID; } }

    #region  PUBLIC METHODS
    //====================================================================================//
    public override void Hide()
    {
        base.Hide();
        if (Text != null)
            Text.gameObject.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        if (Text != null)
            Text.gameObject.SetActive(true);
    }
    // public void UpdateTextColor()
    // {
    //     Text.color = Palette.PaletteManager.PaletteConfiguration.GetLoseColor();
    // }

    //====================================================================================//
    #endregion

    Vector3 DefaultTextPosition;
    Vector3 DefaultTextScale;

    #region OTHERS
    protected override bool OnInitialize()
    {
        if (!base.OnInitialize()) { return false; }

        if (Text == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TextMesh", name);
            enabled = false;
            return false;
        }
        if (TextFollowScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TextFollowScript", name);
            enabled = false;
            return false;
        }


        if (TargetOpenText == null)
        {
            //Debug.LogWarningFormat("{0} (in {1}) is null", "TargetOpenText", name);
            TargetOpenText = new GameObject("TargetOpenText").transform;
            TargetOpenText.parent = Transform;
        }

        TextTransform = TextFollowScript.GetComponent<RectTransform>();

        DefaultTextPosition = TextTransform.localPosition;
        DefaultTextScale = TextTransform.localScale;

        TextFollowScript.Initialize();
        TextFollowScript.FilterDifference.Active = true;
        var acc = TextFollowScript.Accelerate;
        acc.AccelerateValue = 4.5f;
        acc.Active = true;

        // WorldEther.ChangePalette.Subscribe(HandlerChangePalette);

        TargetOpenText.parent = Transform;

        return true;
    }

    RectTransform TextTransform;

    // public override bool TrainingActive
    // {
    //     set { base.TrainingActive = value; Text.gameObject.SetActive(!value); }
    //     get { return base.TrainingActive; }
    // }

    // public void HandlerChangePalette(Ethers.Channel.Info info)
    // {
    //     UpdateTextColor();
    // }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    // void OnDestroy()
    // {
    //     WorldEther.ChangePalette.Unsubscribe(HandlerChangePalette);
    // }

    protected virtual void PlayOneSwipeSoundEffect()
    {
        AudioContainer.Manager.DefaultOneSwipeSound.Play();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // UpdateTextColor();
    }
    public override bool IsRotater()
    {
        return true;
    }

    public override bool IsSwipeble()
    {
        return true;
    }
    private bool wasCall;
    private float summAngle;
    private bool startDragAnimation;

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        Text.gameObject.SetActive(false);
    }

    private void OpenText()
    {
        TextFollowScript.enabled = true;
        TargetOpenText.parent = Diraction.Parent.GetObjectGame().transform;
        TextTransform.SetParent(TargetOpenText.parent);
        TextTransform.localScale = DefaultTextScale;
        TargetOpenText.position = Scaler.PositionMainCameraScale(Vector3.one * 0.5f, 20);
        TextFollowScript.SetTarget(TargetOpenText);
    }
    // private void CloseText()
    // {
    //     TextFollowScript.transform.parent = transform;
    //     TargetOpenText.localPosition = DefaultTextPosition;
    //     TextFollowScript.SetTarget(TargetOpenText);
    // }

    protected override bool OnSwipe(Side.Diraction diraction)
    {
        if (Diraction == diraction)
        {
            // Debug.Log("diraction");
            if (!wasCall)
            {
                // Debug.Log("first call");
                wasCall = true;
                var side = diraction.Parent;

                if (side.Up != Diraction) { side.HideActionComponents(side.Up, true); }
                if (side.Down != Diraction) { side.HideActionComponents(side.Down, true); }
                if (side.Left != Diraction) { side.HideActionComponents(side.Left, true); }
                if (side.Right != Diraction) { side.HideActionComponents(side.Right, true); }

                startDragAnimation = side.GetObjectGame().GetTouchAnimationController().DragAnimation;
                side.GetObjectGame().GetTouchAnimationController().DragAnimation = false;

                OpenText();



            }

            ++CountSwipes;

            float diff = (TargetCountSwipes != 0) ? (90 * AngleFacor) / TargetCountSwipes : 0;
            summAngle += diff;

            diraction.GetRotateFunction().Invoke(diff);
            if (Text != null)
                Text.text = (TargetCountSwipes - CountSwipes).ToString();
            if (CountSwipes >= TargetCountSwipes)
            {
                diraction.GetRotateFunction().Invoke(-summAngle);
                if (TrainingActive) TrainingCount -= TargetCountSwipes - 1;
                // Debug.Log("" + TrainingCount);
                ArrowManual(diraction);
                KitManual();
                diraction.Parent.GetObjectGame().GetTouchAnimationController().DragAnimation = startDragAnimation;
                TextTransform.SetParent(Transform);
            }
            else
            {
                PlayOneSwipeSoundEffect();
                UpScore(ScoreSwipe);
                Undefuse(UndefuseSwipe);
            }
            return true;
        }
        return false;
    }

    public override void ResetAction()
    {
        base.ResetAction();
        TextTransform.localScale = DefaultTextScale;
        TargetOpenText.parent = Transform;
        TextTransform.SetParent(TargetOpenText.parent);
        TargetOpenText.localPosition = DefaultTextPosition;
        TextFollowScript.enabled = false;
        TextFollowScript.SetForcePosition(TargetOpenText.position);
        // Debug.Log("reset");
        CountSwipes = 0;
        _TargetCountSwipes = -1;
        if (GamePlayContenier.GamePlayCore != null)
            TargetCountSwipes = (int)Mathf.Round(GamePlayContenier.GamePlayCore.GetData().SwipesRandomCount.Value);
        else TargetCountSwipes = 2;
        // TargetCountSwipes = (int)Mathf.Round(Diraction.Parent.GetObjectGame().GetActionManager().GetData().SwipesRandomCount.Value);
        summAngle = 0;
        wasCall = false;
        Text.gameObject.SetActive(true);
    }
    #endregion
}