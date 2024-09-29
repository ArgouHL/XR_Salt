using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CharaSelectCtr : MonoBehaviour
{
    public static CharaSelectCtr instance;
    public  CharatarSelectEventer[] eventers;
    public GameObject hints;
    int selectedCount=0;
    //public int targetCount = 1;
    public static Dictionary<ulong,int> selectedCharaDict;
    public CharaterObj[] charaSets;


    public delegate void ServerEvent(string s);
    public static ServerEvent onPlayerSelect;
    public static ServerEvent ShowUI;
    public static ServerEvent HideUI;

    private void Awake()
    {
        instance = this;
        eventers = GetComponentsInChildren<CharatarSelectEventer>();

        selectedCharaDict = new Dictionary<ulong, int>();
        for(int i=0;i< eventers.Count();i++)
        {
            eventers[i].SetSkinAndName(charaSets[i]);
        }
    }
    private void Start()
    {

        onPlayerSelect?.Invoke(selectedCount + "/" + NetworkManager.Singleton.ConnectedClients.Count);
       
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
       
        selectedCount++;
        onPlayerSelect?.Invoke(selectedCount + "/" + NetworkManager.Singleton.ConnectedClients.Count);
    }

    internal static int GetCharaIndex(ulong id)
    {
        return selectedCharaDict[id];
    }

    public void PlayChans()
    {
        Debug.Log(selectedCount);
        Debug.Log(NetworkManager.Singleton.ConnectedClients.Count);
        if (selectedCount != NetworkManager.Singleton.ConnectedClients.Count)
            return;
        AllDisapper();
        SelectSceneEvent.instance.ChanEvnet();

    }


}
