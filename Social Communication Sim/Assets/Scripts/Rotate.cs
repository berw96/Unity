using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotateX;
    [SerializeField] float rotateY;
    [SerializeField] float rotateZ;
    [SerializeField] float rotateXRate;
    [SerializeField] float rotateYRate;
    [SerializeField] float rotateZRate;
    [SerializeField] GameObject dirLight;

    // Update is called once per frame
    void Update()
    {
        if (rotateX <= 120.0f)
        {
            rotate();
        }
    }

    private void rotate()
    {
        dirLight.transform.rotation = Quaternion.Euler(rotateX, rotateY, rotateZ);
        rotateX += rotateXRate;
        rotateY += rotateYRate;
        rotateZ += rotateZRate;
    }
}
