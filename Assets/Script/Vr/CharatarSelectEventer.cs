using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharatarSelectEventer : NetworkBehaviour
{
    public CharaterObj charaSet;
    public TMP_Text nameText;
    public GameObject CharaObj;
    private NetworkVariable<bool> chooseable = new NetworkVariable<bool>();

    

    private void Start()
    {
        
       
        if (IsServer)
        {
            chooseable.Value = true;
        }
        if (!chooseable.Value)
        {
            Unshow();
            return;
        }

        nameText.text = charaSet.charaterData.charaterName;
        var meshs = GetComponentsInChildren<Renderer>();
        foreach (var m in meshs)
        {
         
            m.material.color = charaSet.charaterData.charaSkinColor;
        }

        DebugLogConsole.AddCommandInstance("CharaChoosed"+ charaSet.charaterData.charaterName, "CharaChoosed" + charaSet.charaterData.charaterName, "CharaChoosed", this);
    }

    private void OnEnable()
    {
        GetComponent<XRSimpleInteractable>().hoverEntered.AddListener((HoverEnterEventArgs ergs) => OnHoverEnter(ergs));
        GetComponent<XRSimpleInteractable>().hoverExited.AddListener((HoverExitEventArgs ergs) => OnHoverExit(ergs));
        GetComponent<XRSimpleInteractable>().activated.AddListener((ActivateEventArgs ergs) => OnTriggered(ergs));


    }



    private void OnDisable()
    {
        GetComponent<XRSimpleInteractable>().hoverEntered.RemoveListener((HoverEnterEventArgs ergs) => OnHoverEnter(ergs));
        GetComponent<XRSimpleInteractable>().hoverExited.RemoveListener((HoverExitEventArgs ergs) => OnHoverExit(ergs));
        GetComponent<XRSimpleInteractable>().activated.RemoveListener((ActivateEventArgs ergs) => OnTriggered(ergs));
    }

    private void OnHoverExit(HoverExitEventArgs ergs)
    {
        GetComponent<Outline>().DisOutline();

    }

    private void OnHoverEnter(HoverEnterEventArgs ergs)
    {
        GetComponent<Outline>().EnableOutline();
    }


    public void OnTriggered(ActivateEventArgs ergs)
    {
        //PlayerDataContainer.SetData(charaSet.charaterData.charaterIndex);
        //SceneManageCtr.instance.JoinSlatLobby();
        CharaChoosed();
    }

    private void Unshow()
    {
        CharaObj.SetActive(false);
    }

    [ContextMenu("OnTriggered")]
    public void OnTriggered()
    {
        CharaChoosed();
    }

    public void CharaChoosed()
    {
        CharaChoosedServerRpc(OwnerClientId);
    }

    [ServerRpc]
    private void CharaChoosedServerRpc(ulong clientId)
    {
        if (!chooseable.Value)
            return;
        if (!IsServer) return;
        Unshow();
        chooseable.Value = false;
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        SetCharaClientRpc(clientRpcParams);
    }

    [ClientRpc]
    private void SetCharaClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Unshow();
        if (!IsOwner) return;        
        PlayerDataContainer.SetData(charaSet.charaterData.charaterIndex);
        NetworkPlayer.ownPlayer.SetCharaData(PlayerDataContainer.charaDataIndex);
        //SceneManageCtr.instance.JoinSlatLobby();
    }

}
