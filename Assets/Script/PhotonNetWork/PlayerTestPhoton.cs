using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerTestPhoton : NetworkBehaviour
{
    private NetworkTransform networkTransform;
 //   private InputActionMap player;
 ////   private Vector2 currentMovement => player.FindAction("Move").ReadValue<Vector2>();
 //   private bool IsPressed => currentMovement.magnitude > 0f;
 //   [SerializeField] private InputActionAsset inputAsset;



    //NetWorkAwake
    public void Start()
    {

        //base.OnNetworkSpawn();       
        //player = inputAsset.FindActionMap("Player");
        //player.Enable();

        //num.OnValueChanged += (int previousValue, int newValue) => { Debug.Log(OwnerClientId + ";" + newValue); };

    }

    public void Update()
    {
    //    if(networkTransform.ism)
    }

    public override void FixedUpdateNetwork()
    {      

        //if(IsClient)
        //{
        //    transform.position += new Vector3(currentMovement.x, 0, currentMovement.y) * Time.deltaTime * 5;
        //}
        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.position += Vector3.forward * Time.deltaTime * 5;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.position -= Vector3.forward * Time.deltaTime * 5;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.position -= Vector3.right * Time.deltaTime * 5;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.position += Vector3.right * Time.deltaTime * 5;
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
        //    TestServerRpc(new ServerRpcParams());
            //  num.Value = Random.Range(1, 10);
        }

 
    }



}
