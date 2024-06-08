using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSceneEvent : MonoBehaviour
{
    public static SelectSceneEvent instance;
    public delegate void SceneEvent();
    public SceneEvent ChanShowEvent;


    private void Awake()
    {
        instance = this;
    }

    private void OnDisable()
    {
        ChanShowEvent = null;
 
    }

    public void ChanEvnet()
    {
        ChanShowEvent?.Invoke();
    }

    public void ChanEvnetEnd()
    {
        ChanShowEvent?.Invoke();
    }

}
