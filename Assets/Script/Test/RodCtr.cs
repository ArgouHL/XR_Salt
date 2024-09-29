using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RodCtr : MonoBehaviour
{

    private XRGrabInteractable xrGrabInteractable => GetComponent<XRGrabInteractable>();
    [SerializeField] private GameObject digger;
    
    private void OnEnable ()
    {
        xrGrabInteractable.selectEntered.AddListener(EnableDigger);
        xrGrabInteractable.selectExited.AddListener(DisableDigger);
        
    }

    private void OnDisable()
    {
        xrGrabInteractable.selectEntered.RemoveAllListeners() ;
        xrGrabInteractable.selectExited.RemoveAllListeners();

    }
    public void EnableDigger(SelectEnterEventArgs arg0)
    {
        digger.SetActive(true);
    }
    private void DisableDigger(SelectExitEventArgs arg0)
    {
        digger.SetActive(false);
    }

    [ContextMenu("TestGet")]
    public void TestGet()
    {
        NetworkPlayer.ownPlayer.ChangeOwnerShip(GetComponent<NetworkObject>());
        digger.SetActive(true);
        //GetComponent<NetworkObject>().ChangeOwnership((ulong) 1);
    }

    
}
