using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameFaceCamera : FaceCarema
{
    [SerializeField] private TMP_Text namePlant;
   
    internal void ChangeName(string charaterName)
    {
        namePlant.text = charaterName;
    }
}
