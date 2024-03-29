#define MOVE_CAMERA
#if (UNITY_2019_3_OR_NEWER && MOVE_CAMERA)

namespace CameraManagement
{
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

        private enum PROJECTION
        {
            ORTHOGRAPHIC,
            PERSPECTIVE
        }

        private const float minRotationRate = 0.001f;
        private const float maxRotationRate = 0.100f;
        private const int numberOfRotationLevels = 4;
        private const float rotationLevel_4_EulerAngle = 315.0f;
        private const float rotationLevel_3_EulerAngle = 225.0f;
        private const float rotationLevel_2_EulerAngle = 135.0f;
        private const float rotationLevel_1_EulerAngle = 45.0f;
        private Quaternion rotationLevel_4;
        private Quaternion rotationLevel_3;
        private Quaternion rotationLevel_2;
        private Quaternion rotationLevel_1;
        private int rotationIterator = 0;
        private Quaternion initOrientation, resOrientation;
        private float rotationProgress;
        private DIRECTION rotationDirection;

        private const float minShiftRate = 0.1f;
        private const float maxShiftRate = 2.0f;
        private const int numberOfShiftLevels = 3;
        private const float shiftLevel_3_Height = 18.0f;
        private const float shiftLevel_2_Height = 10.0f;
        private const float shiftLevel_1_Height = 2.0f;
        private Vector3 shiftLevel_3;
        private Vector3 shiftLevel_2;
        private Vector3 shiftLevel_1;
        private float shiftValue;
        private Vector3 initPosition, resPosition;
        private int shiftIterator = 0;
        private DIRECTION shiftDirection;

        private const float minOrthographicZoomRate = 0.1f;
        private const float maxOrthographicZoomRate = 0.5f;
        private const float minPerspectiveZoomRate = 0.1f;
        private const float maxPerspectiveZoomRate = 2.0f;
        private const int numberOfZoomLevels = 2;
        private const int orthographicZoomLevel_2 = 16;
        private const int orthographicZoomLevel_1 = 8;
        private const int perspectiveZoomLevel_2 = 60;
        private const int perspectiveZoomLevel_1 = 100;
        private float resOrthographicZoom;
        private float resPerspectiveZoom;
        private DIRECTION zoomDirection;

        private List<Quaternion> rotationPresets;
        private List<Vector3> shiftPresets;
        private List<int> orthographicZoomPresets;
        private List<int> perspectiveZoomPresets;

        private static GameObject cameraPivot;

        [Header("Movement Settings")]
        [Range(minRotationRate, maxRotationRate)]
        public float rotationRate;
        [Range(minShiftRate, maxShiftRate)]
        public float shiftRate;
        [Range(minOrthographicZoomRate, maxOrthographicZoomRate)]
        public float orthographicZoomRate;
        [Range(minPerspectiveZoomRate, maxPerspectiveZoomRate)]
        public float perspectiveZoomRate;

        [Space(param_spacing)]
        [Header("SFX")]
        [SerializeField] AudioClip moveSFX;
        [SerializeField] AudioClip zoomInSFX;
        [SerializeField] AudioClip zoomOutSFX;
        bool isPlaying = false;

