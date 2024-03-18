using System;
using UnityEngine;

[Serializable]
public class NetworkRigData
{
    private Vector3 _position;
    public Vector3 Position { get { return _position; } }

    private Vector3 _velocity;
    public Vector3 Velocity { get { return _velocity; } }

    public NetworkRigData(Vector3 position, Vector3 velocity)
    {
        _position = position;       
        _velocity = velocity;
    }


}
