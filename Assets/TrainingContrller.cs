using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingContrller : MonoBehaviour
{

    [SerializeField] Appearance AppearanceTraining;
    [SerializeField] ParticleManager Particle;
    [SerializeField] TranslationController Translation;

    [HideInInspector] public ColorsReference ParticleColor = ColorsReference.Normal;
    [HideInInspector] public float AplhaModificator = 0.6f;

    // public int CountTrainingComplete = 0;

    public void SetNameTranslation(string name)
    {
        Translation.NameTranslation = name;
        Translation.UpdateTranslation();
    }

    public int CountParticles
    {
        set
        {
            var emission = Particle.Emission;
            emission.rateOverTime = value;
        }
        get
        {
            return (int)Particle.Emission.rateOverTime.constant;
        }
    }
    private bool _inverse;
    public bool ParticlesInverse
    {
        set
        {
            Particle.Initialize();
            _inverse = value;
            if (_inverse)
                Particle.Transform.localRotation = Quaternion.AngleAxis(180, Vector3.forward);
            else Particle.Transform.localRotation = Quaternion.identity;
        }
        get
        {
            return _inverse;
        }
    }

    public bool ActiveTraining
    {
        set
        {
            Initialize();
            AppearanceTraining.IsAppearance = value;
        }
        get
        {
            return AppearanceTraining.IsAppearance;
        }
    }

    public bool TextActive
    {
        set { Translation.gameObject.SetActive(value); }
        get { return Translation.gameObject.activeSelf; }
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (AppearanceTraining == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "AppearanceTraining", name);
            enabled = false;
            return;
        }
        if (Particle == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Particle", name);
            enabled = false;
            return;
        }
        WorldEther.ChangePalette.Subscribe(Handle);
        WorldEther.ChangePalette.Unsubscribe(Handle);
    }

    private void Handle(Ethers.Channel.Info inf)
    {
        UpdateColor();
    }

    public void UpdateColor()
    {
        Particle.StartColor = ColorReference.ReferenceToColor(ParticleColor, AplhaModificator);
    }
}
