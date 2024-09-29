using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GuideAudioPlayer : AudioPlayerClass
{
    public static GuideAudioPlayer instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        base.Awake();
    }
    public GuidingSoundData testData;




    [ContextMenu("testP")]
    public void TestPlayGuide()
    {
        PlaySound(testData.soundData);
    }

}
