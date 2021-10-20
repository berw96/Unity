#define INPUT_MANAGER
#if (UNITY_2019_3_OR_NEWER && INPUT_MANAGER)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// <c>InputManager</c> maps mouse and touch screen inputs detected by Unity
/// to triggering SFX and RigidBody physics associated with each Jenga block prefab.
/// </summary>
public class InputManager : MonoBehaviour
{
    private const int numberOfJengaLayers = 18;

    [Header("SFX")]
    [SerializeField] AudioClip clip;
    private AudioManager am;
    
    [Header("GFX")]
    [SerializeField] Renderer renderer;
    private MeshRenderer meshRenderer;

    private bool isHovered;
    private bool isSelected;

    // Start is called before the first frame update
    private void Awake()
    {
        am = ScriptableObject.CreateInstance<AudioManager>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isSelected)
        {
            renderer.material.color = Color.white;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Click down");

        if (isHovered && !isSelected)
        {
            isSelected = true;
            am.PlayClip(clip, gameObject);
            renderer.material.color = Color.red;
            meshRenderer.enabled = true;
            return;
        }
            

        if (isSelected)
            isSelected = false;
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
