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
    private NetworkVariable<float> time = new NetworkVariable<float>();
    public Image img;
    public float waitTime = 30;
    public float gameTime = 60;
    private static List<KeyValuePair<ulong,float>> scoreList;
    public TMP_Text hintText;
    private void OnEnable()
    {
        time.OnValueChanged += TimeShow;
    }

    private void TimeShow(float previousValue, float newValue)
    {
        img.fillAmount = Mathf.Lerp(0, 1, newValue / gameTime);
    }

    private void Start()
    {
       if (IsServer)
            StartCount();
    }

    private void StartCount()
    {
        time.Value = gameTime;
        StartCoroutine(CountIE());
    }

    private IEnumerator CountIE()
    {
        yield return new WaitForSeconds(waitTime);
        hintText.text = "把鹽盡力推到自己的鹽山";
        while (time.Value > 0)
        {
            time.Value -= Time.deltaTime;
            yield return null;
        }
        scoreList=PlayZonesCtr.instance.GetScores();
        yield return new WaitForSeconds(3);
        SceneManageCtr.instance.LoadEndCraftScene();
    }


    public static ulong GetHighestPlayer()
    {
       //scoreList.OrderByDescending(kvp => kvp.Value).First();
        return scoreList.OrderByDescending(kvp => kvp.Value).First().Key;
    }


}
