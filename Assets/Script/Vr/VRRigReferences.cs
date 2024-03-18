using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRigReferences : MonoBehaviour
{
    public static VRRigReferences instance;

    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    internal NetworkPlayer networkPlayer;

    private void Awake()
    {
        instance = this;
    }




    public void Deactive()
    {
        gameObject.SetActive(false);
    }

    internal void SetNetworkPlayer(NetworkPlayer _networkPlayer)
    {
        networkPlayer = _networkPlayer;
    }
}
