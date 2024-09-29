using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ShowResult : NetworkBehaviour
{
    public TMP_Text testName;
    public NetworkVariable<int> winnerChara = new NetworkVariable<int>();
    public GuidingSoundData winSfx;


    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;
        LeanTween.delayedCall(10, () => { 
            SceneManageCtr.instance.LoadEndScene();
            SfxPlayer.instance.StopPlay();
        });

        winnerChara.Value = CharaSelectCtr.GetCharaIndex(SaltGame.GetHighestPlayer());

    }

    private void OnEnable()
    {
        winnerChara.OnValueChanged += ShowWinner;
    }

    private void OnDisable()
    {
        winnerChara.OnValueChanged += ShowWinner;
    }

    private void ShowWinner(int previousValue, int newValue)
    {
        PlayWin(newValue);
        PlayWinClientRpc(newValue);
    }


    public void PlayWin(int newValue)
    {
        var name = CharaDatas.GetCharaData(newValue).charaterName;
        Debug.Log(name);
        testName.text = "推鹽最多的是:" + "\r\n" + name;
        SfxPlayer.instance.PlaySound(winSfx.soundData);
    }

    [ClientRpc]
    public void PlayWinClientRpc(int newValue)
    {
        PlayWin(newValue);
    }
}
