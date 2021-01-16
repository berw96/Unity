using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Rotate</c> handles directional light translation
/// during runtime. A <c>Rotate.cs</c> script may be attached
/// to a <c>GameObject</c> and used to make it rotate.
/// 
/// Once a set rotation limit has been reached on an axis, the 
/// object's rotation on that axis is reset.
/// </summary>

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotateX;
    [SerializeField] float rotateY;
    [SerializeField] float rotateZ;
    [SerializeField] float rotateXRate;
    [SerializeField] float rotateYRate;
    [SerializeField] float rotateZRate;
    [SerializeField] float rotateXLimit;
    [SerializeField] float rotateYLimit;
    [SerializeField] float rotateZLimit;
    [SerializeField] GameObject dirLight;

    void FixedUpdate()
    {
        rotate();
        if (rotateX >= rotateXLimit)
            rotateX = 0.0f;
        if (rotateY >= rotateYLimit)
            rotateY = 0.0f;
        if (rotateZ >= rotateZLimit)
            rotateZ = 0.0f;
    }

    private void rotate()
    {
        dirLight.transform.rotation = Quaternion.Euler(rotateX, rotateY, rotateZ);
        rotateX += rotateXRate;
        rotateY += rotateYRate;
        rotateZ += rotateZRate;
    }
}
