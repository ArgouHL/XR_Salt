using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraControl : NetworkBehaviour
{
    public static CameraControl instance;
    private Transform camera;
    private Vector3 cameraOrgPosition;
    private Dictionary<int,Transform> playersViewDict = new Dictionary<int, Transform>();
    public Vector2 DrawOffset = new Vector2(250, 250);
    private int targetPlayer;

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
        camera = Camera.main.transform;
        
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
            SetCamera(camera.parent);
        }

        foreach( var pair in playersViewDict)
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
        if (playersViewDict[targetPlayer] == null)
        {
            playersViewDict.Remove(targetPlayer);
            OnUpdate = null;
            SetCamera(camera.parent);
            return;
        }
        SetCamera(playersViewDict[targetPlayer]);
    }

    private void SetCamera(Transform t)
    {
        camera.position = t.position;
        camera.rotation = t.rotation;
    }



    private void Update()
    {
        OnUpdate?.Invoke();
    }

    internal void AddPlayer(int id,Transform playerTrasform)
    {
        if (playersViewDict.ContainsKey(id))
        {
            Debug.LogError("Player " + id + "camera is existed");
            return;
        }
        playersViewDict.Add(id, playerTrasform);

    }

    private void RemovePlayerCam(ulong playerID)
    {
        if (!playersViewDict.ContainsKey((int)playerID))
        {
            Debug.LogError("Player " + (int)playerID + "camera is not existe");
            return;
        }
        playersViewDict.Remove((int)playerID);
        
    }

}

