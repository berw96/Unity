using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class Timer : ScriptableObject
{
    public float time;

    public void resetTimer()
    {
        time = 0.0f;
    }

    public void incrementTime()
    {
        time += Time.deltaTime;
    }
}
