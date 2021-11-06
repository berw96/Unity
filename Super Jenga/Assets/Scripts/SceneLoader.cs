#define SCENE_LOADER
#if (UNITY_2019_3_OR_NEWER && SCENE_LOADER)

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private static Scene[] scenes;
    public readonly float maxOverlayFill = 1.0f;
    public readonly float minOverlayFill = 0.0f;
    public readonly float maxOverlayFillRate = 0.05f;
    private float waitTime = 0.0f;
    public readonly float maxWaitTime = 1.0f;
    [SerializeField] Image sceneTransitionOverlay;

    private void Awake()
    {
        RegisterAllScenes();
        sceneTransitionOverlay.fillAmount = maxOverlayFill;
        sceneTransitionOverlay.raycastTarget = true;
        sceneTransitionOverlay.maskable = true;
        sceneTransitionOverlay.enabled = true;
    }

    private void FixedUpdate()
    {
        waitTime += Time.fixedDeltaTime;
        {
            if(waitTime >= maxWaitTime)
            {
                waitTime = maxWaitTime;
                if (sceneTransitionOverlay.fillAmount > minOverlayFill)
                {
                    sceneTransitionOverlay.fillAmount -= maxOverlayFillRate;
                    return;
                }
                else
                {
                    sceneTransitionOverlay.fillAmount = minOverlayFill;
                    sceneTransitionOverlay.raycastTarget = false;
                    sceneTransitionOverlay.maskable = false;
                    sceneTransitionOverlay.enabled = false;
                }
            }
        }
    }

    private void RegisterAllScenes()
    {
        scenes = new Scene[SceneManager.sceneCountInBuildSettings];
        Exception e = new Exception("Index off by one error detected.");
        Debug.Log("Recognized scenes in the build: " + scenes.Length);
        for (int i = 0; i < scenes.Length; i++)
        {
            if (i >= scenes.Length)
            {
                throw e;
            }
            scenes[i] = SceneManager.GetSceneByBuildIndex(i);
            Debug.Log("Got a scene: " + scenes[i].name);
            //Be sure that the scenes you want registered are opened in the hierarchy.
        }
    }

    public void LoadScene(int index)
    {
        try
        {
            SceneManager.LoadScene(scenes[index].name);
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogException(e);
        }
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public float GetOverlayFillAmount()
    {
        return sceneTransitionOverlay.fillAmount;
    }
}
#endif
