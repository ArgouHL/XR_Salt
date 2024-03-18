using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchUIToVR : MonoBehaviour
{
    [SerializeField] private Transform vRUIOrigin;
    [SerializeField] private Canvas ui;


    void Start()
    {
        if (UnityEngine.XR.XRSettings.enabled)
            SwitchToVRUI();
    }
    private void SwitchToVRUI()
    {
        ui.renderMode = RenderMode.WorldSpace;
        ui.transform.parent = vRUIOrigin;
        ui.transform.localScale = Vector3.one * 0.01f;
        ui.transform.localPosition = Vector3.zero;
        ui.transform.localRotation = Quaternion.identity;
    }

}
