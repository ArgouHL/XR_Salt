using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ObjList", menuName = "ScriptableObject/ObjList")]
public class ObjList : ScriptableObject
{
   public List<Obj2> objs;
    public List<objorg> obj2s;
}


[Serializable]
public class objorg
{
    public string objname;
    public int id;
    public int value;
    public Animator ani;
    public GameObject g;
    public Mesh mesh;
    public RodCtr ts;



}

