using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltMount : MonoBehaviour
{
    private float volume;
    [SerializeField] private Transform mount;
    private void Start()
    {
        volume = 100;
        UpdateMount();

    }

    internal void AddVolume(float v)
    {
        volume += v;
        UpdateMount();
    }

    private void UpdateMount()
    {
        mount.localScale = Vector3.one * volume * 0.1f;
    }
}
