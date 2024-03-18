using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerTest : NetworkBehaviour
{
    private NetworkVariable<int> num=new NetworkVariable<int>(1,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

    private InputActionMap player;
    private Vector2 currentMovement => player.FindAction("Move").ReadValue<Vector2>();
    private bool IsPressed => currentMovement.magnitude > 0f;
    [SerializeField] private InputActionAsset inputAsset;



    //NetWorkAwake
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();       
        player = inputAsset.FindActionMap("Player");
        player.Enable();

        num.OnValueChanged += (int previousValue, int newValue) => { Debug.Log(OwnerClientId + ";" + newValue); };

    }



    void Update()
    {      
        if (!IsOwner)
            return;
        if (Input.GetKey(KeyCode.Space))
        {
            var g = GameObject.Find("TestCube").GetComponent<NetworkObject>();
            ChangeOwnershipServerRpc(g,OwnerClientId);
            g.transform.position += Vector3.up;
        }
            
        //if(IsPressed)
        //{
        //    transform.position += new Vector3(currentMovement.x, 0, currentMovement.y) * Time.deltaTime * 5;
        //}
        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.position += Vector3.forward * Time.deltaTime * 5;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.position -= Vector3.forward * Time.deltaTime * 5;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.position -= Vector3.right * Time.deltaTime * 5;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.position += Vector3.right * Time.deltaTime * 5;
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestServerRpc(new ServerRpcParams());
            //  num.Value = Random.Range(1, 10);
        }

 
    }

    #region ServerRPC
    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        TestServerRpc(serverRpcParams.Receive.SenderClientId.ToString());
    }

    [ServerRpc]
    private void ChangeOwnershipServerRpc(NetworkObjectReference g, ulong ownerClientId)
    {
       if(g.TryGet(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(ownerClientId);
        }
       else
        {
            Debug.LogWarning("Change fail");
        }
    }

    [ServerRpc]
    private void TestServerRpc(string message)
    {
        Debug.Log("TestingPrc " + OwnerClientId+";"+message);
    }
    #endregion


}
