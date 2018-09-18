using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardSwiper : MonoBehaviour
{
    [SerializeField]
    ObjectGame TargetObject;

    bool up, left, down, right;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (TargetObject != null)
        {
            if (!down && Input.GetKeyDown(KeyCode.DownArrow))
            {
                down = true;
                TargetObject.SwipeDown();
            }
            else if (!up && Input.GetKeyDown(KeyCode.UpArrow))
            {
                up = true;
                TargetObject.SwipeUp();
            }
            else if (!left && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                left = true;
                TargetObject.SwipeLeft();
            }
            else if (!right && Input.GetKeyDown(KeyCode.RightArrow))
            {
                right = true;
                TargetObject.SwipeRight();
            }

            if (Input.GetKeyUp(KeyCode.DownArrow)) down = false;
            if (Input.GetKeyUp(KeyCode.UpArrow)) up = false;
            if (Input.GetKeyUp(KeyCode.LeftArrow)) left = false;
            if (Input.GetKeyUp(KeyCode.RightArrow)) right = false;

            // if (Input.GetKeyDown(KeyCode.Space))
            //     ObjectsGame[0].Stop();
            // else if (Input.GetKeyDown(KeyCode.H))
            //     ObjectsGame[0].Hide();
            // else if (Input.GetKeyDown(KeyCode.S))
            //     ObjectsGame[0].Show();
        }

    }
}
