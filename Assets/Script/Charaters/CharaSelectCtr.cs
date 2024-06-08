using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaSelectCtr : MonoBehaviour
{
    public static CharaSelectCtr instance;
    public  CharatarSelectEventer[] eventers;
    private int selectedCount = 0;
    public int targetCount = 1;
    private void Awake()
    {
        instance = this;
        eventers = GetComponentsInChildren<CharatarSelectEventer>();
        selectedCount = 0;
    }

    internal void AllDisapper()
    {
        foreach(var eventer in eventers)
        {
            eventer.Unshow();
            Debug.Log("s");
        }
    }

    internal void AddCount()
    {
        if (selectedCount >= targetCount)
            return;
        selectedCount++;
        if(selectedCount>= targetCount)
        {
            SelectSceneEvent.instance.ChanEvnet();
        }
            
    }
}
