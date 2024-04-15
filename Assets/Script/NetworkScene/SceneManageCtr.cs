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
            vrStartSceneName = vrStartScene.name;
        }

        if (craftRoom != null)
        {
            craftRoomName = craftRoom.name;
        }
    }
#endif

    public string m_Playscene;
    public string m_Scene2Name;
    public string vrStartSceneName;
    public string craftRoomName;

    public override void OnNetworkSpawn()
    {
        if (IsServer && !string.IsNullOrEmpty(vrStartSceneName))
        {
            var status = LoadStartScene(vrStartSceneName);

            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {vrStartSceneName} " +
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

        GUILayout.BeginArea(new Rect(DrawOffset, new Vector2(100, 50)));
        if (GUILayout.Button("SelectScnen"))
        {
            ChangeScene(vrStartSceneName);
        }
        if (GUILayout.Button("Playscene"))
        {
            ChangeScene(m_Playscene);
        }





        GUILayout.EndArea();

    }
}