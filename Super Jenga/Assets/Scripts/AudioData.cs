using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioData : ScriptableObject
{
    public AudioClip clip;
    public float duration;
    public bool isPlaying;
}
