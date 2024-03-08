using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerControl : NetworkBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestClientRpc();
            //  num.Value = Random.Range(1, 10);
        }
    }


    #region ClientRPC
    [ClientRpc]
    private void TestClientRpc()
    {
        TestClientRpc("");
    }

    [ClientRpc]
    private void TestClientRpc(string message)
    {
        Debug.Log("TestingPrc " + OwnerClientId + message);
    }
    #endregion 
}
