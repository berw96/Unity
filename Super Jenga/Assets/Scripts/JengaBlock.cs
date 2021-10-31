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

    [Header("SFX")]
    [SerializeField] AudioClip clip;

    [Header("GFX")]
    [SerializeField] Renderer renderer;
    private MeshRenderer meshRenderer;

    private bool isHovered;
    private bool isSelected;

    // Start is called before the first frame update
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        {
            if (!gameObject.Equals(GetSelectedBlock()))
                renderer.material.color = defaultColor;
            else
                renderer.material.color = selectedColor;

            if (isHovered &&
                !isSelected)
                renderer.material.color = hoverColor;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Click down");

        if (isHovered &&
            !gameObject.Equals(GetSelectedBlock()))
        {
            isSelected = true;
            SetSelectedBlock(gameObject);
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            return;
        }
        else
        {
            isSelected = false;
            Debug.LogWarning($"{gameObject} is already selected.");
        }

        // enable block to be deselected
        if (gameObject.Equals(GetSelectedBlock()))
        {
            DeselectCurrentBlock();
        }
    }

    private void OnMouseUp()
    { 
        Debug.Log("Click up");
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
}
#endif
