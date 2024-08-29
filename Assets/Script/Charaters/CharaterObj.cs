using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharaterData
{
    public int charaterIndex;
    public string charaterName;
    public Material charaSkinMaterial;

}

[CreateAssetMenu(fileName = "New Chara",menuName = "ScriptableObject/CharaData")]
public class CharaterObj : ScriptableObject
{
    public CharaterData charaterData;

}


