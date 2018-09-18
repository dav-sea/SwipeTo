using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransfusionScript : MonoBehaviour
{

    public event System.Action EventFinish;
    [SerializeField]
    private Renderer Renderer;

    [SerializeField]
    private Gradient Gradient;

    public LoopMode Loop = LoopMode.Once;

    private Material _mat;
    public Material Material
    {
        set { _mat = value; }
        get
        {
            if (_mat == null && Renderer != null)
                _mat = Renderer.material;
            return _mat;
        }
    }

    public float SpeedTransfusionColor = 1;

    private float StartKey = 0;

    public void SetTransfusion(Color color)
    {
        var newGradient = new Gradient();
        newGradient.colorKeys = new GradientColorKey[] { new GradientColorKey(color, 0) };
        SetReflectionGradient(newGradient);
    }

    public void SetTransfusion(params Color[] colors)
    {
        var newGradient = new Gradient();
        var colorKeys = new GradientColorKey[Mathf.Clamp(colors.Length, 0, 8)];

        for (int i = 0; i < colorKeys.Length; ++i)
            colorKeys[i] = new GradientColorKey(colors[i], (float)i / ((float)colorKeys.Length - 1));

        newGradient.SetKeys(colorKeys, new GradientAlphaKey[0]);
        newGradient.mode = GradientMode.Blend;

        SetReflectionGradient(newGradient);
    }

    public void SetTransfusion(Gradient gradient)
    {
        var newGradient = new Gradient();
        newGradient.SetKeys(gradient.colorKeys, gradient.alphaKeys);
        newGradient.mode = gradient.mode;
        SetReflectionGradient(newGradient);
    }

    protected void SetReflectionGradient(Gradient gradient)
    {
        Gradient = gradient;
        Step(0);
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        EventFinish += delegate { };
        if (Material != null)
            Step(StartKey);

    }

    private void Step(float add)
    {
        left += add;
        if (vector > 0)
        {
            if (Distance(left, 0) >= 1)
                Finish();
        }
        else
        {
            if (Distance(left, 1) >= 1)
                Finish();
        }
        Material.color = Gradient.Evaluate(left);
    }

    public void ResetStep()
    {
        left = 0;
        Step(0);
    }

    public void ResetAndStart()
    {
        ResetStep();
        enabled = true;
    }

    private void Finish()
    {
        if (enabled)
            if (Loop == LoopMode.Once)
            {
                left = 1;
                enabled = false;
                EventFinish();
            }
            else if (Loop == LoopMode.Loop)
            {
                left = 0;
            }
            else if (Loop == LoopMode.PingPong)
            {
                vector = -vector;
                left = Distance(left, 0);
            }
    }
    private float left;

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        enabled = false;
    }

    private float vector = 1;

    // Update is called once per frame
    void Update()
    {
        if (Material != null)
        {
            Step(SpeedTransfusionColor * Time.deltaTime * vector);
        }
        else
        {
            enabled = false;
            Debug.LogFormat("TransfusionScript not have material");
        }
    }

    private float Distance(float first, float second)
    {
        return Mathf.Max(first, second) - Mathf.Min(first, second);
    }

    [ContextMenu("FindRenderer")]
    private void Editor_FindRenderer()
    {
        Renderer = GetComponent<Renderer>();
    }

    public enum LoopMode { Once, Loop, PingPong }
}
