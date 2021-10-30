using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private const int param_spacing = 10;

    private enum DIRECTION
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT,
        ZOOMIN,
        ZOOMOUT
    }

    private const float minRotationRate = 0.001f;
    private const float maxRotationRate = 0.100f;
    private Quaternion rotationLevel_4;
    private Quaternion rotationLevel_3;
    private Quaternion rotationLevel_2;
    private Quaternion rotationLevel_1;
    private int rotationIterator = 0;
    private Quaternion initOrientation, resOrientation;
    private float rotationProgress;
    private DIRECTION rotationDirection;

    private const float minShiftRate = 0.5f;
    private const float maxShiftRate = 2.0f;
    private Vector2 shiftLevel_3;
    private Vector2 shiftLevel_2;
    private Vector2 shiftLevel_1;
    private float shiftProgress;
    private Vector2 initPosition, resPosition;
    private int shiftIterator = 0;
    private DIRECTION shiftDirection;

    private const float minZoomRate = 0.2f;
    private const float maxZoomRate = 0.5f;
    private const int zoomLevel_2 = 16;
    private const int zoomLevel_1 = 8;
    private float initZoom, resZoom;
    private DIRECTION zoomDirection;

    private List<Quaternion> rotationPresets;
    private List<Vector2> shiftPresets;
    private List<int> zoomPresets;

    [Header("Camera Pivot")]
    [SerializeField] GameObject cameraPivot;

    [Space(param_spacing)]
    [Header("Rotation Rate")]
    [Range(minRotationRate, maxRotationRate)]
    public float rotationRate;

    [Space(param_spacing)]
    [Header("Shift Rate")]
    [Range(minShiftRate, maxShiftRate)]
    public float shiftRate;

    [Space(param_spacing)]
    [Header("Zoom Rate")]
    [Range(minZoomRate, maxZoomRate)]
    public float zoomRate;
    
    private void Start()
    {
        SetRotationPresets();
        SetShiftPresets();
        SetZoomPresets();

        SetRotationProgress();
        SetShiftProgress();

        SetInitOrientation();
        SetInitPosition();

        Debug.Log($"Init camera orientation = {cameraPivot.transform.rotation.eulerAngles}");
        Debug.Log($"Init camera position = {cameraPivot.transform.position}");
        Debug.Log($"Init camera zoom = {Camera.main.orthographicSize}");
    }

    private void FixedUpdate()
    {
        {
            if (!rotationDirection.Equals(DIRECTION.NONE))
                AdjustCameraAngle();

            if (!zoomDirection.Equals(DIRECTION.NONE))
                AdjustCameraZoom();

            if (!shiftDirection.Equals(DIRECTION.NONE))
                AdjustCameraHeight();
        }
    }

    private void AdjustCameraZoom()
    {
        {
            if (Camera.main.orthographicSize != resZoom)
                switch (zoomDirection)
                {
                    case DIRECTION.ZOOMIN:
                        {
                            if (Camera.main.orthographicSize > resZoom)
                                Camera.main.orthographicSize -= zoomRate;
                            else
                                Camera.main.orthographicSize = resZoom;
                        }
                        break;
                    case DIRECTION.ZOOMOUT:
                        {
                            if (Camera.main.orthographicSize < resZoom)
                                Camera.main.orthographicSize += zoomRate;
                            else
                                Camera.main.orthographicSize = resZoom;
                        }
                        break;
                    default:
                        break;
                }
            else
            {
                StopZooming();
                Debug.Log($"New camera zoom = {Camera.main.orthographicSize}");
            }
        }
    }

    private void AdjustCameraHeight()
    {
        {
            if (cameraPivot.transform.position != (Vector3)resPosition)
                Vector3.Lerp(
                    cameraPivot.transform.position,
                    resPosition,
                    shiftProgress += shiftRate);
            else
            {
                StopShifting();
                Debug.Log($"New camera height = {cameraPivot.transform.position}");
            }
        }
    }

    private void AdjustCameraAngle()
    {
        {
            if (cameraPivot.transform.rotation.eulerAngles != resOrientation.eulerAngles)
                cameraPivot.transform.rotation = Quaternion.Slerp(
                    cameraPivot.transform.rotation,
                    resOrientation,
                    rotationProgress += rotationRate);

            else
            {
                StopRotating();
                AdjustCameraPivotRotationOffset();
                Debug.Log($"New camera orientation = {cameraPivot.transform.rotation.eulerAngles}");
            }
        }
    }

    private void AdjustCameraPivotRotationOffset()
    {
        cameraPivot.transform.rotation.eulerAngles.Set(
                0.0f,
                (cameraPivot.transform.rotation.eulerAngles.y + 45.0f),
                0.0f
                );
    }

    private void SetInitOrientation() { initOrientation.eulerAngles = cameraPivot.transform.rotation.eulerAngles; }

    private void SetInitPosition(){ initPosition = cameraPivot.transform.position; }


    private void SetResOrientation(DIRECTION d)
    {
        switch (d)
        {
            case DIRECTION.LEFT:
                rotationIterator++;
                if (rotationIterator > rotationPresets.Count - 1)
                    rotationIterator = 0;
                break;
            case DIRECTION.RIGHT:
                rotationIterator--;
                if (rotationIterator < 0)
                    rotationIterator += rotationPresets.Count;
                break;
            default:
                break;
        }
        Debug.LogWarning($"Rotation iterator = {rotationIterator}");
        resOrientation = rotationPresets[rotationIterator];
        Debug.Log($"Should now rotate {d.ToString()} to {resOrientation.eulerAngles.y} degrees");
    }

    private void SetResShift(DIRECTION d)
    {
        switch (d)
        {
            case DIRECTION.UP:
                shiftIterator++;
                if (shiftIterator > shiftPresets.Count - 1)
                    shiftIterator = shiftPresets.Count - 1;
                break;
            case DIRECTION.DOWN:
                shiftIterator--;
                if (shiftIterator < 0)
                    shiftIterator = 0;
                break;
            default:
                break;
        }
        Debug.LogWarning($"Shift iterator = {shiftIterator}");
        resPosition = shiftPresets[shiftIterator];
        Debug.Log($"Should now shift {d.ToString()} to {shiftPresets[shiftIterator]}");
    }

    private void SetResZoom(DIRECTION d)
    {
        switch (d)
        {
            case DIRECTION.ZOOMIN:
                resZoom = zoomPresets[0];
                break;
            case DIRECTION.ZOOMOUT:
                resZoom = zoomPresets[1];
                break;
            default:
                break;
        }
        Debug.Log($"Should now {d.ToString()} to {resZoom}");
    }

    private void SetRotationProgress() { rotationProgress = 0.0f; }
    
    private void SetShiftProgress() { shiftProgress = 0.0f; }

    public void SetDirection(GameObject obj)
    {
        if (obj.Equals(null))
        {
            Debug.LogWarning($"Unrecognized object: {obj}." +
                    "Please ensure a valid button with a valid" +
                    "name is connected.");
            return;
        }
        else
        {
            {
                if (obj.name.Contains("Rotate"))
                    if (rotationDirection.Equals(DIRECTION.LEFT) ||
                        rotationDirection.Equals(DIRECTION.RIGHT))
                    {
                        Debug.LogWarning($"Trying to {obj.name} already");
                        return;
                    }
                    else
                    {
                        if (obj.name.Contains("Left"))
                        {
                            rotationDirection = DIRECTION.LEFT;
                            Debug.Log("Rotating LEFT");
                        }
                        if (obj.name.Contains("Right"))
                        {
                            rotationDirection = DIRECTION.RIGHT;
                            Debug.Log("Rotating RIGHT");
                        }
                        SetResOrientation(rotationDirection);
                    }
            }

            {
                if (obj.name.Contains("Shift"))
                    if (shiftDirection.Equals(DIRECTION.UP) ||
                        shiftDirection.Equals(DIRECTION.DOWN))
                    {
                        Debug.LogWarning($"Trying to {obj.name} already");
                        return;
                    }
                    else
                    {
                        if (obj.name.Contains("Up"))
                        {
                            shiftDirection = DIRECTION.UP;
                            Debug.Log("Shifting UP");
                        }
                        if (obj.name.Contains("Down"))
                        {
                            shiftDirection = DIRECTION.DOWN;
                            Debug.Log("Shifting DOWN");
                        }
                        SetResShift(shiftDirection);
                    }
            }

            {
                if (obj.name.Contains("Zoom"))
                    if (zoomDirection.Equals(DIRECTION.ZOOMIN) ||
                        zoomDirection.Equals(DIRECTION.ZOOMOUT))
                    {
                        Debug.LogWarning($"Trying to {obj.name} already");
                        return;
                    }
                    else
                    {
                        if (obj.name.Contains("In"))
                        {
                            zoomDirection = DIRECTION.ZOOMIN;
                            Debug.Log("Zooming IN");
                        }
                        if (obj.name.Contains("Out"))
                        {
                            zoomDirection = DIRECTION.ZOOMOUT;
                            Debug.Log("Zooming OUT");
                        }
                        SetResZoom(zoomDirection);
                    }
            }
        }
    }

    private void SetRotationPresets()
    {
        Debug.Log("Setting Rotation presets...");

        rotationPresets = new List<Quaternion>();

        rotationLevel_4 = Quaternion.Euler(
            initOrientation.x,
            315.0f,
            initOrientation.z
            );
        rotationLevel_3 = Quaternion.Euler(
            initOrientation.x,
            225.0f,
            initOrientation.z
            );
        rotationLevel_2 = Quaternion.Euler(
            initOrientation.x,
            135.0f,
            initOrientation.z
            );
        rotationLevel_1 = Quaternion.Euler(
            initOrientation.x,
            45.0f,
            initOrientation.z
            );

        rotationPresets.Add(rotationLevel_1);
        rotationPresets.Add(rotationLevel_2);
        rotationPresets.Add(rotationLevel_3);
        rotationPresets.Add(rotationLevel_4);
    }

    private void SetShiftPresets()
    {
        Debug.Log("Setting Shift presets...");

        shiftPresets = new List<Vector2>(3);

        shiftLevel_3 = new Vector2(
            initPosition.x,
            18.0f
            );
        shiftLevel_2 = new Vector2(
            initPosition.x,
            10.0f
            );
        shiftLevel_1 = new Vector2(
            initPosition.x,
            2.0f
            );

        shiftPresets.Add(shiftLevel_1);
        shiftPresets.Add(shiftLevel_2);
        shiftPresets.Add(shiftLevel_3);
    }

    private void SetZoomPresets()
    {
        Debug.Log("Setting Zoom presets...");

        zoomPresets = new List<int>(2);
        zoomPresets.Add(zoomLevel_1);
        zoomPresets.Add(zoomLevel_2);
    }

    private void StopRotating()
    {
        rotationDirection = DIRECTION.NONE;
        SetRotationProgress();
    }

    private void StopShifting()
    {
        shiftDirection = DIRECTION.NONE;
        SetShiftProgress();
    }

    private void StopZooming()
    {
        zoomDirection = DIRECTION.NONE;
    }
}
