using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragInSwipeConvertor : MonoBehaviour
{
    [SerializeField]
    private ObjectGame ObjectGame;

    [SerializeField]
    private TouchRotationAnimation TouchRotationAnimation;

    private bool _initialized;
    public void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        //Initialize logic
        if (TouchRotationAnimation == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", TouchRotationAnimation, name);
            _initialized = false;
            enabled = false;
            return;
        }
        if (ObjectGame == null)
        {
            Debug.LogWarningFormat("{0} (in {1}) is null", ObjectGame, name);
            _initialized = false;
            enabled = false;
            return;
        }

        TouchRotationAnimation.Initialize();
        ObjectGame.Initialize();

        TouchRotationAnimation.Drag.AddListener(DragHandler);
    }
    void Start()
    {
        Initialize();
    }

    private void DragHandler(Vector2 drag)
    {
        if (!enabled) return;
        if (Mathf.Abs(drag.x) > Mathf.Abs(drag.y))
        {
            if (drag.x > 0)
                ObjectGame.SwipeRight();
            else
                ObjectGame.SwipeLeft();
        }
        else
        {
            if (drag.y > 0)
                ObjectGame.SwipeUp();
            else
                ObjectGame.SwipeDown();
        }
    }
}