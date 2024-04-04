using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneObjSingleton<T> : MonoBehaviour
{
    public static SceneObjSingleton<T> instance;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        instance.Hide();
    }

    protected virtual void Hide()
    {
        
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.Show();
    }

    protected virtual void Show()
    {

    }
}
