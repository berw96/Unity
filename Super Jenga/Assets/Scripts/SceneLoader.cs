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
    private enum SCENE_TRANSITION_MODE
    {
        LOADING,
        RESET,
        QUIT,
        NEXT_SCENE,
        PREV_SCENE
    }

    private static Scene[] scenes;
    public readonly float maxOverlayFill = 1.0f;
    public readonly float minOverlayFill = 0.0f;
    public readonly float maxOverlayFillRate = 0.025f;
    private float waitTime;
    public readonly float maxWaitTime = 1.0f;
    [SerializeField] Image sceneTransitionOverlay;
    private SCENE_TRANSITION_MODE mode;
    private int targetSceneIndex;

    private void Awake()
    {
        mode = SCENE_TRANSITION_MODE.LOADING;
        RegisterAllScenes();
        targetSceneIndex = SceneManager.GetActiveScene().buildIndex;
        waitTime = 0.0f;
        sceneTransitionOverlay.fillAmount = maxOverlayFill;
        sceneTransitionOverlay.raycastTarget = true;
        sceneTransitionOverlay.maskable = true;
        sceneTransitionOverlay.enabled = true;
    }

    private void FixedUpdate()
    {
        waitTime += Time.fixedDeltaTime;

        switch (mode)
        {
            case SCENE_TRANSITION_MODE.LOADING:
                if (waitTime >= maxWaitTime)
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
                        return;
                    }
                }
                break;
            default:
                switch (mode)
                {
                    case SCENE_TRANSITION_MODE.RESET:
                        targetSceneIndex = SceneManager.GetActiveScene().buildIndex;
                        break;
                    case SCENE_TRANSITION_MODE.QUIT:
                        ExitApplication();
                        break;
                    case SCENE_TRANSITION_MODE.NEXT_SCENE:
                        targetSceneIndex++;
                        break;
                    case SCENE_TRANSITION_MODE.PREV_SCENE:
                        targetSceneIndex--;
                        break;
                }

                sceneTransitionOverlay.raycastTarget = true;
                sceneTransitionOverlay.maskable = true;
                sceneTransitionOverlay.enabled = true;

                if (waitTime >= maxWaitTime)
                {
                    waitTime = maxWaitTime;
                    if (sceneTransitionOverlay.fillAmount < maxOverlayFill)
                    {
                        sceneTransitionOverlay.fillAmount += maxOverlayFillRate;
                        return;
                    }
                    else
                    {
                        sceneTransitionOverlay.fillAmount = maxOverlayFill;
                        LoadScene(targetSceneIndex);
                        return;
                    }
                }
                break;
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

    public void SetSceneTransitionMode(GameObject obj)
    {
        waitTime = 0.0f;

        switch (obj.name)
        {
            case "Reset":
                mode = SCENE_TRANSITION_MODE.RESET;
                break;
            case "Quit":
                mode = SCENE_TRANSITION_MODE.QUIT;
                break;
            case "Continue":
                mode = SCENE_TRANSITION_MODE.NEXT_SCENE;
                break;
            case "Back":
                mode = SCENE_TRANSITION_MODE.PREV_SCENE;
                break;
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
