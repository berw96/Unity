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
    [SerializeField] AudioClip hoveredSFX;
    [SerializeField] AudioClip selectedSFX;
    [SerializeField] AudioClip inventoriedSFX;
    [SerializeField] AudioClip collisionSFX;

    [Header("GFX")]
    [SerializeField] Renderer renderer;
    private static float colorLerpRate = 0.05f;

    [Header("Physics")]
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] BoxCollider collider;
    [SerializeField] PhysicMaterial material;

    private static GameObject cameraPivotReference;
    private readonly Color defaultColor = new Color(215.0f / 255.0f, 166.0f / 255.0f, 104.0f / 255.0f);
    private readonly Color hoverColor = Color.grey;
    private readonly Color selectedColor = Color.red;
    private readonly Color inventoriedColor = Color.green;
    private float colorLerpProgress = 0.0f;

    private static GameObject dragPoint;

    private bool isHovered = false;
    private bool isDragging = false;
    private int clicks = 0;

    private static float blockLerpRate = 0.000005f;
    private float blockLerpProgress = 0.0f;
    private Vector3 initBlockPosition;
    private Vector3 initBlockRotation;
    private Vector3 clickPoint;

    // Start is called before the first frame update
    private void Awake()
    {
        dragPoint = GameObject.Find("MouseDragPoint");
    }

    private void InitializeClicks()
    {
        clicks = 0;
    }

    private void FixedUpdate()
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
                    !gameObject.Equals(GetSelectedBlock()) &&
                    GetInventoriedBlock() == null)
                    LerpToColor(hoverColor);
            }

            {
                if (gameObject.Equals(GetInventoriedBlock()))
                {
                    LerpToColor(inventoriedColor);
                    rigidbody.isKinematic = true;
                }
                else
                    rigidbody.isKinematic = false;
            }
        }
    }

    private void OnMouseDown()
    {
        clickPoint = Input.mousePosition;
        initBlockPosition = gameObject.transform.position;
        initBlockRotation = gameObject.transform.rotation.eulerAngles;

        {
            if (isHovered &&
            !gameObject.Equals(GetSelectedBlock()))
            {
                if (!gameObject.Equals(GetInventoriedBlock()))
                    RemoveInventoriedBlock();
                InitializeClicks();
                clicks++;
                SetSelectedBlock(gameObject);
                AudioSource.PlayClipAtPoint(selectedSFX, Camera.main.transform.position);
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
        isDragging = false;
        Debug.Log("Click up");
        Debug.Log($"Clicks provided = {clicks}");
        Cursor.visible = true;
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

            AdjustDragPoint(dragAngle);
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

    private void LerpToPoint(Vector3 target)
    {
        {
            if (gameObject.transform.position != target)
            {
                gameObject.transform.position = Vector3.Lerp(
                    gameObject.transform.position,
                    target,
                    blockLerpProgress += blockLerpRate
                    );

                {
                    if (blockLerpProgress >= 1.0f)
                        blockLerpProgress = 1.0f;
                }
            }
            else
            {
                gameObject.transform.position = target;
                blockLerpProgress = 0.0f;
                return;
            }
        }
    }

    private void AdjustDragPoint(float dragAngle)
    {
        // declare Vector3 to store new drag point.
        Vector3 newPoint = new Vector3();

        // we all float down here ;)
        float mouseDX = Input.mousePosition.x - clickPoint.x;
        float mouseDY = Input.mousePosition.y - clickPoint.y;
        float blockDX = (initBlockPosition.x + mouseDX);
        float blockDZ = (initBlockPosition.z + mouseDY);

        // range-based angle detection to avoid bugs caused by floating point leakage.
        // sets drag point target based on camera pivot angle.
        {
            if (dragAngle >= 44.5f &&
                dragAngle <= 45.5f)
            {
                newPoint = new Vector3(
                    blockDX,
                    initBlockPosition.y,
                    blockDZ
                    );

                Debug.Log("45 DRAG");
            }
        }

        {
            if (dragAngle >= 134.5f &&
                dragAngle <= 135.5f)
            {
                newPoint = new Vector3(
                    blockDZ,
                    initBlockPosition.y,
                    -blockDX
                    );

                Debug.Log("135 DRAG");
            }
        }

        {
            if (dragAngle >= 224.5f &&
                dragAngle <= 225.5f)
            {
                newPoint = new Vector3(
                    -blockDX,
                    initBlockPosition.y,
                    -blockDZ
                    );

                Debug.Log("225 DRAG");
            }
        }

        {
            if (dragAngle >= 314.5f &&
                dragAngle <= 315.5f)
            {
                newPoint = new Vector3(
                    -blockDZ,
                    initBlockPosition.y,
                    blockDX
                    );

                Debug.Log("315 DRAG");
            }
        }
        
        dragPoint.transform.position = newPoint;
        
        {
            if (dragPoint.transform.position != null)
                LerpToPoint(dragPoint.transform.position);
            else
                Debug.LogWarning("Drag point is set to null. " +
                    "Unable to specify where block should be moved.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Block collider ENTER");
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Block collider EXIT");
    }
}
#endif
