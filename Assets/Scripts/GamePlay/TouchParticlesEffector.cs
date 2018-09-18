using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TouchParticlesEffector : MonoBehaviour
{

    [SerializeField]
    ObjectGame TargetObjectGame;

    [SerializeField]
    Gradient LoseColor;

    [SerializeField]
    Gradient RightColor;

    [SerializeField]
    GameObject PrefabParticle;

    PoolObjects<ParticleManager> Particles;

    Dictionary<int, ParticleManager> DictionaryParticles;

    [SerializeField]
    private float distance = 100;

    [SerializeField]
    private int CountParticles;

    [SerializeField]
    private bool ActiveEffect;

    public bool RightEffect = true;

    public void SetActiveEffect(bool value)
    {
        ActiveEffect = value;
        if (!value && DictionaryParticles != null)
        {
            var array = DictionaryParticles.Values.ToArray();
            for (int i = array.Length - 1; i >= 0; --i)
                Particles.Enqueue(array[i]);

            DictionaryParticles.Clear();
        }
    }

    public void UpdateColors()
    {
        SetColors(Palette.PaletteManager.PaletteConfiguration.GetNormalColor(), Palette.PaletteManager.PaletteConfiguration.GetLoseColor());
    }

    public void SetColors(Color normal, Color lose)
    {
        RightColor = CreateGradient(normal);
        LoseColor = CreateGradient(lose);
    }

    private Gradient CreateGradient(Color Target)
    {
        Gradient gradient = new Gradient();

        GradientColorKey[] keys = new GradientColorKey[] {
            new GradientColorKey(Color.white,0),
            new GradientColorKey(Target,1)
        };

        gradient.colorKeys = keys;
        gradient.mode = GradientMode.Blend;

        return gradient;
    }

    public bool GetActiveEffect()
    {
        return ActiveEffect;
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TargetObjectGame == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "TargetObjectGame", name);
            enabled = false;
            return;
        }
        if (PrefabParticle == null || PrefabParticle.GetComponent<ParticleManager>() == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null or {0} not have ParticleManager", "PrefabParticle", name);
            enabled = false;
            return;
        }

        DictionaryParticles = new Dictionary<int, ParticleManager>(CountParticles / 2);

        Particles = new PoolObjects<ParticleManager>();

        Particles.EnqueueObject += delegate (ParticleManager manager) { manager.EmissonActive = false; };
        Particles.DequeueObject += delegate (ParticleManager manager) { manager.EmissonActive = true; };

        for (int i = 0; i < CountParticles; ++i)
            Particles.AddObject(Instantiate(PrefabParticle, transform).GetComponent<ParticleManager>());

        // TargetObjectGame.Initialize();
        var touchRotation = TargetObjectGame.GetTouchAnimationController().GetTouchRotation();

        touchRotation.DragAction += DragHandler;
        touchRotation.DragBegin += DragBeginHandler;
        touchRotation.DragEnd += DragEndHandler;
    }
    void Start()
    {
        Initialize();
        WorldEther.ChangePalette.Subscribe(HandlerPalette);
        if (ActiveEffect)
        {
            int eff = GameSettings.QualitySettingsToInt(GameSettings.Manager.GetEffects());
            SetActiveEffect(eff >= 0);
            RightEffect = eff == 1;
        }
        UpdateColors();
    }
    Side _side;
    Side.Diraction _diraction;

    private void HandlerPalette(Ethers.Channel.Info info)
    {
        UpdateColors();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        WorldEther.ChangePalette.Unsubscribe(HandlerPalette);
    }

    //TODO
    private void DragHandler(int id, Vector2 press, Vector2 current)
    {
        if (!enabled || !ActiveEffect) return;


        ParticleManager particles;
        if (!DictionaryParticles.TryGetValue(id, out particles))
        {
            particles = AddParticles(id);
            if (particles == null) return;
            // particles.EmissonActive = true;
        }
        particles.Position = Camera.main.ScreenPointToRay(current).GetPoint(distance);

        if (RightEffect)
        {
            _side = TargetObjectGame.FrontSide;
            if (_side == null) return;
            Vector2 drag = press - current;

            if (Mathf.Abs(drag.x) > Mathf.Abs(drag.y))
            {
                if (drag.x > 0)
                {
                    _diraction = _side.Left;
                }
                else
                    _diraction = _side.Right;
            }
            else
            {
                if (drag.y > 0)
                    _diraction = _side.Down;
                else
                    _diraction = _side.Up;
            }

            bool flag = false;
            var ps = particles.ParticleSystem.main;
            var comps = _side.GetActionComponents();

            foreach (ActionComponent comp in comps)
                if (comp.WillRotate(_diraction))
                {
                    ParticleSystem.MinMaxGradient gradient = new ParticleSystem.MinMaxGradient(RightColor);
                    gradient.mode = ParticleSystemGradientMode.RandomColor;
                    ps.startColor = gradient;

                    flag = true;
                    // Debug.Log("есть");
                    break;
                }

            if (!flag)
            {
                ParticleSystem.MinMaxGradient gradient = new ParticleSystem.MinMaxGradient(LoseColor);
                gradient.mode = ParticleSystemGradientMode.RandomColor;
                ps.startColor = gradient;
            }

            // Debug.Log("position");
        }
    }

    private void DragBeginHandler(int id)
    {
        if (!ActiveEffect) return;
        AddParticles(id);
    }
    private ParticleManager AddParticles(int id)
    {
        if (DictionaryParticles.Count >= CountParticles) return null;
        var particles = Particles.Dequeue();
        DictionaryParticles.Add(id, particles);
        return particles;
    }
    private void DragEndHandler(int id)
    {
        if (!ActiveEffect) return;
        ParticleManager manager;
        if (DictionaryParticles.TryGetValue(id, out manager))
        {
            DictionaryParticles.Remove(id);
            Particles.Enqueue(manager);
        }
    }
}
