using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayZone : MonoBehaviour
{
    private int index;
    [SerializeField] private Transform orgPos;
    [SerializeField] private GameObject realSnow;
    [SerializeField] private GameObject fakeSnow;
    private SaltMount saltMount => GetComponentInChildren<SaltMount>();

 

    internal void SetPlayerable()
    {
        fakeSnow.SetActive(false);
        realSnow.SetActive(true);

    }


    internal void SetSaltMountOwner(ulong id)
    {
        saltMount.SetOwner(id);
        
    }

    internal Transform GetTransform()
    {
        return orgPos;
    }




}
