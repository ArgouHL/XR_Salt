using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Showvelocity : MonoBehaviour
{
    [SerializeField] Rigidbody rig;
    [SerializeField] TMP_Text t;

    private void Update()
    {
        t.text = rig.velocity.ToString();
    }
}
