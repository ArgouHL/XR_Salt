using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private int num;
    [SerializeField] private GameObject g;

    private void Start()
    {
       for(int i = 0;i<num;i++)
        Instantiate(g, new Vector3(Random.Range(0, 2f), Random.Range(0,2f), Random.Range(0, 2f)),Quaternion.identity);
    }
}
