using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaSelectCtr : MonoBehaviour
{
    public static CharaSelectCtr instance;
    public  CharatarSelectEventer[] eventers;
    public GameObject hints;
    private int selectedCount = 0;
    public int targetCount = 1;
    public static Dictionary<ulong,int> selectedCharaDict;
    private void Awake()
    {
        instance = this;
        eventers = GetComponentsInChildren<CharatarSelectEventer>();
        selectedCount = 0;
        selectedCharaDict = new Dictionary<ulong, int>();
    }

    internal void AllDisapper()
    {
        foreach(var eventer in eventers)
        {
            eventer.Unshow();
            Debug.Log("s");
        }
        hints.SetActive(false);
    }

    internal void AddSelected(ulong id,int charaIndex)
    {
        selectedCharaDict.Add(id, charaIndex);
        if (selectedCount >= targetCount)
            return;
        selectedCount++;
        if(selectedCount>= targetCount)
        {
            SelectSceneEvent.instance.ChanEvnet();
        }
            
    }

    internal static int GetCharaIndex(ulong id)
    {
        return selectedCharaDict[id];
    }
}
