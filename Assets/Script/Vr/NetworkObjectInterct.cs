using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(VRRigReferences))]
public class NetworkObjectInterct : MonoBehaviour
{
    VRRigReferences vrRigRef;

    private void Awake()
    {
        vrRigRef = GetComponent<VRRigReferences>();
    }

    public void OnSelectableGrap(SelectEnterEventArgs ergs)
    {




        NetworkObject networkObject = ergs.interactableObject.transform.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.GetComponent<VRSyncCollision>().isBeingGrap = true;
            vrRigRef.networkPlayer.RequestOwnershipServerRpc(networkObject);
        }
    }

    public void OnSelectableExit(SelectExitEventArgs ergs)
    {

        if (!ergs.interactableObject.transform.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
            return;
        if (networkObject.IsOwner)
        {
            networkObject.GetComponent<VRSyncCollision>().isBeingGrap = false;
            //   networkObject.SynchronizeTransform = false;
            vrRigRef.networkPlayer.CancelOwnershipServerRpc(networkObject, networkObject.GetComponent<Rigidbody>().velocity);
        }
    }



}


