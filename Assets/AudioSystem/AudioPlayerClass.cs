using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerClass : MonoBehaviour
{


    public void PlaySound(SoundData gData)
    {
        AudioSource soundPlayer = GetComponent<AudioSource>();
        soundPlayer.volume = gData.volume;
        soundPlayer.clip = gData.clip;
        soundPlayer.Play();

    }
    public void PlaySound(MultiLangVoices multiLangVoices)
    {
        switch (LangugeSelecter.languge)
        {
            case Lang.Chi:
            default:
                PlaySound(multiLangVoices.chiVoice);
                break;
            case Lang.TW:
                PlaySound(multiLangVoices.twVoice);
                break;

        }
    }
}
