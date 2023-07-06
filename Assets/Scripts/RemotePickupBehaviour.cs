using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class RemotePickupBehaviour : MonoBehaviour
{
    private bool gripPressed = false;
    [SerializeField] private XRRayInteractor interactorRef;
    [SerializeField] private float gripSensitivity = 0.3f;

    public void Update()
    {
        RecallObject();
    }


    public void RecallObject()
    {
        bool gripPressedValue = Input.GetAxis("XRI_Right_Grip") > gripSensitivity;

        if (gripPressedValue && !gripPressed)
        {
   
        }

        gripPressed = gripPressedValue;
    }
}



