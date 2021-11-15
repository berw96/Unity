using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    private const int minFrameRate = 20;
    private const int maxFrameRate = 300;

    [Range(minFrameRate, maxFrameRate)]
    [SerializeField] int frameRate;

    void Start()
    {
        if (frameRate < 20)
            frameRate = 20;

        Application.targetFrameRate = frameRate;
    }
}
