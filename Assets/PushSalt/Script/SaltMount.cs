using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class SaltMount : NetworkBehaviour
{

    private NetworkVariable<float> volume = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private float startVolume = 50;
    [SerializeField] private Transform mount;
    [SerializeField] private TMP_Text volumeShow;
    [SerializeField] private Material mountMat;

    private float testv = 0;
    public override void OnNetworkSpawn()
    {

        volume.OnValueChanged += UpdateMount;
        UpdateMount(0, 0);
        GetComponentInChildren<MeshRenderer>().material = new Material(mountMat);

    }




    //private void Start()
    //{
    //    volume.Value = 6;
    //    UpdateMount();

    //}

    internal void AddVolume(float v)
    {
        Debug.Log(v);
        volume.Value += v;

        //AddVolumeServerRpc(v);


    }

    private void UpdateMount(float preV, float nowV)
    {
        float scale = Mathf.Pow(((nowV + startVolume) * 1000) * 12 / Mathf.PI * 1.23f, 1f / 3f) / 100;
        mount.localScale = Vector3.one * scale;
        volumeShow.text = nowV + "kg";
        GetComponentInChildren<MeshRenderer>().material.SetFloat("_NoiseScale", scale*1000);
        Debug.Log(scale);
    }

    [ContextMenu("AddVTest")]
    public void AddVTest()
    {
        testv += 10;
        UpdateMount(0, testv);
    }

    [ContextMenu("AddV")]
    public void AddV()
    {
        AddVolumeServerRpc(10);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddVolumeServerRpc(float v)
    {
        volume.Value += v;
    }

    internal void SetOwner(ulong id)
    {
        GetComponent<NetworkObject>().ChangeOwnership(id);
    }
}
