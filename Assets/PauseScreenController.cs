using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenController : MonoBehaviour
{
    public void GoToMenu()
    {
        PlayerProgress.Manager.ProgressScore += Score.ScoreManager.CurrentScore;
    }
}
