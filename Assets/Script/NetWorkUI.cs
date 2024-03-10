using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetWorkUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;    
    [SerializeField] private Button hostBtn;    
    [SerializeField] private Button clientBtn;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private TMP_Text pingText;

    public static NetWorkUI instance;
    public delegate void UpdateEvent();
    public UpdateEvent OnUpdate;

    private void OnDisable()
    {
        OnUpdate = null;
    }


    private void Update()
    {
        OnUpdate?.Invoke();
    }

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        instance = this;

        serverBtn.onClick.AddListener(() =>
        {
            RelayLink.instance.CreateServerRelay();
        });

        hostBtn.onClick.AddListener(() =>
        {
            RelayLink.instance.CreateLanServer();
        });

        clientBtn.onClick.AddListener(() =>
        {
            RelayLink.instance.JoinRelay();
        });
    }

    public void HideUI(string state)
    {

        canvas.alpha = 0;
        canvas.interactable = false;
        stateText.text = state;
        OnUpdate += ShowPing;
    }





    private void ShowPing()
    {
        if (canvas.alpha <1)
        {
            ulong ping = NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(NetworkManager.Singleton.NetworkConfig.NetworkTransport.ServerClientId);
            pingText.text = ping.ToString();
        }
     
    
    }
}
