using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class VRSyncCollision : MonoBehaviour
{

    internal bool isBeingGrap=false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("vrHand"))
            return;
        if (isBeingGrap)
            return;
        var networkObj = GetComponent<NetworkObject>();

        networkObj.RemoveOwnership();
        Debug.Log("Owner back server");
    }



    [ClientRpc]
    public void EnableTranformSyncClientRpc(NetworkObjectReference networkObjectRef)
    {
        if (networkObjectRef.TryGet(out NetworkObject networkObject))
        {
            networkObject.SynchronizeTransform = true;
        }
    }
}
