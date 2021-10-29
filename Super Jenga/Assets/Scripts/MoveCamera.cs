using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private const int param_spacing = 20;

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

    private const float minRotationSpeed = 0.001f;
    private const float maxRotationSpeed = 0.100f;
    private Quaternion rotationLevel_4;
    private Quaternion rotationLevel_3;
    private Quaternion rotationLevel_2;
    private Quaternion rotationLevel_1;
    private int rotationIterator = 0;
    private Quaternion initOrientation, resOrientation;
    private float rotationProgress;
    private DIRECTION rotationDirection;

    private const float minShiftSpeed = 0.5f;
    private const float maxShiftSpeed = 2.0f;
    private const float shiftLevel_3 = 18.0f;
    private const float shiftLevel_2 = 10.0f;
    private const float shiftLevel_1 = 2.0f;
    private float shiftProgress;
    private Vector3 initPosition, resPosition;
    private DIRECTION shiftDirection;

    private const float minZoomSpeed = 0.2f;
    private const float maxZoomSpeed = 0.5f;
    private const int zoomLevel_2 = 16;
    private const int zoomLevel_1 = 8;
    private float zoomProgress;
    private float initZoom, resZoom;
    private DIRECTION zoomDirection;

    private List<Quaternion> rotationPresets;
    private List<float> shiftPresets;
    private List<int> zoomPresets;

    [Header("Camera Pivot")]
    [SerializeField] GameObject cameraPivot;
    [Space(param_spacing)]
    [Header("Rotation Speed")]
    [Range(minRotationSpeed, maxRotationSpeed)]
    public float rotationSpeed;
    [Space(param_spacing)]
    [Header("Shift Speed")]
    [Range(minShiftSpeed, maxShiftSpeed)]
    public float shiftSpeed;
    [Space(param_spacing)]
    [Header("Zoom Speed")]
    [Range(minZoomSpeed, maxZoomSpeed)]
    public float zoomSpeed;
    
    private void Start()
    {
        SetRotationPresets();
        SetShiftPresets();
        SetZoomPresets();

        SetRotationProgress();
        SetShiftProgress();
        SetZoomProgress();

        SetInitOrientation();
        SetInitPosition();
        SetInitZoom();

        Debug.Log($"Init y-orientation = {cameraPivot.transform.rotation.eulerAngles.y}");
    }

    private void FixedUpdate()
    {
        if (rotationDirection.Equals(DIRECTION.NONE))
            return;

        SlerpObject();
    }

    private void LerpObject()
    {

    }

    private void SlerpObject()
    {
        if (cameraPivot.transform.rotation.eulerAngles != resOrientation.eulerAngles)
        {
            cameraPivot.transform.rotation = Quaternion.Slerp(
                cameraPivot.transform.rotation,
                resOrientation,
                rotationProgress += rotationSpeed);
        }
        else
        {
            cameraPivot.transform.rotation.eulerAngles.Set(
                0.0f,
                (cameraPivot.transform.rotation.eulerAngles.y + 45.0f),
                0.0f
                );
            StopRotating();
            Debug.Log($"New y-orientation = {cameraPivot.transform.rotation.eulerAngles.y}");
        }
    }

    private void SetInitOrientation() { initOrientation.eulerAngles = cameraPivot.transform.rotation.eulerAngles; }

    private void SetInitPosition(){ initPosition = cameraPivot.transform.position; }

    private void SetInitZoom(){ initZoom = Camera.main.orthographicSize; }

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
        Debug.LogWarning($"Iterator value: {rotationIterator}");
        resOrientation = rotationPresets[rotationIterator];
        Debug.Log($"Should now rotate to {resOrientation.eulerAngles.y} degrees");
    }

    private void SetRotationProgress() { rotationProgress = 0.0f; }
    
    private void SetShiftProgress() { shiftProgress = 0.0f; }
    
    private void SetZoomProgress() { zoomProgress = 0.0f; }

    public void SetRotationDirection(GameObject obj)
    {
        if (rotationDirection.Equals(DIRECTION.LEFT) ||
            rotationDirection.Equals(DIRECTION.RIGHT))
        {
            Debug.LogWarning("Trying to rotate already");
            return;
        }
            
        switch (obj.name)
        {
            case "RotateLeft":
                rotationDirection = DIRECTION.LEFT;
                SetResOrientation(rotationDirection);
                Debug.Log("Rotating LEFT");
                break;
            case "RotateRight":
                rotationDirection = DIRECTION.RIGHT;
                SetResOrientation(rotationDirection);
                Debug.Log("Rotating RIGHT");
                break;
            default:
                rotationDirection = DIRECTION.NONE;
                SetResOrientation(rotationDirection);
                Debug.LogWarning($"Unrecognized object reference: {obj}." +
                    "Please ensure either a RotateLeft or RotateRight " +
                    "button is attached.");
                break;
        }
    }

    public void SetShiftDirection(GameObject obj)
    {

    }
    
    public void SetZoomDirection(GameObject obj)
    {

    }

    private void SetRotationPresets()
    {
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
        shiftPresets = new List<float>(3);
        shiftPresets.Add(shiftLevel_1);
        shiftPresets.Add(shiftLevel_2);
        shiftPresets.Add(shiftLevel_3);
    }

    private void SetZoomPresets()
    {
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

    }

    private void StopZooming()
    {

    }
}
