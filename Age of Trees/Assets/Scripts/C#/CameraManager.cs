#define CAMERA_MANAGER
#if (UNITY_2019_3_OR_NEWER && CAMERA_MANAGER)

/*
 * Age of Trees, a Lindenmayer simulator.
 * 
 * By Elliot Walker (2021) | SN: 3368 6408
 * Goldsmiths, University of London
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Slider zoom_slider;
    private float editor_FOV;

    private void Awake()
    {
        try {
            Debug.Log($"Registered {zoom_slider} as the slider for the camera zoom.");
            editor_FOV = Camera.main.fieldOfView;
        } catch (NullReferenceException e) {
            Debug.LogWarning($"{e}");
        }
    }

    private void Update()
    {
        Camera.main.fieldOfView = zoom_slider.value * editor_FOV;
    }
}
#endif