        private void Start()
        {
            cameraPivot = GameObject.Find("CameraPivot");

            SetRotationPresets();
            SetShiftPresets();
            SetZoomPresets();

            SetRotationProgress();
            SetShiftValue();

            SetInitOrientation();
            SetInitPosition();

            ToggleOrthographicProjection();

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
                if(Camera.main.orthographic)
                    if (Camera.main.orthographicSize != resOrthographicZoom)
                        switch (zoomDirection)
                        {
                            case DIRECTION.ZOOMIN:
                                {
                                    if (Camera.main.orthographicSize > resOrthographicZoom)
                                        Camera.main.orthographicSize -= orthographicZoomRate;
                                    else
                                        Camera.main.orthographicSize = resOrthographicZoom;
                                }
                                break;
                            case DIRECTION.ZOOMOUT:
                                {
                                    if (Camera.main.orthographicSize < resOrthographicZoom)
                                        Camera.main.orthographicSize += orthographicZoomRate;
                                    else
                                        Camera.main.orthographicSize = resOrthographicZoom;
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
                else
                {
                    if (Camera.main.fieldOfView != resPerspectiveZoom)
                        switch (zoomDirection)
                        {
                            case DIRECTION.ZOOMIN:
                                {
                                    if (Camera.main.fieldOfView > resPerspectiveZoom)
                                        Camera.main.fieldOfView -= perspectiveZoomRate;
                                    else
                                        Camera.main.fieldOfView = resPerspectiveZoom;
                                }
                                break;
                            case DIRECTION.ZOOMOUT:
                                {
                                    if (Camera.main.fieldOfView < resPerspectiveZoom)
                                        Camera.main.fieldOfView += perspectiveZoomRate;
                                    else
                                        Camera.main.fieldOfView = resPerspectiveZoom;
                                }
                                break;
                            default:
                                break;
                        }
                    else
                    {
                        StopZooming();
                        Debug.Log($"New camera FOV = {Camera.main.fieldOfView}");
                    }
                }
            }
        }

        private void AdjustCameraHeight()
        {
            {
                if (cameraPivot.transform.position.y != resPosition.y)
                {
                    switch (shiftDirection)
                    {
                        case DIRECTION.UP:
                            {
                                if (cameraPivot.transform.position.y < resPosition.y)
                                {
                                    shiftValue += shiftRate;
                                    cameraPivot.transform.position = new Vector2(0.0f, shiftValue);
                                }
                                else
                                    cameraPivot.transform.position = resPosition;
                            }
                            break;
                        case DIRECTION.DOWN:
                            {
                                if (cameraPivot.transform.position.y > resPosition.y)
                                {
                                    shiftValue -= shiftRate;
                                    cameraPivot.transform.position = new Vector2(0.0f, shiftValue);
                                }
                                else
                                    cameraPivot.transform.position = resPosition;
                            }
                            break;
                        default:
                            break;
                    }
                }
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
                    Mathf.RoundToInt((int)cameraPivot.transform.rotation.eulerAngles.y),
                    0.0f
                    );
        }

        private void SetInitOrientation() { initOrientation.eulerAngles = cameraPivot.transform.rotation.eulerAngles; }

        private void SetInitPosition()
        {
            initPosition = cameraPivot.transform.position;
        }

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
                    resOrthographicZoom = orthographicZoomPresets[0];
                    resPerspectiveZoom = perspectiveZoomPresets[0];
                    break;
                case DIRECTION.ZOOMOUT:
                    resOrthographicZoom = orthographicZoomPresets[1];
                    resPerspectiveZoom = perspectiveZoomPresets[1];
                    break;
                default:
                    break;
            }
            Debug.Log($"Should now {d.ToString()} to {resOrthographicZoom}");
        }

        private void SetRotationProgress() { rotationProgress = 0.0f; }

        private void SetShiftValue() { shiftValue = 0.0f; }

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
                            StartCoroutine(PlaySound(moveSFX, Camera.main.transform.position, 1.0f));
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
                            StartCoroutine(PlaySound(moveSFX, Camera.main.transform.position, 1.0f));
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
                                StartCoroutine(PlaySound(zoomInSFX, Camera.main.transform.position, 0.25f));
                                zoomDirection = DIRECTION.ZOOMIN;
                                Debug.Log("Zooming IN");
                            }
                            if (obj.name.Contains("Out"))
                            {
                                StartCoroutine(PlaySound(zoomOutSFX, Camera.main.transform.position, 0.25f));
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

            rotationPresets = new List<Quaternion>(numberOfRotationLevels);

            rotationLevel_4 = Quaternion.Euler(
                initOrientation.x,
                rotationLevel_4_EulerAngle,
                initOrientation.z
                );
            rotationLevel_3 = Quaternion.Euler(
                initOrientation.x,
                rotationLevel_3_EulerAngle,
                initOrientation.z
                );
            rotationLevel_2 = Quaternion.Euler(
                initOrientation.x,
                rotationLevel_2_EulerAngle,
                initOrientation.z
                );
            rotationLevel_1 = Quaternion.Euler(
                initOrientation.x,
                rotationLevel_1_EulerAngle,
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

            shiftPresets = new List<Vector3>(numberOfShiftLevels);

            shiftLevel_3 = new Vector3(
                initPosition.x,
                shiftLevel_3_Height,
                initPosition.z
                );
            shiftLevel_2 = new Vector3(
                initPosition.x,
                shiftLevel_2_Height,
                initPosition.z
                );
            shiftLevel_1 = new Vector3(
                initPosition.x,
                shiftLevel_1_Height,
                initPosition.z
                );

            shiftPresets.Add(shiftLevel_1);
            shiftPresets.Add(shiftLevel_2);
            shiftPresets.Add(shiftLevel_3);
        }

        private void SetZoomPresets()
        {
            Debug.Log("Setting Zoom presets...");

            orthographicZoomPresets = new List<int>(numberOfZoomLevels);
            orthographicZoomPresets.Add(orthographicZoomLevel_1);
            orthographicZoomPresets.Add(orthographicZoomLevel_2);
            
            perspectiveZoomPresets = new List<int>(numberOfZoomLevels);
            perspectiveZoomPresets.Add(perspectiveZoomLevel_2);
            perspectiveZoomPresets.Add(perspectiveZoomLevel_1);
        }

        private void StopRotating()
        {
            rotationDirection = DIRECTION.NONE;
            SetRotationProgress();
        }

        private void StopShifting()
        {
            shiftDirection = DIRECTION.NONE;
        }

        private void StopZooming()
        {
            zoomDirection = DIRECTION.NONE;
        }

        public static GameObject GetCameraPivot()
        {
            return cameraPivot;
        }

        public void ToggleOrthographicProjection()
        {
            if (Camera.main.orthographic)
            {
                Camera.main.orthographic = false;
                return;
            }
            if (!Camera.main.orthographic)
            {
                Camera.main.orthographic = true;
                return;
            }
        }

        private IEnumerator PlaySound(AudioClip clip, Vector3 point, float volume)
        {
            if (!isPlaying && clip.loadState == AudioDataLoadState.Loaded)
            {
                isPlaying = true;
                AudioSource.PlayClipAtPoint(clip, point, volume);
            }
            
            yield return new WaitForSeconds(clip.length);
            isPlaying = false;
        }
    }
}
#endif
