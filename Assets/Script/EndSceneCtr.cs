using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EndSceneCtr : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {

        SetPlayerPos();


    }

    private void SetPlayerPos()
    {
        if (IsServer)
            AllPlayerControl.instance.HideAllPlayer();

        if (NetworkPlayer.ownPlayer == null)
            return;

        NetworkPlayer.ownPlayer.Tele(Vector3.zero);
        


    }
}
