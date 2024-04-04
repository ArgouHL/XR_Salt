using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManageCtr : NetworkBehaviour
{
    public static SceneManageCtr intance;

    private void Awake()
    {
        if (intance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        intance = this;

    }



#if UNITY_EDITOR
    public UnityEditor.SceneAsset SceneAsset1;
    public UnityEditor.SceneAsset SceneAsset2;
    public UnityEditor.SceneAsset vrStartScene;
    private void OnValidate()
    {
        if (SceneAsset1 != null)
        {
            m_Scene1Name = SceneAsset1.name;
        }

        if (SceneAsset2 != null)
        {
            m_Scene2Name = SceneAsset2.name;
        }

        if (vrStartScene != null)
        {
            vrStartSceneName = vrStartScene.name;
        }
    }
#endif

    private string m_Scene1Name;
    private string m_Scene2Name;
    private string vrStartSceneName;

    public override void OnNetworkSpawn()
    {
        if (IsServer && !string.IsNullOrEmpty(m_Scene1Name))
        {
            var status = NetworkManager.SceneManager.LoadScene(m_Scene1Name, LoadSceneMode.Additive);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {m_Scene1Name} " +
                      $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
        else if (!string.IsNullOrEmpty(vrStartSceneName))
        {
            var status = NetworkManager.SceneManager.LoadScene(vrStartSceneName, LoadSceneMode.Additive);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {vrStartSceneName} " +
                      $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }
    }

    [ContextMenu("ChangeScene1")]
    public void ChangeScene1()
    {
        StartCoroutine(ChangeScene1IE());

    }


    private IEnumerator ChangeScene1IE()
    {
        DisloadScene(m_Scene2Name);
        yield return new WaitForSeconds(0.2f);
        ChangeScene(m_Scene1Name);
    }

    [ContextMenu("ChangeScene2")]
    public void ChangeScene2()
    {

        StartCoroutine(ChangeScene2IE());

    }
    private IEnumerator ChangeScene2IE()
    {
        DisloadScene(m_Scene1Name);
        yield return new WaitForSeconds(0.2f);
        ChangeScene(m_Scene2Name);
    }


    private void DisloadScene(string scnenName)
    {
        if (!IsServer)
            return;
        NetworkManager.SceneManager.UnloadScene(SceneManager.GetSceneByName(scnenName));
    }

    private void ChangeScene(string scnenName)
    {
        if (!IsServer)
            return;

        NetworkManager.SceneManager.LoadScene(scnenName, LoadSceneMode.Additive);

        //  ChangeSceneClientRpc(scnenName);
    }

    [ClientRpc]
    private void ChangeSceneClientRpc(string scnenName)
    {
        NetworkManager.SceneManager.LoadScene(scnenName, LoadSceneMode.Single);
    }


}