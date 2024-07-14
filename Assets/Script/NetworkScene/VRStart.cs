using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VRStart : MonoBehaviour
{
    public GameObject startUI;
    public static VRStart instance;
    private void Awake()
    {
        instance = this;
    }

    private int count = 0;

    public void StartChi()
    {
        LangugeSelecter.languge = Lang.Chi;
        StartJoin();
    }

    public void StartTW()
    {
        LangugeSelecter.languge = Lang.TW;
        StartJoin();
    }
    public void StartJoin()
    {
        count++;
        if (count < 2)
            return;
        NetworkLink.instance.JoinLan();
      
        //SceneManageCtr.instance.VrStart();
    }

    public void HideBlack()
    {
        startUI.SetActive(false);
    }
    
}
