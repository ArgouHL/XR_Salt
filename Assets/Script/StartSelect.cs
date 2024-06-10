using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSelect : MonoBehaviour
{
    public GameObject PC;
    public GameObject VR;
    private void Awake()
    {


#if UNITY_STANDALONE || UNITY_EDITOR
        VR.SetActive(true);
#else
        PC.SetActive(true);
#endif
    }
}
