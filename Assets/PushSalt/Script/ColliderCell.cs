using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ColliderCell : MonoBehaviour
{
    private float volume;
    private float maxvolume = 1f;
   // [SerializeField] private SaltGrid saltGrid;
    private Coroutine recoveryCoro;
    private const float recoverySpeed = 0.3f;
    private VisualEffect vfx;

    private void Awake()
    {
        vfx = GetComponent<VisualEffect>();
        SetCell();
    }
    internal void SetCell()
    {
       // saltGrid = _saltGrid;
        volume = maxvolume;
        vfx.SetFloat("height", volume);
    }

    internal bool CanPush()
    {
        return volume>0;
    }
    public float Digged()
    {
        float v = volume;

        Debug.Log("Digged");
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
        while (volume < maxvolume)
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
