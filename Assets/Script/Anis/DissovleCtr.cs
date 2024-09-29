using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissovleCtr : MonoBehaviour
{

    public Material[] dissolveMaterials;
    public GameObject[] gameObjects;
    private void Awake()
    {
        foreach (var mat in dissolveMaterials)
        {
            mat.SetFloat("_Dissove", 0);
        }
        foreach (var g in gameObjects)
        {
            g.SetActive(false);
        }
    }

    public void Apper(Action endAction=null)
    {
        foreach (var g in gameObjects)
        {
            g.SetActive(true);
        }
        foreach (var mat in dissolveMaterials)
        {
            
            LeanTween.value(0, 1, 2).setOnUpdate((float val) => mat.SetFloat("_Dissove", val)).setOnComplete(endAction);
        }
    }

    public void Disapper()
    {
        foreach (var mat in dissolveMaterials)
        {
            LeanTween.value(1, 0, 2).setOnUpdate((float val) => mat.SetFloat("_Dissove", val));
        }
    }

}
