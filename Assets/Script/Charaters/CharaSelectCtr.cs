using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaSelectCtr : MonoBehaviour
{
    public static CharaSelectCtr instance;
    public  CharatarSelectEventer[] eventers;
    private void Awake()
    {
        instance = this;
        eventers = GetComponentsInChildren<CharatarSelectEventer>();
    }

    internal void AllDisapper()
    {
        foreach(var eventer in eventers)
        {
            eventer.Unshow();
        }
    }
}
