using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GuildingSound", menuName = "ScriptableObject /GuildingSound")]
public class GuidingSoundData : ScriptableObject
{
    public Lang language;
    public int Id;
    public SoundData soundData;
}

[System.Serializable]
public class SoundData
{
    public float volume;
    public float duration;
    public AudioClip clip;        
}
