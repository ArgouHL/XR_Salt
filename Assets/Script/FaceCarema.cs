using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCarema : MonoBehaviour
{
    protected void Update()
    {

        transform.forward = Camera.main.transform.forward;
    }

}
