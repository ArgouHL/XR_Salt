using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    public Vector3 velocity;
    private Rigidbody rig;
    [SerializeField] public float maxVolume;
    [SerializeField] private float volume;
    private Vector3 frontVector;
    private void Awake()
    {
        rig = GetComponentInParent<Rigidbody>();
    }

    public void Update()
    {
        frontVector = -transform.up;
        Vector3 _frontVector = -transform.up;
        frontVector.y = 0;
        // frontVector = frontVector.normalized;
        velocity = rig.velocity;

        Debug.DrawRay(transform.position, frontVector, Color.blue);
        Debug.DrawRay(transform.position, _frontVector, Color.green);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("cell"))
        {
            if (!TryGetComponent<ColliderCell>(out ColliderCell cell))

                return;

            cell = other.GetComponent<ColliderCell>();
            if (!cell.CanPush())
                return;
            Debug.Log(Vector3.Angle(rig.velocity.normalized, frontVector));
            if (rig.velocity.magnitude <= 0 || Vector3.Angle(rig.velocity.normalized, frontVector) > 60)
                return;
            float v = volume + cell.Digged();
            volume = v < maxVolume ? v : maxVolume;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SaltMount"))
        {
            if (TryGetComponent<SaltMount>(out SaltMount saltMount))
            {
                if (!(volume > 0))
                    return;
                saltMount.AddVolume(volume);
                volume = 0;
            }
        }
    }


}
