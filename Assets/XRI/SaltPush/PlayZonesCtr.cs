using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayZonesCtr : NetworkBehaviour
{
    [SerializeField] private PlayZone[] playZones;

    public static PlayZonesCtr instance;
    private void Awake()
    {
        instance = this;

    }

    public override void OnNetworkSpawn()
    {

        SetPlayerPosAndZone();
        //SetPlayZone();

    }

    private void SetSaltZone()
    {
        for (ulong i = 0; i < 8; i++)
        {
            playZones[i].SetSaltMountOwner(i + 1);
        }
    }

    private void SetPlayerPosAndZone()
    {

        if (NetworkPlayer.ownPlayer == null)
            return;

        int id = (int)NetworkPlayer.ownPlayer.OwnerClientId - 1;
        NetworkPlayer.ownPlayer.Tele(TargetPos(id));
        playZones[id].SetPlayerable();
        RequestSaltZoneServerRpc(NetworkPlayer.ownPlayer.OwnerClientId);
    }

    internal List<KeyValuePair<ulong, float>> GetScores()
    {
        List<KeyValuePair<ulong, float>> _list = new List<KeyValuePair<ulong, float>>();
        foreach (var z in playZones)
        {
            var s = z.GetComponentInChildren<SaltMount>();
            _list.Add(new KeyValuePair<ulong, float>(s.GetOwner() , s.GetVolume()));          
        }
        return _list;

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

    [ServerRpc(RequireOwnership = false)]
    private void RequestSaltZoneServerRpc(ulong id)
    {
        playZones[id - 1].SetSaltMountOwner(id);
    }

   
}
