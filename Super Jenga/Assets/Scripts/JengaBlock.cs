#define JENGA_BLOCK
#if (UNITY_2019_3_OR_NEWER && JENGA_BLOCK)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static BlockManagement.SelectedBlock;

/// <summary>
/// <c>InputManager</c> maps mouse and touch screen inputs detected by Unity
/// to triggering SFX and RigidBody physics associated with each Jenga block prefab.
/// </summary>
public class JengaBlock : MonoBehaviour
{
    private const int numberOfJengaLayers = 18;
    private const int numberOfJengaBlocks = 54;
    private const int numberOfJengaSlots = 108;

    private readonly Color defaultColor = Color.white;
    private readonly Color hoverColor = Color.grey;
    private readonly Color selectedColor = Color.red;
    private float colorLerpProgress;

    [Header("SFX")]
    [SerializeField] AudioClip clip;

    [Header("GFX")]
    [SerializeField] Renderer renderer;
    private MeshRenderer meshRenderer;
    private static float colorLerpRate = 0.05f;

    private bool isHovered;
    private bool isHeld;

    // Start is called before the first frame update
    private void Awake()
    {
        colorLerpProgress = 0.0f;
        meshRenderer = GetComponent<MeshRenderer>();
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
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Click down");
        {
            if (isHovered &&
            !gameObject.Equals(GetSelectedBlock()))
            {
                SetSelectedBlock(gameObject);
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
                return;
            }
            else if (isHovered &&
                gameObject.Equals(GetSelectedBlock()))
            {
                Debug.LogWarning($"{gameObject} is already selected.");
                DeselectCurrentBlock();
            }
        }
    }

    private void OnMouseUp()
    { 
        Debug.Log("Click up");
        isHeld = false;
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

    private void LerpToColor(Color target)
    {
        {
            if (renderer.material.color != target)
            {
                Debug.Log($"Lerping colors...progress [{colorLerpProgress}]");
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
}
#endif
