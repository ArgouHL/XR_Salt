using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayZone : MonoBehaviour
{
    private int index;
    [SerializeField] private Transform orgPos;
    [SerializeField] private GameObject realSnow;
    [SerializeField] private GameObject fakeSnow;
    [SerializeField] private SaltMount saltmount;


    internal void SetPlayerable()
    {
        fakeSnow.SetActive(false);
        realSnow.SetActive(true);
       
    }  
    

    internal Transform GetTransform()
    {
        return orgPos;
    }




}
