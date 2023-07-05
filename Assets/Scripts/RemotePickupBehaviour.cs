using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class RemotePickupBehaviour : MonoBehaviour
{
    private bool triggerPressed = false;
    [SerializeField] private XRRayInteractor interactorRef;
    [SerializeField] private float gripSensitivity = 0.3f;
    private void Update()
    {
        // Check if the trigger button is pressed
        bool triggerPressedValue = Input.GetAxis("XRI_Right_Grip") > gripSensitivity;

        if (triggerPressedValue && !triggerPressed)
        {
            interactorRef.useForceGrab = true;
        }
        else if (!triggerPressedValue && triggerPressed)
        {
            interactorRef.useForceGrab = false;
        }

        triggerPressed = triggerPressedValue;
    }
}



