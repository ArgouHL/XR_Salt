using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEditor;

public class ChanAndSaltAniCtr : NetworkBehaviour
{
    public DissovleCtr chanDissovleCtr;
    public DissovleCtr saltDissovleCtr;
    public Animator saltAni;
    // public float aniAndSoudDuration = 32;

    public GuidingSoundData[] startGuideDatas;
    public GuidingSoundData[] guideDatas;
    public GuidingSoundData[] endGuideDatas;
    public MultiLangVoices startGuideVoice;
    public MultiLangVoices[] guideVoices;
    public MultiLangVoices endGuideVoice;

    public int guideStep = 0;

    private void Init()
    {
        startGuideVoice = new MultiLangVoices();
        guideVoices = new MultiLangVoices[5];
        for (int i = 0; i < 5; i++)
        {
            guideVoices[i] = new MultiLangVoices();
        }
        endGuideVoice = new MultiLangVoices();
    }

    private void Start()
    {
        Init();
        foreach (var data in startGuideDatas)
        {
            SortWithLang(data, startGuideVoice);
        }
        foreach (var data in guideDatas)
        {
            SortWithLang(data, guideVoices[data.Id]);
        }
        foreach (var data in endGuideDatas)
        {
            SortWithLang(data, endGuideVoice);
        }
    }


    private void SortWithLang(GuidingSoundData data, MultiLangVoices voice)
    {
        switch (data.language)
        {
            case Lang.Chi:
                voice.chiVoice = data.soundData;
                break;
            case Lang.TW:
                voice.twVoice = data.soundData;
                break;
        }
        if (voice.time < data.soundData.duration)
        {
            voice.time = data.soundData.duration;
        }

    }


    public override void OnNetworkSpawn()
    {

    }

    private void OnEnable()
    {
        SelectSceneEvent.instance.ChanShowEvent += AllShow;
    }

    private void OnDisable()
    {
        SelectSceneEvent.instance.ChanShowEvent += AllShow;
    }

    [ContextMenu("Ani")]
    public void AllShow()
    {
        StartCoroutine(GuidingAni());
        //afterSound
        // LeanTween.delayedCall(aniAndSoudDuration, SceneManageCtr.instance.LoadPlayScene);

    }

    
    private IEnumerator GuidingAni()
    {
        ChanShow();
        ChanShowClientRpc();
          yield return new WaitForSeconds(3);
        ChanStartGuide();
        ChanStartGuideClientRpc();
       
        yield return new WaitForSeconds(startGuideVoice.time);
        SaltShow();
        SaltShowClientRpc();
        yield return new WaitForSeconds(3);
        for (int i = 0; i < guideVoices.Length; i++)
        {
            SaltGuide(i);
            SaltGuideClientRpc(i);
            yield return new WaitForSeconds(guideVoices[i].time);
        }
        GuideEnd();
        GuideEndClientRpc();
        yield return new WaitForSeconds(endGuideVoice.time);
    }

    public void ChanShow()
    {
        chanDissovleCtr.Apper();
    }

    public void ChanStartGuide()
    {
        GuideAudioPlayer.instance.PlaySound(startGuideVoice);
    }

    public void SaltShow()
    {
        saltDissovleCtr.Apper();
    }

    public void SaltGuide(int index)
    {
        saltAni.ResetTrigger("Play");
        saltAni.SetTrigger("Play");
        GuideAudioPlayer.instance.PlaySound(guideVoices[index]);
    }

    public void GuideEnd()
    {
        GuideAudioPlayer.instance.PlaySound(endGuideVoice);

    }

 
    [ClientRpc]
    public void ChanShowClientRpc()
    {
        ChanShow();
    }

    [ClientRpc]
    public void ChanStartGuideClientRpc()
    {
        GuideAudioPlayer.instance.PlaySound(startGuideVoice);
    }

    [ClientRpc]
    public void SaltShowClientRpc()
    {
        ChanStartGuide();
    }

    [ClientRpc]
    public void SaltGuideClientRpc(int index)
    {
        SaltGuide(index);
    }

    [ClientRpc]
    public void GuideEndClientRpc()
    {
        GuideEnd();
    }


}






