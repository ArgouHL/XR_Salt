using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class netWorkObjectTest : MonoBehaviour
{





    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up , ForceMode.Impulse);
        }
    }
}
