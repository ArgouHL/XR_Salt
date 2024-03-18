using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    private VRRigReferences vrRig => VRRigReferences.instance;

    private Renderer[] meshs;
    [SerializeField] private Collider[] colliders;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            DisableMeshs();
            Debug.Log(OwnerClientId);
            vrRig.SetNetworkPlayer(this);
            


        }
        else
        {
            EnableCollider();
        }

    }

    private void EnableCollider()
    {
        foreach (var c in colliders)
        {
            c.isTrigger = true;
        }
    }

    private void DisableMeshs()
    {
        meshs = GetComponentsInChildren<Renderer>();
        foreach (var m in meshs)
        {
            m.enabled = false;
        }
    }

    private void Update()
    {
        if (!IsOwner)
            return;
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        root.position = vrRig.root.position;
        root.rotation = vrRig.root.rotation;
        head.position = vrRig.head.position;
        head.rotation = vrRig.head.rotation;
        leftHand.position = vrRig.leftHand.position;
        leftHand.rotation = vrRig.leftHand.rotation;
        rightHand.position = vrRig.rightHand.position;
        rightHand.rotation = vrRig.rightHand.rotation;
    }





    [ServerRpc]
    public void RequestOwnershipServerRpc(NetworkObjectReference networkObjectRef)
    {
        Debug.Log("test");
        if (networkObjectRef.TryGet(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(OwnerClientId);
            Debug.Log(" chang to Player ID:" + OwnerClientId);

        }
        else
        {
            Debug.LogWarning("no chang,Player ID:" + OwnerClientId);
        }

    }


    [ServerRpc]
    public void CancelOwnershipServerRpc(NetworkObjectReference networkObjectRef, Vector3 velocity)
    {
        Debug.Log("test");
        if (networkObjectRef.TryGet(out NetworkObject networkObject))
        {
           // networkObject.SynchronizeTransform = false;
           // networkObject.RemoveOwnership();
          
           // networkObject.transform.GetComponent<Rigidbody>().velocity = velocity;
         //   Debug.Log("Owner back server");
          //  EnableTranformSyncClientRpc(networkObjectRef);



        }
        else
        {
            Debug.LogWarning("no chang,Player ID:" + OwnerClientId);
        }

    }


   
}
