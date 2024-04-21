using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class SaltMount : NetworkBehaviour
{
    private NetworkVariable<float> volume = new NetworkVariable<float>(0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private Transform mount;
    public float startValue = 60;

    public override void OnNetworkSpawn()
    {
        volume.OnValueChanged += UpdateMount;
        volume.Value = startValue;
        UpdateMount(0, volume.Value);
    }


    //private void Start()
    //{
    //    volume.Value = 6;
    //    UpdateMount();

    //}

    internal void AddVolume(float v)
    {
        float nowVolum = volume.Value;
        nowVolum += v;
        UpdateMount(0,nowVolum);
        volume.Value += v;
       
    }

    private void UpdateMount(float preV, float nowV)
    {
        mount.localScale = Vector3.one * nowV * 0.01f;
    }

    [ContextMenu("AddV")]
    public void AddV()
    {
        AddVolume(5);
    }

}
