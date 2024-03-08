using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndriodTest : MonoBehaviour
{
    [SerializeField] private CanvasGroup c;
    private void Awake()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        c.alpha = 1;
        c.interactable = true;
#endif
    }
}
