using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManageCtr : NetworkBehaviour
{
    public static SceneManageCtr instance;
    public Vector2 DrawOffset = new Vector2(300, 10);
    private string nowSceneName;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

    }

    private void Start()
    {

    }


#if UNITY_EDITOR
    public UnityEditor.SceneAsset Playscene;
    public UnityEditor.SceneAsset SceneAsset2;
    public UnityEditor.SceneAsset vrStartScene;
    public UnityEditor.SceneAsset craftRoom;
    public UnityEditor.SceneAsset craftRoomEnd;
    public UnityEditor.SceneAsset endScene;
    private void OnValidate()
    {
        if (Playscene != null)
        {
            m_Playscene = Playscene.name;
        }

        if (SceneAsset2 != null)
        {
            m_Scene2Name = SceneAsset2.name;
        }

        if (vrStartScene != null)
        {
            m_vrStartScene = vrStartScene.name;
        }

        if (craftRoom != null)
        {
            m_craftRoom = craftRoom.name;
        }

        if (craftRoomEnd != null)
        {
            m_craftRoomEnd = craftRoomEnd.name;
        }

        if (craftRoom != null)
        {
            m_endScene = endScene.name;
        }
    }
#endif

    public string m_Playscene;
    public string m_Scene2Name;
    public string m_vrStartScene;
    public string m_craftRoom;
    public string m_craftRoomEnd;
    public string m_endScene;

    public override void OnNetworkSpawn()
    {
        if (IsServer && !string.IsNullOrEmpty(m_vrStartScene))
        {
            var status = LoadStartScene(m_vrStartScene);

            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {m_vrStartScene} " +
                      $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }

            Debug.Log("StartS");
        }
        else if (IsServer)
            Debug.Log("StartNot");
    }

    //internal void VrStart()
    //{
    //    if (!string.IsNullOrEmpty(vrStartSceneName))
    //    {
    //        SceneManager.LoadSceneAsync(vrStartSceneName, LoadSceneMode.Additive);

    //    }
    //    if (!string.IsNullOrEmpty(craftRoomName))
    //    {
    //        SceneManager.LoadSceneAsync(craftRoomName, LoadSceneMode.Additive);

    //    }
    //}



    //internal void JoinSlatLobby()
    //{
    //    StartCoroutine(JoinLobbyIE());
    //}



    //private IEnumerator JoinLobbyIE()
    //{
    //    yield return SceneManager.UnloadSceneAsync(vrStartSceneName);  
    //    yield return SceneManager.LoadSceneAsync(m_Scene1Name, LoadSceneMode.Additive);        
    //    NetworkLink.instance.JoinLan();
    //}

    private void ChangeScene(string scnenName)
    {
        if (!IsServer)
            return;
        if (nowSceneName == null)
            return;
        ForceDeseletAllClientRpc();
        NetworkManager.SceneManager.UnloadScene(SceneManager.GetSceneByName(nowSceneName));
        nowSceneName = scnenName;
        NetworkManager.SceneManager.OnUnloadEventCompleted += LoadScene;

    }

    private void LoadScene(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {

        NetworkManager.SceneManager.LoadScene(nowSceneName, LoadSceneMode.Additive);

    }

    private SceneEventProgressStatus LoadStartScene(string scnenName)
    {
        if (!IsServer)
        {
            return SceneEventProgressStatus.None;
        }

        nowSceneName = scnenName;
        return NetworkManager.SceneManager.LoadScene(scnenName, LoadSceneMode.Additive);

        //  ChangeSceneClientRpc(scnenName);
    }




    void OnGUI()
    {
        if (!IsServer)
            return;
#if UNITY_ANDROID && !UNITY_EDITOR
        return;
#endif

        GUILayout.BeginArea(new Rect(DrawOffset, new Vector2(100, 200)));
        if (GUILayout.Button("SelectScnen"))
        {
            ChangeScene(m_vrStartScene);
        }
        if (GUILayout.Button("Playscene"))
        {
            ChangeScene(m_Playscene);
        }
        if (GUILayout.Button("EndScene"))
        {
            ChangeScene(m_endScene);
        }




        GUILayout.EndArea();

    }

   
    [ClientRpc]
    [ContextMenu("deselect")]
    private void ForceDeseletAllClientRpc()
    {
        NetworkPlayer.ownPlayer.DeselectAll();
    }

    public void LoadPlayScene()
    {
        ChangeScene(m_Playscene);
    }

    public void LoadEndScene()
    {
        ChangeScene(m_endScene);
    }
}