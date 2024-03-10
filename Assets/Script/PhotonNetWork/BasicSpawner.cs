using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using System;
using Fusion.Sockets;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }



    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    private NetworkRunner networkRunner;
    public static BasicSpawner instance;
    [SerializeField] private NetworkPrefabRef networkPrefab;
    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private Vector2 pos = new Vector2(0, 0);

    private void Awake()
    {
        instance = this;
    }



    //Host
    async void StartGame(GameMode mode)
    {
        networkRunner = gameObject.AddComponent<NetworkRunner>();
        networkRunner.ProvideInput = true;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        PhotonUIManager.instance.HideUI(mode.ToString());
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

        var rtt = runner.GetPlayerRtt(PlayerRef.None);
        PhotonUIManager.instance.ShowPing(rtt);

    }

    internal void CreateServer()
    {
        StartGame(GameMode.Server);
    }


    internal void CreateHost()
    {
        StartGame(GameMode.Host);
    }

    internal void Join()
    {
        StartGame(GameMode.Client);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 playerPos = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkObject = runner.Spawn(networkPrefab, playerPos, Quaternion.identity, player);
            spawnedCharacters.Add(player, networkObject);
        }
      
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnedCharacters.Remove(player);
        }
    }


    private void Update()
    {
        if (networkRunner == null)
            return;
        var rtt = networkRunner.GetPlayerRtt(PlayerRef.None);
        PhotonUIManager.instance.ShowPing(rtt);
    }
}