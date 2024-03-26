using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ColliderCell : MonoBehaviour
{
    private float volume = 1;
    private SaltGrid saltGrid;
    private Coroutine recoveryCoro;
    private const float recoverySpeed = 0.3f;
    private VisualEffect vfx;

    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();
        vfx.SetFloat("height", volume);
    }
    internal void SetCell(SaltGrid _saltGrid)
    {
        saltGrid = _saltGrid;
    }


    public float Digged()
    {
        float v = volume;


        if (recoveryCoro != null)
            StopCoroutine(recoveryCoro);
        recoveryCoro = StartCoroutine(RecoveryIE());



        return v;
    }

    private IEnumerator RecoveryIE()
    {
        volume = 0;
        vfx.SetFloat("height", volume);     
        yield return new WaitForSeconds(2);
        while (volume < 1)
        {

            volume += Time.deltaTime * recoverySpeed;
            vfx.SetFloat("height", volume);
            yield return null;

        }
        volume = 1;
        vfx.SetFloat("height", volume);
        recoveryCoro = null;
    }



}
