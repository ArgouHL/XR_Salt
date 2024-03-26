using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class VfxTransform : MonoBehaviour
{
  
    private VisualEffect ve;
    private void Awake()
    {
        ve = GetComponent<VisualEffect>();
    }

    private void FixedUpdate()
    {
        var r = transform.rotation * Quaternion.Euler(Vector3.right * 90);
        ve.SetVector3("Rotation", r.eulerAngles);
      
    }
}
