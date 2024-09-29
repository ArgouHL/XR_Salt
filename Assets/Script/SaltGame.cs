using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SaltGame : NetworkBehaviour
{
    public static SaltGame instance;
    private void Awake()
    {
        instance = this;
    }
    public Transform camPos;    
    private NetworkVariable<float> time = new NetworkVariable<float>();
    public Image img;
    public float waitTime = 30;
    public float gameTime = 60;
    private static List<KeyValuePair<ulong, float>> scoreList;
    public TMP_Text hintText;
    public GuidingSoundData chStart;
    public GuidingSoundData chCount;
    public GuidingSoundData chEnd;
    public GuidingSoundData twEnd;
    [HideInInspector]
    public NetworkVariable<bool> gameEnded = new NetworkVariable<bool>();


    private void OnEnable()
    {
        time.OnValueChanged += TimeShow;
    }

    private void OnDisable()
    {
        time.OnValueChanged -= TimeShow;
    }

    private void TimeShow(float previousValue, float newValue)
    {
        img.fillAmount = Mathf.Lerp(0, 1, newValue / gameTime);
    }

    private void Start()
    {
        GameEnd(true);
        if (IsServer)
            StartCount();
    }
    private void GameEnd(bool b)
    {
        if (!IsServer)
            return;
        gameEnded.Value = b;
    }

    private void StartCount()
    {
        time.Value = gameTime;
        StartCoroutine(CountIE());
    }

    private IEnumerator CountIE()
    {
        yield return new WaitForSeconds(waitTime);
        PlayStart();
        PlayStartClientRpc();
        yield return new WaitForSeconds(chStart.soundData.duration);
        PlayCount();
        PlayCountClientRpc();
        yield return new WaitForSeconds(chCount.soundData.duration);
        GameEnd(false);
        hintText.text = "把鹽盡力推到自己的鹽山";
        while (time.Value > 0)
        {
            time.Value -= Time.deltaTime;
            yield return null;
        }
        scoreList = PlayZonesCtr.instance.GetScores();
        Playend();
        PlayendClientRpc();
        GameEnd(true);
        yield return new WaitForSeconds(20);
        SceneManageCtr.instance.LoadEndCraftScene();
    }


    public static ulong GetHighestPlayer()
    {
        //scoreList.OrderByDescending(kvp => kvp.Value).First();
        return scoreList.OrderByDescending(kvp => kvp.Value).First().Key;
    }



    public void PlayStart()
    {
        GuideAudioPlayer.instance.PlaySound(chStart.soundData);
    }

    public void PlayCount()
    {
        GuideAudioPlayer.instance.PlaySound(chCount.soundData);
    }
    [ClientRpc]
    public void PlayStartClientRpc()
    {
        PlayStart();
    }
    [ClientRpc]
    public void PlayCountClientRpc()
    {
        PlayCount();
    }

    public void Playend()
    {
        GuideAudioPlayer.instance.PlaySound(chEnd.soundData);
    }

    [ClientRpc]
    public void PlayendClientRpc()
    {
        Playend();
    }



}
