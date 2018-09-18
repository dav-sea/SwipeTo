using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSwitch : MonoBehaviour
{
    [Range(1, 10)] [SerializeField] private int Segments = 3;

    private int currentSwitch { get { return Mathf.RoundToInt(AudioManager.BaseVolume * Segments); } }

    public void NextSwitch()
    {
        SetSegmentToVolume(currentSwitch + 1);
    }

    private void SetSegmentToVolume(int segment)
    {
        if (segment > Segments) segment = 0;
        SetVolume(segment / (float)Segments);
    }

    private void SetVolume(float volume)
    {
        AudioManager.BaseVolume = volume;
    }

    public void UpdateCurrent()
    {

    }
}
