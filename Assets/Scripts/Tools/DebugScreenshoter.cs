using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class DebugScreenshoter : MonoBehaviour
{
    public static DebugScreenshoter Screenshoter { private set; get; }

    public string Path = "C:/Users/" + System.Environment.UserName + "/Desktop/";

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (Screenshoter != null)
        {
            Destroy(this);
            return;
        }

        Screenshoter = this;
    }

    public void Shot()
    {
        ScreenCapture.CaptureScreenshot(Path + GetRandomName() + ".png");
    }

    public string GetRandomName()
    {
        string result = Application.productName + " shot_";
        for (int i = Random.Range(7, 11); i >= 0; i--)
        {
            result += (Random.Range(0, 10));
        }
        return result;
    }

    bool press;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !press)
        {
            Shot();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            press = false;
        }
    }

    void Awake()
    {
        Initialize();
    }
}
