using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    private const float zoomRate    = 0.5f;
    private const int maxZoom       = 8;
    private const int minZoom       = 16;
    private bool isZoomingIn        = false;
    private bool isZoomingOut       = false;

    private void Start()
    {
        Camera.main.orthographicSize = minZoom;
    }

    private void FixedUpdate()
    {
        if (isZoomingIn)
        {
            if (Camera.main.orthographicSize <= maxZoom)
            {
                isZoomingIn = false;
                Camera.main.orthographicSize = maxZoom;
                return;
            }
            else
            {
                Camera.main.orthographicSize -= zoomRate;
            }
        }
        if (isZoomingOut)
        {
            if (Camera.main.orthographicSize >= minZoom)
            {
                isZoomingOut = false;
                Camera.main.orthographicSize = minZoom;
                return;
            }
            else
            {
                Camera.main.orthographicSize += zoomRate;
            }
        }
    }

    public void ZoomIn()
    {
        if (isZoomingIn || isZoomingOut)
            return;

        isZoomingIn = true;
    }

    public void ZoomOut()
    {
        if (isZoomingIn || isZoomingOut)
            return;

        isZoomingOut = true;
    }
}
