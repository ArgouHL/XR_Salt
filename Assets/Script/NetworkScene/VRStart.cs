using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VRStart : MonoBehaviour
{
   
    void Start()
    {
        NetworkLink.instance.JoinLan();
        //SceneManageCtr.instance.VrStart();
    }

    
}
