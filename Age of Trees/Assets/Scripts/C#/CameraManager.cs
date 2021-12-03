#define CAMERA_MANAGER
#if (UNITY_2019_3_OR_NEWER && CAMERA_MANAGER)

using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Slider slider;
    private float editor_FOV;

    private void Start()
    {
        try {
            Debug.Log($"Registered {slider} as the slider for the camera zoom.");
            editor_FOV = Camera.main.fieldOfView;
        } catch (NullReferenceException) {
            Exception e = new NullReferenceException();
            Debug.LogWarning($"{e.ToString()}");
        }
    }

    private void FixedUpdate()
    {
        Camera.main.fieldOfView = slider.value * editor_FOV;
    }
}
#endif
