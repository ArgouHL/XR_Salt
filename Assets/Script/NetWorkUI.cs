using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetWorkUI : MonoBehaviour
{
    [SerializeField] protected Button lanServerBtn;
    [SerializeField] protected Button relayServerBtn;
    [SerializeField] protected Button relayHostBtn;    
    [SerializeField] protected Button clientBtn;
    [SerializeField] protected Button lanclientBtn;
    [SerializeField] protected CanvasGroup canvas;
    [SerializeField] protected TMP_Text stateText;
    [SerializeField] protected TMP_Text pingText;

    public static NetWorkUI instance;
    public delegate void UpdateEvent();
    public UpdateEvent OnUpdate;
    public ulong ping;
    private void OnDisable()
    {
        OnUpdate = null;
    }


    private void Update()
    {
        OnUpdate?.Invoke();
    }

    protected virtual void Awake()
    {


        canvas = GetComponent<CanvasGroup>();
        instance = this;

        relayServerBtn.onClick.AddListener(() =>
        {
            NetworkLink.instance.CreateServerRelay();
        });

        lanServerBtn.onClick.AddListener(() =>
        {
            NetworkLink.instance.CreateLanServer();
        });

        relayHostBtn.onClick.AddListener(() =>
        {
            NetworkLink.instance.CreateHost();
        });


        clientBtn.onClick.AddListener(() =>
        {
            NetworkLink.instance.JoinRelay();
        });

        lanclientBtn.onClick.AddListener(() =>
        {
            NetworkLink.instance.JoinLan();
        });



    }

    public void HideUI(string state,bool showFPS=true)
    {

        canvas.alpha = 0;
        canvas.interactable = false;
        stateText.text = state;
        if(showFPS)
        OnUpdate += ShowPing;
    }





    private void ShowPing()
    {
        if (canvas.alpha <1)
        {
             ping = NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(NetworkManager.Singleton.NetworkConfig.NetworkTransport.ServerClientId);
            pingText.text = ping.ToString();
        }
     
    
    }
}
