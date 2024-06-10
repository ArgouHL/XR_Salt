using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ShowResult : NetworkBehaviour
{
    public TMP_Text testName;
    private NetworkVariable<int> winnerChara = new NetworkVariable<int>();



    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;
        LeanTween.delayedCall(5, () => SceneManageCtr.instance.LoadEndScene());
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
        var name = CharaDatas.GetCharaData(newValue).charaterName;
        Debug.Log(name);
        testName.text = name;
        
    }
}
