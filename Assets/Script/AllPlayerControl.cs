using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class AllPlayerControl : NetworkBehaviour
{
    public static AllPlayerControl instance;
    private Transform nowCamera
        ;
    private Vector3 cameraOrgPosition;
    private Dictionary<ulong, NetworkPlayer> playersDict = new Dictionary<ulong, NetworkPlayer>();
    public Vector2 DrawOffset = new Vector2(250, 250);
    private ulong targetPlayer;

    private delegate void CameraUpdateAction();
    private CameraUpdateAction OnUpdate;

    
    public override void OnNetworkSpawn()
    {
        NetworkManager.OnClientDisconnectCallback += RemovePlayerCam;
    }

   
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        nowCamera = Camera.main.transform;
        
    }

    void OnGUI()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return;
#endif

        GUILayout.BeginArea(new Rect(DrawOffset, new Vector2(200, 100)));
        if (GUILayout.Button("OrgPoint"))
        {
            OnUpdate = null;
            SetCamera(nowCamera.parent);
        }

        foreach( var pair in playersDict)
        {
            if (GUILayout.Button("Player" + pair.Key))
            {
                targetPlayer = pair.Key;
                OnUpdate = UpdateCamera;
            }
        }

        //for (int i = 0; i < playersView.Count; i++)
        //{
        //    int id= playersViewDict.GetKe(playersView[i]
        //    if (GUILayout.Button("Player" + i))
        //    {
        //        targetPlayer = i;
        //        OnUpdate = UpdateCamera;
        //    }
        //}


        GUILayout.EndArea();

    }

    private void UpdateCamera()
    {
        if (playersDict[targetPlayer] == null)
        {
            playersDict.Remove(targetPlayer);
            OnUpdate = null;
            SetCamera(nowCamera.parent);
            return;
        }
        SetCamera(playersDict[targetPlayer].head.transform);
        Debug.Log("UpdateCamera");
    }

    private void SetCamera(Transform t)
    {
        nowCamera.position = t.position;
        nowCamera.rotation = t.rotation;
    }



    private void Update()
    {
        OnUpdate?.Invoke();
    }

    internal void AddPlayer(ulong id,NetworkPlayer networkPlayer)
    {
        if (playersDict.ContainsKey(id))
        {
            Debug.LogError("Player " + id + "camera is existed");
            return;

        }
        playersDict.Add(id, networkPlayer);
        Debug.Log("player " + id + " Added");
    }

    private void RemovePlayerCam(ulong playerID)
    {
        if (!playersDict.ContainsKey(playerID))
        {
            Debug.LogError("Player " + (int)playerID + "camera is not existe");
            return;
        }
        playersDict.Remove(playerID);
        
    }

    internal NetworkPlayer GetPlayer(ulong clientId)
    {
        return playersDict[clientId];
    }

    internal void HideAllPlayer()
    {
        foreach(var np in playersDict)
        {
            np.Value.DisapperClientRpc();
        }
    }
}

