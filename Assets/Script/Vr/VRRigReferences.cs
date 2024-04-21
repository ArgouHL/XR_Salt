using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRRigReferences : MonoBehaviour
{
    public static VRRigReferences instance;

    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    internal NetworkPlayer networkPlayer;
    public GameObject leftHandOrgModel;
    public GameObject righHandOrgModelt;
    
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

    internal void UnShow()
    {
        leftHandOrgModel.SetActive(false);
        righHandOrgModelt.SetActive(false);
    }


}
