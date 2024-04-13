using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameFaceCamera : MonoBehaviour
{
    [SerializeField] private TMP_Text namePlant;
    private void Update()
    {

        transform.forward = Camera.main.transform.forward;
    }

    internal void ChangeName(string charaterName)
    {
        namePlant.text = charaterName;
    }
}
