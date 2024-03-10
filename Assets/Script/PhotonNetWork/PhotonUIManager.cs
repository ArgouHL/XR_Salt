using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonUIManager : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private TMP_Text pingText;

    public static PhotonUIManager instance;
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
        canvas.alpha = 1;
        canvas.interactable = true;
        canvas = GetComponent<CanvasGroup>();
        instance = this;

        serverBtn.onClick.AddListener(() =>
        {
            BasicSpawner.instance.CreateServer();

        });

        hostBtn.onClick.AddListener(() =>
        {
            BasicSpawner.instance.CreateHost();
        });

        clientBtn.onClick.AddListener(() =>
        {
            BasicSpawner.instance.Join();
        });
    }

    public void HideUI(string state)
    {
        canvas.alpha = 0;
        canvas.interactable = false;
        stateText.text = state;
       // OnUpdate += ShowPing;
    }





    public void ShowPing(double rtt)
    {
  
          //  ulong ping = NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(NetworkManager.Singleton.NetworkConfig.NetworkTransport.ServerClientId);
          Debug.Log("rtt :"+rtt);
        


    }
}
