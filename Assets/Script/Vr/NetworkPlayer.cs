using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    private VRRigReferences vrRig => VRRigReferences.instance;

    private Renderer[] meshs;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private NameFaceCamera nameShow;

    private NetworkVariable<int> charaDataIndex = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    

    public static NetworkPlayer ownPlayer;

    private delegate void UpdateAction();
    private UpdateAction OnUpdate;

    private void OnDisable()
    {
        if(IsOwner)
        {
            OnUpdate -= UpdateTransform;
        }
    }

    public override void OnNetworkSpawn()
    {
      
        NetworkManager.OnClientConnectedCallback += AddPlayerCam;
       
        base.OnNetworkSpawn();
        if (IsServer)
        {
            charaDataIndex.Value = -1;
            HidePlayer();
        }
        else
        {
            SetChara(0, charaDataIndex.Value);
        }


        if (IsOwner)
        {

            Debug.Log(OwnerClientId);
            vrRig.SetNetworkPlayer(this);
            OnUpdate += UpdateTransform;
            
            //charaDataIndex.Value = PlayerDataContainer.charaDataIndex;
            ownPlayer = this;
            // CharaChangeServerRpc();


        }
        else
        {
            EnableCollider();
        }

        charaDataIndex.OnValueChanged += SetChara;

        //SetChara();
    }



    private void HidePlayer()
    {
        nameShow.ChangeName("");

        meshs = GetComponentsInChildren<Renderer>();
        foreach (var m in meshs)
        {
            m.enabled = false;
        }

    }

    public void SetCharaData(int charaDataIndex)
    {
        CharaChangeServerRpc(charaDataIndex);
        //charaDataIndex.Value = charaDataIndex;

        //var charaData=CharaDatas.GetCharaData(charaDataIndex);
        //SetChara();

    }

    private void EnableCollider()
    {
        foreach (var c in colliders)
        {
            c.isTrigger = true;
        }
    }

    //private void HideChara()
    //{
    //    meshs = GetComponentsInChildren<Renderer>();
    //    foreach (var m in meshs)
    //    {
    //        m.enabled = false;
    //    }
    //}

    private void Update()
    {
        OnUpdate?.Invoke();
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

    [ServerRpc]
    public void CharaChangeServerRpc(int _charaDataIndex)
    {
        charaDataIndex.Value = _charaDataIndex;
        // SetChara();

    }

    private void SetChara(int previousValue, int newValue)
    {
        if (newValue < 0)
        {
            HidePlayer();
            return;
        }

        var data = CharaDatas.GetCharaData(newValue);
        //TurnONChara
        SetCharaSkin(data.charaSkinColor);
        nameShow.ChangeName(data.charaterName);

    }

    private void SetCharaSkin(Color charaSkinColor)
    {
        meshs = GetComponentsInChildren<Renderer>();
        foreach (var m in meshs)
        {
            m.enabled = true;
            m.material.color = charaSkinColor;
        }
    }


    internal void AddPlayerCam(ulong player)
    {
        if (!IsOwner)
            return;
        AddCheckCameraServerRpc();
    }

  


    [ServerRpc]
    private void AddCheckCameraServerRpc()
    {
        CameraControl.instance.AddPlayer((int)OwnerClientId,head);
    }

   

    internal void Tele(Transform targetTransform)
    {
       TeleportRequest teleportRequest = new TeleportRequest();

        teleportRequest.destinationPosition = targetTransform.position;
        teleportRequest.destinationRotation = targetTransform.rotation;
        FindObjectOfType<TeleportationProvider>().QueueTeleportRequest(teleportRequest);
        ////VRCtr.instance.Teleport(targetTransform);
    }
}
