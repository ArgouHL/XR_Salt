using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetWorkUIVR : NetWorkUI
{

   

    protected override void Awake()
    {


        canvas = GetComponent<CanvasGroup>();
        instance = this;
    

       

        clientBtn.onClick.AddListener(() =>
        {
            RelayLink.instance.JoinRelay();
        });

        lanclientBtn.onClick.AddListener(() =>
        {
            RelayLink.instance.JoinLan();
        });

        

    }

   
}
