using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }
    public GuidingSoundData testData;

    [ContextMenu("testP")]
    public void TestPlayGuide()
    {
        PlaySound(testData.soundData);
    }

}
