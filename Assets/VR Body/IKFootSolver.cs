using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    public bool isMovingForward;
    public bool isMovingBackward;
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] IKFootSolver otherFoot = default;
    [SerializeField] float speed = 4;
    [SerializeField] float stepDistance = .2f;
    [SerializeField] float backStepDistance = .1f;
    [SerializeField] float stepLength = .2f;
    [SerializeField] float sideStepLength = .1f;

    [SerializeField] float stepHeight = .3f;
    [SerializeField] Vector3 footOffset = default;

    public Vector3 footRotOffset;
    public float footYPosOffset = 0.1f;

    public float rayStartYOffset = 0;
    public float rayLength = 1.5f;

    float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;
    private float angle;
    private bool isMoved = false;
    private VRrig vRrig;
    Vector3 oldhitPos;
    private void Start()
    {
        footSpacing = transform.localPosition.x;
        oldhitPos = body.position;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 2;
        vRrig = body.GetComponent<VRrig>();
    }

    // Update is called once per frame

    void Update()
    {
        transform.position = currentPosition + Vector3.up * footYPosOffset;
        transform.localRotation = Quaternion.Euler(footRotOffset);

        Ray ray = new Ray(body.position + (body.right * footSpacing) + Vector3.up * rayStartYOffset, Vector3.down);

        Debug.DrawRay(body.position + (body.right * footSpacing) + Vector3.up * rayStartYOffset, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, rayLength, terrainLayer.value))
        {

            float dis = Vector3.Distance(newPosition, info.point);

            if (lerp >= 1 && !otherFoot.IsMoving())
            {
               // Vector3 direction = Vector3.ProjectOnPlane(info.point - currentPosition, Vector3.up).normalized;
                Vector3 _dir = info.point - oldhitPos;
                Debug.DrawRay(body.position + Vector3.up, _dir, Color.green, 0.3f);
                oldhitPos = info.point;
                angle = Vector3.Angle(body.forward, _dir);
                isMovingForward = false;
                isMovingBackward = false;
                isMovingForward = angle < 130;
                isMovingBackward = angle > 50;

                if (dis > backStepDistance && isMovingBackward)
                {
                    lerp = 0;
                    newPosition = info.point + _dir * stepLength + Quaternion.LookRotation(body.forward) * footOffset;
                    newNormal = info.normal;
                    isMoved = true;
                }
                else if (dis > stepDistance)
                {
                    lerp = 0;
                    if (isMovingForward)
                    {
                        newPosition = info.point + _dir * stepLength + Quaternion.LookRotation(body.forward) * footOffset;
                    }
                    else
                    {
                        newPosition = info.point + _dir * sideStepLength + Quaternion.LookRotation(body.forward) * footOffset;

                    }
                    newNormal = info.normal;
                    isMoved = true;
                }
                else if (isMoved || vRrig.isRotating)
                {
                    lerp = 0;
                    isMoved = false;
                    newPosition = info.point + Quaternion.LookRotation(body.forward) * footOffset;
                    newNormal = info.normal;
                }
            }
            
        }




        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.1f);
    }



    public bool IsMoving()
    {
        return lerp < 1;
    }



}
