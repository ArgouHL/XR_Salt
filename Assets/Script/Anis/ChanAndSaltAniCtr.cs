using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChanAndSaltAniCtr : NetworkBehaviour
{
    public DissovleCtr chanDissovleCtr;
    public DissovleCtr saltDissovleCtr;
    public Animator saltAni;
    public float aniAndSoudDuration = 32;

    private void Start()
    {
        SelectSceneEvent.instance.ChanShowEvent += AllShow;
    }

    public void AllShow()
    {
        ChanShowClientRpc();
        ChanShow();
        //afterSound
        LeanTween.delayedCall(aniAndSoudDuration, SceneManageCtr.instance.LoadPlayScene);
        
    }

    [ContextMenu("Ani")]
    public void ChanShow()
    {
        chanDissovleCtr.Apper();
        LeanTween.delayedCall(2, SaltShow);
    }

    public void SaltShow()
    {
        saltDissovleCtr.Apper();
        LeanTween.delayedCall(3, PlayAni);
    }


    [ClientRpc]
    private void ChanShowClientRpc()
    {
        ChanShow();
        //if (NetworkPlayer.ownPlayer.OwnerClientId != clientID)
        //    return;
        //PlayerDataContainer.SetData(charaSet.charaterData.charaterIndex);
        //NetworkPlayer.ownPlayer.SetCharaData(PlayerDataContainer.charaDataIndex);

    }


    public void PlayAni()
    {
        saltAni.SetTrigger("Play");
    }



}
