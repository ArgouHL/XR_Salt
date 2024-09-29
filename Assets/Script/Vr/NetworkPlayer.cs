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
    [SerializeField] internal Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private GameObject model;


    private VRRigReferences vrRig => VRRigReferences.instance;

    private Renderer[] meshs;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private NameFaceCamera nameShow;

    private NetworkVariable<int> charaDataIndex = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<bool> selected = new NetworkVariable<bool>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    public static NetworkPlayer ownPlayer;

    private delegate void UpdateAction();
    private UpdateAction OnUpdate;

    private void OnDisable()
    {
        if (IsOwner)
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
            selected.Value = false;

        }

        SetChara(0, charaDataIndex.Value);



        if (IsOwner)
        {

            Debug.Log(OwnerClientId);
            vrRig.SetNetworkPlayer(this);
            OnUpdate += UpdateTransform;

            //charaDataIndex.Value = PlayerDataContainer.charaDataIndex;
            ownPlayer = this;
            // CharaChangeServerRpc();
            Tele(new Vector3(0, 0, -3));
            model.SetActive(false);
        }
        else
        {
            //  EnableCollider();
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
    private void ShowPlayer(int newValue)
    {
        var data = CharaDatas.GetCharaData(newValue);
        //TurnONChara
        SetCharaSkin(data.charaSkinMaterial);
        nameShow.ChangeName(data.charaterName);

        meshs = GetComponentsInChildren<Renderer>();
        foreach (var m in meshs)
        {
            m.enabled = true;
        }
        if (IsOwner)
        {
            vrRig.UnShow();
        }
    }

    //public void SetCharaData(int charaDataIndex)
    //{
    //    CharaChangeServerRpc(charaDataIndex);
    //    //charaDataIndex.Value = charaDataIndex;

    //    //var charaData=CharaDatas.GetCharaData(charaDataIndex);
    //    //SetChara();

    //}

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


    internal void CharaChange(int _charaDataIndex)
    {
        charaDataIndex.Value = _charaDataIndex;
        selected.Value = true;

    }


    private void SetChara(int previousValue, int newValue)
    {
        if (newValue < 0)
        {
            HidePlayer();
            return;
        }

        ShowPlayer(newValue);

    }

    private void SetCharaSkin(Material charaSkinMaterial)
    {
        meshs = GetComponentsInChildren<Renderer>();
        foreach (var m in meshs)
        {
            m.enabled = true;
            m.material = charaSkinMaterial;
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
        AllPlayerControl.instance.AddPlayer(OwnerClientId, this);
    }



    internal void Tele(Transform targetTransform)
    {
        TeleportRequest teleportRequest = new TeleportRequest();

        teleportRequest.destinationPosition = targetTransform.position;
        teleportRequest.destinationRotation = targetTransform.rotation;
        FindObjectOfType<TeleportationProvider>().QueueTeleportRequest(teleportRequest);
        ////VRCtr.instance.Teleport(targetTransform);
    }

    internal void Tele(Vector3 targetPosition)
    {
        TeleportRequest teleportRequest = new TeleportRequest();

        teleportRequest.destinationPosition = targetPosition;
        teleportRequest.destinationRotation = transform.rotation;
        FindObjectOfType<TeleportationProvider>().QueueTeleportRequest(teleportRequest);
        ////VRCtr.instance.Teleport(targetTransform);
    }

    internal void ChangeOwnerShip(NetworkObject networkObject)
    {
        NetworkObjectReference nf = new NetworkObjectReference(networkObject);
        RequestOwnershipServerRpc(nf);

    }

    
    internal void DeselectAll()
    {
        VRCtr.instance.ForceDeselect();
    }


   
    internal void Disapper()
    {
        gameObject.SetActive(false);
    }

    [ClientRpc]
    internal void DisapperClientRpc()
    {
        Disapper();
    }


}
