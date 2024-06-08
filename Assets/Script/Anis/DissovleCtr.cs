using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissovleCtr : MonoBehaviour
{
    public Material[] dissolveMaterials;
    private void Awake()
    {
        foreach (var mat in dissolveMaterials)
        {
            mat.SetFloat("_Dissove", 0);
        }
    }

    public void Apper()
    {
        foreach(var mat in dissolveMaterials)
        {
            LeanTween.value(0, 1, 2).setOnUpdate((float val) => mat.SetFloat("_Dissove", val));
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
