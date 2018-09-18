using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text Text;
    // Update is called once per frame
    [SerializeField]
    [Range(0, 2)]
    float TimeToUpdateStatistic = 0.5f;
    private string Version;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Text == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", "Text", name);
            enabled = false;
            return;
        }
    }
    void Awake()
    {
        Initialize();
        Version = "<size=35>" + Application.version + "</size>\n";
    }

    protected void SetText(string text)
    {
        if (Text != null)
        {
            Text.text = Version + text;
        }
    }

    protected void SetInfo(float frames, float maxFrameTime, float avgFrameTime)
    {
        // SetText(string.Format("FPS {0}\nMax {1}\n Avg {2}", (int)frames, maxFrameTime, avgFrameTime));
        SetText(((int)frames).ToString());
    }

    int FramesCount;
    float TimeLeft;
    float MaxFrameTime;

    public void UpdateStatistic()
    {
        SetInfo(FramesCount / TimeLeft, MaxFrameTime, TimeLeft / FramesCount);
        Flash();
    }

    public void Flash()
    {
        TimeLeft = 0;
        FramesCount = 0;
        MaxFrameTime = 0;
    }

    protected void Stat(float timeLeft)
    {

        TimeLeft += timeLeft;
        ++FramesCount;
        if (timeLeft > MaxFrameTime) MaxFrameTime = timeLeft;
        if (TimeLeft >= TimeToUpdateStatistic) UpdateStatistic();
    }

    void Update()
    {
        Stat(Time.unscaledDeltaTime);
    }
}
