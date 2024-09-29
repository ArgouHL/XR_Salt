using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerClass : MonoBehaviour
{
    public AudioSource soundPlayer;
    internal virtual void Awake()
    {
        soundPlayer = GetComponent<AudioSource>();
    }
    public void PlaySound(SoundData gData)
    {

        soundPlayer.volume = gData.volume;
        soundPlayer.clip = gData.clip;
        soundPlayer.Play();

    }
    public void PlaySound(MultiLangVoices multiLangVoices)
    {
        print(LangugeSelecter.languge);
        print(multiLangVoices.chiVoice.clip);
        print(multiLangVoices.twVoice.clip);
        switch (LangugeSelecter.languge)
        {
            case Lang.Chi:
            default:
                if (multiLangVoices.chiVoice == null)
                    return;
                PlaySound(multiLangVoices.chiVoice);
                break;
            case Lang.TW:
                if (multiLangVoices.twVoice == null)
                    return;
                PlaySound(multiLangVoices.twVoice);
                break;

        }
    }
}
