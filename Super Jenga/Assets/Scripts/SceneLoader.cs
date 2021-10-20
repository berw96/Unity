﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : ScriptableObject
{
    private static Scene[] scenes;

    private void Awake()
    {
        RegisterAllScenes();
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
}
