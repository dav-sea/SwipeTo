using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeHelper : MonoBehaviour
{
    public static event System.Action EventFullLeft;
    private static FreezeHelper instance;
    private const float Xprocent = 0.5f, Yprocent = 0.15f;

    [SerializeField] private UnityEngine.UI.Text Text;
    [SerializeField] private Appearance Appearance;

    private float _freeze;
    public static float FreezeTime
    {
        set
        {
            instance._freeze = value;
            if (value != 0)
            {
                instance.enabled = true;
                instance.Appearance.Show();
                AudioContainer.Manager.DefaultClocksSound.Play();
            }
        }
        get { return instance._freeze; }
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        _freeze -= Time.deltaTime;
        if (_freeze < 0)
        {
            _freeze = 0;
            enabled = false;
            Appearance.Hide();
            AudioContainer.Manager.DefaultClocksSound.ForceVolume = 0;
            // GamePlayContenier.GamePlayCore.
            EventFullLeft();
        }
        UpdateTime();
    }

    protected void SetTime(float sec)
    {
        Text.text = string.Format("{0:00.00}", sec);
    }

    public void UpdateTime()
    {
        SetTime(_freeze);
    }

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        instance = this;
        EventFullLeft += delegate { };
    }
    void Awake()
    {
        Initialize();
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Text.transform.position = UIOrganization.ScalePosition.PositionScale(UIContenier.Contenier.GetUICamera(), Xprocent, Yprocent, 300, new Vector2(0, 0));
    }
}
