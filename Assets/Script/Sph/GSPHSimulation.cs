using UnityEngine;
using System.Collections.Generic;

public class GSPHSimulation : MonoBehaviour
{
 



    float SmoothingKernel(float radius, float dst)
    {
        float x = Mathf.Abs(dst)- radius;

        float value= (-x/(Mathf.Sqrt(x*x)+0.0001f)+1)*0.5f;
        return value;
    }

}