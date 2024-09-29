using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : AudioPlayerClass
{
    public static SfxPlayer instance;
    internal override void Awake()
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

    public void StopPlay()
    {
        soundPlayer.Stop();
    }
}
