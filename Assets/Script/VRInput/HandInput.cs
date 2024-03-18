using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset xrInput;
    private InputActionMap xrInterection;
    private InputAction xrGrip;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private Side side;
    float grapValue => xrGrip.ReadValue<float>();

    private void Awake()
    {
        switch (side)
        {
            case Side.Left:
                xrGrip = xrInput.FindActionMap("XRI LeftHand Interaction").FindAction("Select Value");
                break;

            case Side.Right:
                xrGrip = xrInput.FindActionMap("XRI RightHand Interaction").FindAction("Select Value");
                break;

        }
    }


    // Update is called once per frame
    private void Update()
    {
        handAnimator.SetFloat("Grip", grapValue);
    }
}

enum Side { Left, Right }
