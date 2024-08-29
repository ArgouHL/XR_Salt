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
    public GameObject[] controllerModels;
    public XRBaseInteractor[] interactors;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        org = FindObjectOfType<XROrigin>().transform;
        xRInteractionManager = GetComponentInChildren<XRInteractionManager>();
        

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

    internal void ShowOwnPlayer(int charaterIndex)
    {
        foreach(var c in controllerModels)
        {
            c.SetActive(false);
        }
        skinnedMeshRenderer.material = CharaDatas.GetCharaData(charaterIndex).charaSkinMaterial;
        

    }
}
