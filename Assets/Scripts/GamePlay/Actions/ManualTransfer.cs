using UnityEngine;

public class ManualTransfer : ManualKit
{
    private static int _TrainingCount;
    public override int TrainingCount { set { _TrainingCount = value; } get { return _TrainingCount; } }
    public override string TranslationTraining
    {
        get { return "T_Transfer"; }
    }
    #region OUT SCRIPTS
    //====================================================================================//
    [Space(3)]
    [Header("Transfer Settings")]
    [Space(1)]
    [Header("Scripts")]
    // [SerializeField]
    // private Animation Animation;
    [SerializeField]
    private TransfusionScript TransfusionScript;
    [SerializeField]
    private Transform ModelTransform;
    //====================================================================================//
    #endregion

    public const int TransferID = 6;

    #region PUBLIC METHODS
    //====================================================================================//
    public void UpdateColors()
    {
        TransfusionScript.SetTransfusion(Palette.PaletteManager.PaletteConfiguration.GetNormalColor(), Palette.PaletteManager.PaletteConfiguration.GetLoseColor());
    }

    public override int ID { get { return TransferID; } }
    public override void ResetAction()
    {
        base.ResetAction();
        wasUsed = false;
        TransfusionScript.enabled = false;
        TransfusionScript.ResetStep();
        ModelTransform.localScale = Vector3.one;
        ModelTransform.localPosition = Vector3.zero;
    }

    protected override void InitializeTraining()
    {
        Training.ParticlesInverse = true;
    }

    public override bool WillRotate(Side.Diraction diraction)
    {
        return IsActiveAction && diraction == Diraction.Parent.GetInverseDiraction(Diraction);
    }
    //====================================================================================//
    #endregion
    public override bool IsRotater()
    {
        return true;
    }

    public override bool IsSwipeble()
    {
        return true;
    }
    bool wasUsed;
    protected override bool OnSwipe(Side.Diraction diraction)
    {
        var myRot = Diraction.LocalRotation.eulerAngles.z + 180;
        if (myRot >= 360) myRot -= 360;

        if (!wasUsed && myRot == diraction.LocalRotation.eulerAngles.z)
        {
            PlaySoundEffect();
            wasUsed = true;
            Diraction.GetRotateFunction().Invoke(-90);
            Diraction.Parent.RemoveActionComponent(this);

            var objectGame = Diraction.Parent.GetObjectGame();//Ссылка на корневой ObjectGame
            var newSide = objectGame.CreateSide();//Ссылка на новую сторону
            var side = objectGame.FrontSide;//Ссылка на предыдущую сторону
            objectGame.SetupFrontSide(newSide, newSide.GetInverseDiraction(newSide.GetEqualsDiraction(Diraction)), false);//Ставим новую сторону

            newSide.AddActionComponent(this, null);
            newSide.EventHide += Hide;

            Transform.parent = objectGame.GetTransformManager().Transform;

            KitManual();
            TransfusionScript.enabled = true;
            return true;
        }
        return false;
    }
    protected override bool OnInitialize()
    {
        base.OnInitialize();
        enabled = false; // TODO WTF?? LOL
        if (TransfusionScript == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TransfusionScript", name);
            enabled = false;
            return false;
        }
        if (TransfusionScript.Material == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TransfusionScript.Material", name);
            enabled = false;
            return false;
        }
        if (ModelTransform == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "ModelTransform", name);
            enabled = false;
            return false;
        }
        //Training.ParticlesInverse = true;
        UpdateColors();
        WorldEther.ChangePalette.Subscribe(HandlerChangePalette);
        return true;
    }
    protected virtual void PlaySoundEffect()
    {
        AudioContainer.Manager.DefaultTransferSound.Play();
    }
    private void HandlerChangePalette(Ethers.Channel.Info info)
    {
        UpdateColors();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangePalette.Unsubscribe(HandlerChangePalette);
    }
}