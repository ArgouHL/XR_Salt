using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss: bossbase
{



    public override void Attack()
    {
        base.Attack();
        Debug.Log("Atk2");
    }

    public override void Def()
    {
        Debug.Log("Def");
    }

    public void Def2()
    {
        Debug.Log("Def");
    }
}



public class BossesCtr : MonoBehaviour
{


    public bossbase[] _boss;
    private void Start()
    {
        _boss[0].Def();
        _boss[1].Def();
    }


}

public abstract class bossbase: MonoBehaviour
{
    public Obj2 bossdata;
    protected int int21=21;


    public void Start()
    {
        Debug.Log(int21);
    }
    public virtual void Attack()
    {
   


    }

    public abstract void Def();
  
}
