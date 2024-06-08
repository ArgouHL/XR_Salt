using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VRStart : MonoBehaviour
{
    public GameObject startUI;
    public void StartJoin()
    {
        NetworkLink.instance.JoinLan();
        startUI.SetActive(false);
        //SceneManageCtr.instance.VrStart();
    }

    
}
