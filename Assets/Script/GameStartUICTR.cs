using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartUICTR : MonoBehaviour
{
    public CanvasGroup cg;
    public TMP_Text text;

    private void OnEnable()
    {
        CharaSelectCtr.ShowUI += Show;
        CharaSelectCtr.onPlayerSelect += UpdateShow;
    }
    private void OnDisable()
    {
        CharaSelectCtr.ShowUI += Show;
        CharaSelectCtr.onPlayerSelect += UpdateShow;
    }


    private void UpdateShow(string s)
    {
        cg.interactable = true;
        cg.alpha = 1;
        Show(s);
    }

    private void Show(string s)
    {
        text.text = s;
    }

    public void StartGame()
    {
        CharaSelectCtr.instance.PlayChans();
    }
}
