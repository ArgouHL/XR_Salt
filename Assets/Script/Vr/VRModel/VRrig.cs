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
    private Vector3 headBodyOffest;
    //private Vector3 headNeckOffest;
    //private float neckBodyOffest;
    public bool isRotating = false;
    private bool isMoving = false;
    private float lerp = 1;
    private Vector3 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        headBodyOffest = headConstrain.position - transform.position;
        //neckBodyOffest = neckConstrain.position.y - transform.position.y;
        //headNeckOffest = neckConstrain.position - headConstrain.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       var frontVector = headConstrain.forward;
        frontVector.y = 0;
        frontVector = frontVector.normalized;
        Debug.DrawRay(headConstrain.position, frontVector);
        float angle = Vector3.Angle(transform.forward, frontVector.normalized);

        if (lerp >= 1)
        {
            lerp = 0;
            if (Vector3.Distance(oldPos, transform.position) > 0.5f)
                isRotating = true;
            oldPos = transform.position;
            
        }

        if(angle > 30)
        {
            isRotating = true;
        }

        if(angle < 5)
        {
            isRotating = false;
        }
       
        if (isRotating)
        {
            var rot = Quaternion.LookRotation(frontVector, Vector3.up); 
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.2f);
        }

        transform.position = headConstrain.position - Quaternion.LookRotation(transform.forward) * headBodyOffest;
        //transform.position = headConstrain.position+ headConstrain.rotation* headNeckOffest- new Vector3(0,neckBodyOffest,0);
        Debug.DrawRay(headConstrain.position, headConstrain.rotation.eulerAngles, Color.blue);
       
        head.Map();
        leftHand.Map();
        rightHand.Map();

        if (lerp < 1)
        {

            lerp += Time.fixedDeltaTime;
        }
        else
        {
            //oldPosition = newPosition;
            //oldNormal = newNormal;
        }

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
