using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;
    private Transform camera;
    private Vector3 cameraOrgPosition;
    private List<Transform> playersView = new List<Transform>();
    public Vector2 DrawOffset = new Vector2(50, 50);
    private int targetPlayer;

    private delegate void CameraUpdateAction();
    private CameraUpdateAction OnUpdate;

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

        GUILayout.BeginArea(new Rect(DrawOffset, new Vector2(200, 200)));
        if (GUILayout.Button("OrgPoint"))
        {
            OnUpdate = null;
            SetCamera(camera.parent);
        }
        for (int i = 0; i < playersView.Count; i++)
        {
            if (GUILayout.Button("Player" + i))
            {
                targetPlayer = i;
                OnUpdate = UpdateCamera;
            }
        }


        GUILayout.EndArea();

    }

    private void UpdateCamera()
    {
        SetCamera(playersView[targetPlayer]);
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

    internal void AddPlayer(Transform playerTrasform)
    {
        playersView.Add(playerTrasform);
    }

    internal void RemovePlayer(Transform playerTrasform)
    {
        playersView.Remove(playerTrasform);
    }
}

