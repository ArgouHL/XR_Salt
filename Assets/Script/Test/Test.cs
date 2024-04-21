using IngameDebugConsole;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Test : MonoBehaviour
{

    private void Start()
    {
        DebugLogConsole.AddCommandInstance("TestGet", "TestGet", "TestGet", this);
    }

    [ContextMenu("TestGet")]
    public void TestGet()
    {
        NetworkPlayer.ownPlayer.ChangeOwnerShip(GetComponent<NetworkObject>());
        //GetComponent<NetworkObject>().ChangeOwnership((ulong) 1);
    }    
}
