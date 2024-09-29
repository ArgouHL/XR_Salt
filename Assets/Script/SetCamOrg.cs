using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SetCamOrg : NetworkBehaviour
{
    public Transform camPos;
    private void Start()
    {
        if (!IsServer)
            return;
        AllPlayerControl.instance.SetCamOrgPos(camPos);
    }
}
