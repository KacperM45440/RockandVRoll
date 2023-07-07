using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Linq;

public class RemotePickupBehaviour : XRBaseInteractor
{
    private bool isRecalled = false;
    private bool gripPressed = false;
    private XRGrabInteractable grabbedObject;
    [SerializeField] private XRRayInteractor interactorRef;
    [SerializeField] private float gripSensitivity = 0.3f;

    public GameObject controller;

    public void Update()
    {
        if (!isRecalled)
        {
            RecallObject();
        }
    }

    public void RecallObject()
    {
        bool gripPressedValue = Input.GetAxis("XRI_Right_Grip") > gripSensitivity;

        if (gripPressedValue && !gripPressed)
        {
            isRecalled = true;

            try
            {
                grabbedObject = interactorRef.interactablesSelected[0] as XRGrabInteractable;
            }
            catch 
            {
                isRecalled = false;
                return;
            }

            ForceDeselect();
            interactorRef.useForceGrab = true;
            ForceSelect(grabbedObject);
        }
        else if (!gripPressedValue && gripPressed)
        {
            interactorRef.useForceGrab = false;
        }

        gripPressed = gripPressedValue;
        isRecalled = false;
    }

    public void ForceDeselect()
    {
        gameObject.GetComponent<CustomInteractionManager>().ForceDeselect(interactorRef);
    }
    public void ForceSelect(IXRSelectInteractable interactable)
    {
        gameObject.GetComponent<CustomInteractionManager>().SelectEnter(interactorRef, interactable);
    }
}



