using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Rotate</c> handles camera translation
/// during runtime. A <c>Rotate.cs</c> script may be attached
/// to a <c>GameObject</c> and used to make it rotate.
/// 
/// Once a set rotation limit has been reached on an axis, the 
/// cameraPivotect's rotation on that axis is reset.
/// </summary>

public class Rotate : MonoBehaviour
{
    private const int param_spacing = 25;
    private const float rotationYOffset = 90.000000000f;

    private float initRotationX;
    public float initRotationY;
    private float initRotationZ;

    private bool isRotatingRight = false;
    private bool isRotatingLeft = false;

    [Header ("Rotation Parameters")]
    [SerializeField] float rotateX;
    [SerializeField] float rotateY;
    [SerializeField] float rotateZ;
    [Space(param_spacing)]
    [SerializeField] float rotateXRate;
    [SerializeField] float rotateYRate;
    [SerializeField] float rotateZRate;
    [Space(param_spacing)]
    [SerializeField] float rotateXLimit;
    [SerializeField] float rotateYLimit;
    [SerializeField] float rotateZLimit;
    [Space(param_spacing)]
    [SerializeField] GameObject cameraPivot;

    private void Start()
    {
        initRotationX = cameraPivot.transform.rotation.x;
        initRotationY = cameraPivot.transform.rotation.y;
        initRotationZ = cameraPivot.transform.rotation.z;
    }

    private void FixedUpdate()
    {
        if (isRotatingRight)
        {
            RotateObject();
            if (rotateY >= (initRotationY + rotationYOffset))
            {
                isRotatingRight = false;
                rotateYRate = 0.0f;
                cameraPivot.transform.rotation = Quaternion.Euler(
                    initRotationX, 
                    Mathf.Round(initRotationY + rotationYOffset), 
                    initRotationZ);
            }
        }
        if (isRotatingLeft)
        {
            RotateObject();
            if (rotateY <= (initRotationY - rotationYOffset))
            {
                isRotatingLeft = false;
                rotateYRate = 0.0f;
                cameraPivot.transform.rotation = Quaternion.Euler(
                    initRotationX, 
                    Mathf.Round(initRotationY - rotationYOffset), 
                    initRotationZ);
            }
        }
    }

    private void RotateObject()
    {
        rotateX += rotateXRate;
        rotateY += rotateYRate;
        rotateZ += rotateZRate;
        cameraPivot.transform.rotation = Quaternion.Euler(rotateX, rotateY, rotateZ);
    }

    public void RotateRight()
    {
        if (isRotatingLeft || isRotatingRight)
            return;

        StoreInitRotationY();

        Debug.Log("Rotating Right");

        isRotatingRight = true;
        isRotatingLeft = false;
        rotateYRate = 5.0f;
    }

    public void RotateLeft()
    {
        if (isRotatingLeft || isRotatingRight)
            return;

        StoreInitRotationY();

        Debug.Log("Rotating Left");

        isRotatingRight = false;
        isRotatingLeft = true;
        rotateYRate = -5.0f;
    }

    private void StoreInitRotationY()
    {
        initRotationY = rotateY;
    }
}
