using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayZonesCtr : MonoBehaviour
{
    [SerializeField] private PlayZone[] playZones;

    public static PlayZonesCtr instance;
    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        SetPlayerPosAndZone();
        //SetPlayZone();

    }

    private void SetPlayerPosAndZone()
    {
        if (NetworkPlayer.ownPlayer == null)
            return;

        int id = (int)NetworkPlayer.ownPlayer.OwnerClientId-1;
        NetworkPlayer.ownPlayer.Tele(TargetPos(id));
        playZones[id].SetPlayerable();
    }

    //private void SetPlayZone()
    //{
    //    if (!IsClient)
    //        return;

    //    playZones[(int)OwnerClientId-1].SetPlayerable();
    //}


    private Transform TargetPos(int id)
    {
       
        return playZones[id].GetTransform();
       


    }

}
