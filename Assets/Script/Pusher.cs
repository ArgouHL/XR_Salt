using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
   public Vector3 velocity;
    private Rigidbody rig;
    [SerializeField] public float maxVolume;
    private float volume;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        Vector3 frontVector = -transform.up;
        Vector3 _frontVector = -transform.up;
        frontVector.y = 0;
       // frontVector = frontVector.normalized;
        velocity = rig.velocity;
        
        Debug.DrawRay(transform.position, frontVector, Color.blue);
        Debug.DrawRay(transform.position, _frontVector, Color.green);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("cell"))
        {
            ColliderCell cell = other.GetComponent<ColliderCell>();
            float v=volume+cell.Digged();
            volume = v < maxVolume?v: maxVolume;
        }
        if (other.CompareTag("SaltMountion"))
        {
          
        }
    }


}
