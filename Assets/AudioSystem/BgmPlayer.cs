using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlayer : AudioPlayerClass
{
    public static BgmPlayer instance;
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
}
