using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using IngameDebugConsole;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Text;
using Unity.Services.Qos;
using System.Net;
using AddressFamily = System.Net.Sockets.AddressFamily;
using System;
using UnityEngine.UIElements;
using System.Linq;
using System.Threading.Tasks;

public class NetworkLink : MonoBehaviour
{
    public static NetworkLink instance;
    private Lobby curentLobby;
    private string r;
    private bool IsReagionTested = false;

    private string myAddressLocal;
    private string myAddressGlobal;
    public delegate void ServerEvent();
    public static ServerEvent OnServerStart;
    public static ServerEvent OnClientStart;
    Dictionary<IPAddress, DiscoveryResponseData> discoveredServers = new Dictionary<IPAddress, DiscoveryResponseData>();
    public ExampleNetworkDiscovery m_Discovery;


    public NetworkManager m_NetworkManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        DebugLogConsole.AddCommandInstance("LanServer", "LanServer", "CreateLanServer", this);
        DebugLogConsole.AddCommandInstance("ServerRelay", "ServerRelay", "CreateServerRelay", this);
        DebugLogConsole.AddCommandInstance("JoinIP", "JoinIP", "JoinIP", this);
        DebugLogConsole.AddCommandInstance("JoinRelay", "JoinRelay", "JoinRelay", this);
        m_NetworkManager = FindObjectOfType<NetworkManager>();
        m_Discovery = FindObjectOfType<ExampleNetworkDiscovery>();

    }

    private async void Start()
    {
        var options = new InitializationOptions();
        await UnityServices.InitializeAsync(options);

        AuthenticationService.Instance.SignedIn += () =>
       {
           Debug.Log("Signed In :" + AuthenticationService.Instance.PlayerId);
       };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //GetIP();
      
    }

 

    private void GetIP()
    {

        //string strHostName = "";
        //strHostName = Dns.GetHostName();

        //var ipEntry = Dns.GetHostEntry(strHostName);

        //var addr = ipEntry.AddressList;

        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                myAddressLocal = ip.ToString();
                break;
            } //if
        } //foreach
        //Get the global IP
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.ipify.org");
        request.Method = "GET";
        request.Timeout = 1000; //time in ms
        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                myAddressGlobal = reader.ReadToEnd();
            } //if
            else
            {
                Debug.LogError("Timed out? " + response.StatusDescription);
                myAddressGlobal = "127.0.0.1";
            } //else
        } //try
        catch (WebException ex)
        {
            Debug.Log("Likely no internet connection: " + ex.Message);
            myAddressGlobal = "127.0.0.1";
        } //catch
        Debug.Log(myAddressGlobal);
        Debug.Log(myAddressLocal);
        //return addr[addr.Length - 1].ToString();


    }

    public void CreateLanServer()
    {
        //if (!IsReagionTested)
        //    return;
        try
        {

            //Allocation alloc = await RelayService.Instance.CreateAllocationAsync(5, r);
            //string newJoinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
            //Debug.Log(newJoinCode);

            //RelayServerData relayServerData = new RelayServerData(alloc, "dtls");
            // NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            //CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            //lobbyOptions.IsPrivate = false;
            //lobbyOptions.Data = new Dictionary<string, DataObject>();
            //DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, newJoinCode);
            //lobbyOptions.Data.Add("JoinCode", dataObject);




            //GetIP();
            //Debug.Log(myAddressLocal);
            // var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            //transport.ConnectionData.Address = "0.0.0.0";
            //CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            //lobbyOptions.IsPrivate = false;
            //lobbyOptions.Data = new Dictionary<string, DataObject>();
            //DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, myAddressLocal);
            //lobbyOptions.IsLocked = false;
            //lobbyOptions.Data.Add("IP", dataObject);
            //        lobbyOptions.Data.Add("Port", dataObject2);
            // curentLobby = await Lobbies.Instance.CreateLobbyAsync("XR_Lobby", 4, lobbyOptions);
            //  Debug.Log(curentLobby.Id);

            NetworkManager.Singleton.StartServer();
            NetWorkUI.instance.HideUI("Server");
            OnServerStart?.Invoke();


        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    public void CreateServerRelay()
    {
        StartCoroutine(SetRelayServer());

    }
    public async void CreateHost()
    {
        try
        {

            Allocation alloc = await RelayService.Instance.CreateAllocationAsync(5, r);
            string newJoinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
            Debug.Log(newJoinCode);

            RelayServerData relayServerData = new RelayServerData(alloc, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>();
            DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, newJoinCode);
            lobbyOptions.Data.Add("JoinCode", dataObject);



            //GetIP();
            //Debug.Log(myAddressLocal);

            //CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            //lobbyOptions.IsPrivate = false;
            //lobbyOptions.Data = new Dictionary<string, DataObject>();
            //DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, myAddressLocal);

            //lobbyOptions.Data.Add("IP", dataObject);

            curentLobby = await Lobbies.Instance.CreateLobbyAsync("L", 5, lobbyOptions);


            NetworkManager.Singleton.StartHost();
            NetWorkUI.instance.HideUI("Host");

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);

        }
    }

    public void JoinIP(string ip)
    {
        //curentLobby = await Lobbies.Instance.QuickJoinLobbyAsync();
        //string ip = curentLobby.Data["IP"].Value;
        Debug.Log("Join" + ip);
        try
        {
            //  JoinAllocation alloc = await RelayService.Instance.JoinAllocationAsync(joinCode);
            //   RelayServerData relayServerData = new RelayServerData(alloc, "dtls");
            //      NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = ip;
            if (NetworkManager.Singleton.StartClient())
                NetWorkUI.instance.HideUI("Client");

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }

    public async void GetServerIP()
    {
        var curentLobby = await Lobbies.Instance.QueryLobbiesAsync();
        foreach (var lobby in curentLobby.Results)
        {
            Debug.Log("JoinCode :" + lobby.Data["JoinCode"].Value);
        }

    }
    public async void JoinRelay()
    {
        var curentLobby = await Lobbies.Instance.QueryLobbiesAsync();
        string joinCode = curentLobby.Results[0].Data["JoinCode"].Value;
        Debug.Log("Join" + joinCode);
        try
        {
            JoinAllocation alloc = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(alloc, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            NetworkManager.Singleton.StartClient();
            NetWorkUI.instance.HideUI("Client");

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }

    }


    private IEnumerator SetRelayServer()
    {
        // Request list of valid regions
        var regionsTask = Relay.Instance.ListRegionsAsync();

        while (!regionsTask.IsCompleted)
        {
            yield return null;
        }

        if (regionsTask.IsFaulted)
        {
            Debug.LogError("List regions request failed");
            yield break;
        }

        var regionList = regionsTask.Result;

        IList<string> list = new List<string>();

        foreach (var region in regionList)
        {
            list.Add(region.Id);
        }

        var qosResultsForRegion = QosService.Instance.GetSortedQosResultsAsync("relay", list);
        while (!qosResultsForRegion.IsCompleted)
        {
            Debug.Log("Regions testing");
            yield return new WaitForSeconds(0.5f);
        }
        if (qosResultsForRegion.IsFaulted)
        {
            Debug.LogError("Regions test failed");
            yield break;
        }
        int i = 999999;
        foreach (var region in qosResultsForRegion.Result)
        {
            if (region.AverageLatencyMs < i)
            {

                i = region.AverageLatencyMs;
                r = region.Region;

            }
            //Debug.Log(region.Region +";" + region.AverageLatencyMs);

        }
        Debug.Log("SelectRegion : " + r + ";" + i);
        RelayServer();
        OnServerStart?.Invoke();

    }
    private async void RelayServer()
    {
        try
        {

            Allocation alloc = await RelayService.Instance.CreateAllocationAsync(5, r);
            string newJoinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);
            Debug.Log(newJoinCode);

            RelayServerData relayServerData = new RelayServerData(alloc, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>();
            DataObject dataObject = new DataObject(DataObject.VisibilityOptions.Public, newJoinCode);
            lobbyOptions.Data.Add("JoinCode", dataObject);


            curentLobby = await Lobbies.Instance.CreateLobbyAsync("XR_Lobby", 4, lobbyOptions);


            NetworkManager.Singleton.StartServer();
            NetWorkUI.instance.HideUI("Server");



        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    internal void JoinLan()
    {
        m_Discovery.OnServerFound.AddListener(OnServerFound);
        m_Discovery.OnServerNotFound.AddListener(OnServerNotFound);
        ConnetToLanServer();

    }

    private async void ConnetToLanServer()
    {
       // NetWorkUI.instance.HideUI("Connecting", false);
        m_Discovery.StartClient();
        m_Discovery.TaskClientBroadcast(new DiscoveryBroadcastData());
        await Task.Delay(200);
        while (discoveredServers.Count == 0)
        {
            Debug.Log(discoveredServers.Count);
            OnServerNotFound();
            await Task.Delay(500);


        }


    }


    public void OnServerFound(IPEndPoint sender, DiscoveryResponseData response)
    {

        discoveredServers[sender.Address] = response;
        Debug.Log(discoveredServers.Count);

        var ips = discoveredServers.ToArray();

        UnityTransport transport = (UnityTransport)m_NetworkManager.NetworkConfig.NetworkTransport;
        transport.SetConnectionData(ips[0].Key.ToString(), ips[0].Value.Port);
        NetworkManager.Singleton.StartClient();
      //  NetWorkUI.instance.HideUI("Client");
        Debug.Log("Join" + ips[0].Key.ToString() + ";" + ips[0].Value.Port);

    }

    public void OnServerNotFound()
    {
        Debug.Log("Connecting");
        discoveredServers.Clear();
        m_Discovery.TaskClientBroadcast(new DiscoveryBroadcastData());
    }


   
}
