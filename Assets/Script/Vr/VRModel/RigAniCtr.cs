using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigAniCtr : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private InputActionAsset xrInput;
    private InputAction leftGrip;
    private InputAction rightGrip;
    float leftGrapValue => leftGrip.ReadValue<float>();
    float rightGrapValue => rightGrip.ReadValue<float>();
    private void Awake()
    {
        animator = GetComponent<Animator>();
        leftGrip = xrInput.FindActionMap("XRI LeftHand Interaction").FindAction("Select Value");
        rightGrip = xrInput.FindActionMap("XRI RightHand Interaction").FindAction("Select Value");
    }

    private void Update()
    {
        animator.SetFloat("LeftGrip", leftGrapValue);
        animator.SetFloat("RightGrip", rightGrapValue);
    }
}
