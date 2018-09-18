using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalLoopSoundController : MonoBehaviour
{
    void Start()
    {
        WorldEther.LoseGame.Subscribe(Stop);
        WorldEther.PauseGame.Subscribe(Stop);
        WorldEther.ResumeGame.Subscribe(Play);
        WorldEther.RestartGame.Subscribe(Play);
    }

    private void Stop(Ethers.Channel.Info inf)
    {
        AudioContainer.Manager.CriticalDefuseLoop.GetTarget().Pause();
    }
    private void Play(Ethers.Channel.Info inf)
    {
        AudioContainer.Manager.CriticalDefuseLoop.GetTarget().Play();
    }
}
