using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaDatas : MonoBehaviour
{
    [SerializeField] private CharaterObj[] charas;

    private static Dictionary<int, CharaterData> charaDict;

    private void Start()
    {
        charaDict = new Dictionary<int, CharaterData>();
        foreach (var chara in charas)
        {
            charaDict.Add(chara.charaterData.charaterIndex, chara.charaterData);
        } 
            
    }

    public static CharaterData GetCharaData(int index)
    {
        return charaDict[index];
    }
}
