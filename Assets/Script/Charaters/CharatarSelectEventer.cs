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
    private NetworkVariable<bool> chooseable = new NetworkVariable<bool>(true);
    private CharaSelectCtr charaSelect;


    private void Start()
    {
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

        DebugLogConsole.AddCommandInstance("CharaChoosed" + charaSet.charaterData.charaterName, "CharaChoosed" + charaSet.charaterData.charaterName, "CharaChoosed", this);
        charaSelect = GetComponentInParent<CharaSelectCtr>();


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
        CharaSelectCtr.instance.AllDisapper();
        CharaChoosed();

    }

    internal void Unshow()
    {
        Debug.Log("Unshow");
        CharaObj.SetActive(false);
    }

    [ContextMenu("OnTriggered")]
    public void OnTriggered()
    {
        CharaSelectCtr.instance.AllDisapper();
        CharaChoosed();
    }

    public void CharaChoosed()
    {
        nameText.text = "choosed";
        //if (IsServer)
        //    GetComponentInChildren<MeshRenderer>().material.color = UnityEngine.Random.ColorHSV();
        CharaChoosedServerRpc(NetworkPlayer.ownPlayer.OwnerClientId);
        VRCtr.instance.ChangeColor(charaSet.charaterData.charaterIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CharaChoosedServerRpc(ulong clientId)
    {
        if (!chooseable.Value)
            return;

        Unshow();
        chooseable.Value = false;
        UnshowClientRpc();
        SetChara(clientId, charaSet.charaterData.charaterIndex);
        charaSelect.AddCount();

    }

    private void SetChara(ulong clientId, int charaterIndex)
    {
        NetworkPlayer player = AllPlayerControl.instance.GetPlayer(clientId);
        player.CharaChange(charaterIndex);
    }

    [ClientRpc]
    private void UnshowClientRpc()
    {
        Unshow();
        //if (NetworkPlayer.ownPlayer.OwnerClientId != clientID)
        //    return;
        //PlayerDataContainer.SetData(charaSet.charaterData.charaterIndex);
        //NetworkPlayer.ownPlayer.SetCharaData(PlayerDataContainer.charaDataIndex);

    }



}
