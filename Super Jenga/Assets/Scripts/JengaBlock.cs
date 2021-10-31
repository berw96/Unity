#define JENGA_BLOCK
#if (UNITY_2019_3_OR_NEWER && JENGA_BLOCK)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static BlockManagement.SelectedBlock;
using static BlockManagement.InventoriedBlock;
using static CameraManagement.MoveCamera;

public class JengaBlock : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] AudioClip clip;

    [Header("GFX")]
    [SerializeField] Renderer renderer;
    private MeshRenderer meshRenderer;
    private static float colorLerpRate = 0.05f;

    private static GameObject dragPoint;

    private static GameObject cameraPivotReference;
    private readonly Color defaultColor = Color.white;
    private readonly Color hoverColor = Color.grey;
    private readonly Color selectedColor = Color.red;
    private readonly Color inventoriedColor = Color.green;
    private float colorLerpProgress = 0.0f;

    private bool isHovered = false;
    private bool isDragging = false;
    private int clicks = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        dragPoint = GameObject.Find("MouseDragPoint");
    }

    private void InitializeClicks()
    {
        clicks = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        {
            {
                if (!gameObject.Equals(GetSelectedBlock()))
                    LerpToColor(defaultColor);
                else
                    LerpToColor(selectedColor);
            }

            {
                if (isHovered &&
                    !gameObject.Equals(GetSelectedBlock()))
                    LerpToColor(hoverColor);
            }

            {
                if (gameObject.Equals(GetInventoriedBlock()))
                    LerpToColor(inventoriedColor);
            }

            {
                if (isDragging)
                {
                    Debug.Log("Drag point should be moving...");
                    dragPoint.transform.position = Input.mousePosition;
                }
            }
        }
    }

    private void OnMouseDown()
    {
        {
            if (isHovered &&
            !gameObject.Equals(GetSelectedBlock()))
            {
                if(!gameObject.Equals(GetInventoriedBlock()))
                    RemoveInventoriedBlock();
                InitializeClicks();
                clicks++;
                SetSelectedBlock(gameObject);
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
                return;
            }
            else if (isHovered &&
                gameObject.Equals(GetSelectedBlock()))
            {
                clicks++;
                if (clicks == 2)
                    AddBlockToInventory(gameObject);
                if (clicks >= int.MaxValue - 1)
                    clicks = int.MaxValue - 1; 
                return;
            }
        }
    }

    private void OnMouseUp()
    { 
        Debug.Log("Click up");
        Debug.Log($"Clicks provided = {clicks}");
        Cursor.visible = true;
        isDragging = false;
    }

    private void OnMouseOver()
    {
        isHovered = true;
        Debug.Log("Hovering");
    }

    private void OnMouseExit()
    {
        isHovered = false;
        Debug.Log("Not hovering");
    }

    private void OnMouseDrag()
    {
        if (GetInventoriedBlock() != null)
        {
            isDragging = true;
            cameraPivotReference = GetCameraPivot();
            float dragAngle = cameraPivotReference.transform.rotation.eulerAngles.y;

            Debug.Log($"Dragging at {dragAngle} degrees");
            Cursor.visible = false;

            Debug.Log($"Mouse position: [{Input.mousePosition.normalized}]");

            // range-based angle detection to avoid bugs caused by floating point leakage.
            {
                if (dragAngle >= 44.5f &&
                dragAngle <= 45.5f)
                    Debug.Log("45 DRAG");

                if (dragAngle >= 134.5f &&
                    dragAngle <= 135.5f)
                    Debug.Log("135 DRAG");

                if (dragAngle >= 224.5f &&
                    dragAngle <= 225.5f)
                    Debug.Log("225 DRAG");

                if (dragAngle >= 314.5f &&
                    dragAngle <= 315.5f)
                    Debug.Log("315 DRAG");
            }
        }
    }

    private void LerpToColor(Color target)
    {
        {
            if (renderer.material.color != target)
            {
                Debug.Log($"Lerping colors...progress [{colorLerpProgress * 100}%]");
                renderer.material.color = Color.Lerp(
                    renderer.material.color,
                    target,
                    colorLerpProgress += colorLerpRate
                    );

                {
                    if (colorLerpProgress >= 1.0f)
                        colorLerpProgress = 1.0f;
                }
            }
            else
            {
                renderer.material.color = target;
                colorLerpProgress = 0.0f;
                return;
            }
        }
    }

    private void LerpToDragPoint()
    {

    }
}
#endif
