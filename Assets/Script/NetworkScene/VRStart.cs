using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VRStart : MonoBehaviour
{
    public GameObject startUI;
    private int count = 0;
    public void StartJoin()
    {
        count++;
        if (count < 2)
            return;
        NetworkLink.instance.JoinLan();
       startUI.SetActive(false);
        //SceneManageCtr.instance.VrStart();
    }

    
}
