using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class VRrig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Transform headConstrain;
    public Transform neckConstrainOffsetedTracker;
    public Transform spineRotTrack;   
    public float spineRotFactor = 0.5f;
    public float spineRotOffeset;

    public Transform neckRotTrack;
    public float neckRotOffeset;

    private Vector3 orgPos;
    private Vector3 headBodyOffest;
    private Vector3 headNeckOffest;
    private Vector3 neckSpineOffest;
    public Vector3 rotationOffeset;
    public Vector3 positionOffeset;
    private float originHeight;
    public float maxLow;

    public bool isRotating = false;
    private bool isMoving = false;
    private float lerp = 1;
    private Vector3 oldPos;
    public float heightOffset;
    private Quaternion spineRotation;
    private Quaternion neckRotation;
    private LTDescr delayedCallTween;
    private Vector3 lastFrontVector;
    // Start is called before the first frame update
    void Start()
    {
        orgPos = transform.position;
        originHeight = head.vrTarget.position.y;
        headBodyOffest = headConstrain.position - transform.position;
        neckSpineOffest = head.vrTarget.position - spineRotTrack.position;

        headNeckOffest = neckConstrainOffsetedTracker.position - headConstrain.position;


    }

    // Update is called once per frame
    void Update()
    {
        spineRotTrack.localRotation = spineRotation;
        neckRotTrack.localRotation = neckRotation;
        lastFrontVector = head.vrTarget.forward;    
        Debug.DrawRay(transform.position + Vector3.up, lastFrontVector);
        lastFrontVector.y = 0;
        lastFrontVector = lastFrontVector.normalized;

        //vrTarget.rotation * Quaternion.Euler(trackingRotationOffset)
        float angle = Vector3.Angle(transform.forward, lastFrontVector.normalized);

        if (lerp >= 1)
        {
            lerp = 0;
            if (Vector3.Distance(oldPos, transform.position) > 0.5f)
                isRotating = true;
            oldPos = transform.position;

        }
        //transform.rotation= Quaternion.LookRotation(frontVector, Vector3.up);
        if (angle > 35)
        {
            isRotating = true;
            if (delayedCallTween != null)
            {
                LeanTween.cancel(delayedCallTween.id);
                delayedCallTween = null;
            }
             
        }

        if (angle < 5)
        {
            if (delayedCallTween == null)
            {
                delayedCallTween = LeanTween.delayedCall(0.3f, () => isRotating = false);

            }


        }

        if (isRotating)
        {
            var rot = Quaternion.LookRotation(lastFrontVector, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 540*Time.deltaTime);
        }

        //Debug.DrawRay(transform.position + Vector3.up, transform.forward, Color.cyan);
        // transform.position = headConstrain.position -  transform.rotation* headBodyOffest;

        Vector3 offset = head.vrTarget.rotation * Quaternion.Euler(rotationOffeset) * headNeckOffest;
        offset.y = 0;
        //transform.position = headConstrain.position - new Vector3(0, headBodyOffest.y, 0);
        float heightDiff = originHeight - head.vrTarget.position.y;
       
        float spineAngle = 0;
        float zDistance = 0;
        if (heightDiff > maxLow)
        {
            float c = neckSpineOffest.y;
            float a = neckSpineOffest.y - heightDiff + maxLow;
            spineAngle = Mathf.Acos(a / c) * Mathf.Rad2Deg * spineRotFactor;
            zDistance = Mathf.Sqrt((c * c) - (a * a)) * spineRotFactor;
        }
        
      
        Vector3 newPos = head.vrTarget.position -  new Vector3(0, headBodyOffest.y, 0) + offset + transform.rotation * (new Vector3(0, 0, -zDistance)) + head.vrTarget.rotation * positionOffeset;
       // Vector3 newPos = head.vrTarget.position + _rot * new Vector3(0, headBodyOffest.y, 0) + offset + transform.rotation * (new Vector3(0, 0, -zDistance)) + head.vrTarget.rotation * positionOffeset;
       

       if(newPos.y - orgPos.y<-maxLow)
        {
            newPos.y = -maxLow;
        }
        newPos.y += heightOffset;
        //newPos.y = orgPos.y-newPos.y <= maxLow ? -maxLow : newPos.y;

        spineRotation = Quaternion.Euler(new Vector3(spineAngle + spineRotOffeset, 0, 0));
        neckRotation = Quaternion.Euler(new Vector3(head.vrTarget.localRotation.eulerAngles.x-spineAngle + neckRotOffeset, 0, 0));
        transform.position = newPos;
        


        head.Map();
        leftHand.Map();
        rightHand.Map();
        
        if (lerp <= 1)
        {

            lerp += Time.deltaTime;
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

    public virtual void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPosOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);


    }

    public float RigRotationX()
    {
        float _x = rigTarget.localRotation.eulerAngles.x;

        return _x > 180 ? _x : 180 - _x;
    }


}

[System.Serializable]
public class VRMapHead:VRMap
{
    public float upRotateLimit;
    public float downRotateLimit;
    public float angle;
    public override void Map()
    {



        rigTarget.position = vrTarget.TransformPoint(trackingPosOffset);



        angle = vrTarget.rotation.eulerAngles.z;
        
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

  

}
