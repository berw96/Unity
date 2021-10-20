using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shift : MonoBehaviour
{
    [SerializeField] GameObject cameraPivot;
    private float shiftRate         = 2.0f;
    private bool isShiftingUp       = false;
    private bool isShiftingDown     = false;
    private float shiftCeilingLimit = 10.0f;
    private float shiftFloorLimit   = 2.0f;
    private float shiftValue        = 0.0f;
    private float cameraSize;

    private void Start()
    {
        cameraPivot.transform.position = new Vector2(0.0f, shiftCeilingLimit);
    }

    private void FixedUpdate()
    {
        cameraSize = Camera.main.orthographicSize;

        if (isShiftingUp)
        {
            if (cameraPivot.transform.position.y >= shiftCeilingLimit)
            {
                isShiftingUp = false;
                cameraPivot.transform.position = new Vector2(0.0f, shiftCeilingLimit);
                return;
            }
            shiftValue += shiftRate;
            cameraPivot.transform.position = new Vector2(0.0f, shiftValue);
        }
        if (isShiftingDown)
        {
            if (cameraPivot.transform.position.y <= shiftFloorLimit)
            {
                isShiftingDown = false;
                cameraPivot.transform.position = new Vector2(0.0f, shiftFloorLimit);
                return;
            }
            shiftValue -= shiftRate;
            cameraPivot.transform.position = new Vector2(0.0f, shiftValue);
        }
    }

    public void ShiftUp()
    {
        if (CheckIfShifting())
            return;

        isShiftingUp = true;
    }

    public void ShiftDown()
    {
        if (CheckIfShifting())
            return;

        isShiftingDown = true;
    }

    private bool CheckIfShifting()
    {
        if (isShiftingUp || isShiftingDown)
            return true;
        return false;
    }
}
