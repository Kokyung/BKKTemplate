using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioMonitor : MonoBehaviour
{
    public int currentChannels = 0;

    private void OnAudioFilterRead(float[] data, int channels)
    {
        currentChannels = channels;
    }
}
