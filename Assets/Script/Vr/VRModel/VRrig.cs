using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRrig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Transform headConstrain;
    public Transform neckConstrain;

    private Vector3 headNeckOffest;
    private float neckBodyOffest;

    // Start is called before the first frame update
    void Start()
    {
        neckBodyOffest = neckConstrain.position.y - transform.position.y;
        headNeckOffest = neckConstrain.position - headConstrain.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       var frontVector = headConstrain.forward;
        frontVector.y = 0;         
        transform.forward = frontVector.normalized;
        
        transform.position = headConstrain.position+ headConstrain.rotation* headNeckOffest- new Vector3(0,neckBodyOffest,0);
        Debug.DrawRay(headConstrain.position, headConstrain.rotation.eulerAngles, Color.blue);
       
        head.Map();
        leftHand.Map();
        rightHand.Map();
       

    }



}

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPosOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPosOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}
