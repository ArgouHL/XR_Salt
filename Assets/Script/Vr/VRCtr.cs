using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRCtr : MonoBehaviour
{
    public static VRCtr instance;
    public Transform org;
    private XRInteractionManager xRInteractionManager;
    public Material handMaterial;
    public XRBaseInteractor[] interactors;

    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        org = FindObjectOfType<XROrigin>().transform;
        xRInteractionManager = GetComponentInChildren<XRInteractionManager>();
        handMaterial.color = Color.white;

        foreach (var interactor in interactors)
        {
            IXRSelectInteractor selectinteractor = interactor;
            Debug.Log(selectinteractor.transform.name);
        }
        //interactors = GetComponentsInChildren<IXRSelectInteractor>();
        //foreach (var ixr in interactors)
        //{
        //    Debug.Log(ixr.transform.gameObject.name);

        //}

    }
    internal void Teleport(Transform target)
    {
        org.position = target.position;
    }

    internal void Teleport(Vector3 position)
    {
        org.position = position;

    }

    internal void ForceDeselect()
    {
        foreach (var interactor in interactors)
        {
            IXRSelectInteractor selectinteractor = interactor;
            xRInteractionManager.CancelInteractorSelection(selectinteractor);
        }

        //GetComponentsInChildren<IXRSelectInteractor>();
        //xRInteractionManager.CancelInteractorSelection(GetComponentsInChildren<XRBaseInteractor>());
    }

    internal void ChangeColor(int charaterIndex)
    {
        Color color = CharaDatas.GetCharaData(charaterIndex).charaSkinColor;
        handMaterial.color = color;

    }
}
