using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class VRCtr : MonoBehaviour
{
    public static VRCtr instance;
    public Transform org;
    private void Awake()
    {
        instance = this;
       
    }
    private void Start()
    {
        org = FindObjectOfType<XROrigin>().transform;
    }
    internal void Teleport(Transform target)
    {
        org.position = target.position;
    }
}
