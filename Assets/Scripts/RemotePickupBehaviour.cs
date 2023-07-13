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
    private static RemotePickupBehaviour _instance;
    public static RemotePickupBehaviour Instance { get { return _instance; } }

    private bool isRecalled = false;
    private bool gripPressedRight = false;
    private bool gripPressedLeft = false;
    public XRGrabInteractable grabbedObject;
    public XRRayInteractor usedInteractor;
    public XRRayInteractor interactorRefLeft;
    public XRRayInteractor interactorRefRight;
    [SerializeField] private float gripSensitivity = 0.3f;

    public GameObject controllerLeft;
    public GameObject controllerRight;

    private new void Awake()
    {
        base.Awake();

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public void Update()
    {
        CheckForInput();
    }

    public void CheckForInput()
    {
        if (isRecalled)
        {
            return;
        }

        gripPressedRight = Input.GetAxis("XRI_Right_Grip") > gripSensitivity;
        gripPressedLeft = Input.GetAxis("XRI_Left_Grip") > gripSensitivity;

        if (gripPressedRight)
        {
            RecallObject(controllerRight, interactorRefRight);
        }
        else if (!gripPressedRight)
        {
            ReleaseObject(interactorRefRight);
        }

        if (gripPressedLeft)
        {
            RecallObject(controllerLeft, interactorRefLeft);
        }
        else if (!gripPressedLeft)
        {
            ReleaseObject(interactorRefLeft);
        }
    }
    public void RecallObject(GameObject currentController, XRRayInteractor currentInteractor)
    {
        if (gripPressedRight && currentController == controllerRight || gripPressedLeft && currentController == controllerLeft)
        {
            isRecalled = true;

            try
            {
                grabbedObject = currentInteractor.interactablesSelected[0] as XRGrabInteractable;
            }
            catch 
            {
                isRecalled = false;
                return;
            }

            ForceDeselect(currentInteractor);
            currentInteractor.useForceGrab = true;
            ForceSelect(currentInteractor,grabbedObject);
        }
        isRecalled = false;
    }

    public void ReleaseObject(XRRayInteractor currentInteractorRef)
    {
        currentInteractorRef.useForceGrab = false;
        grabbedObject = null;
    }
    public void ForceDeselect(XRRayInteractor interactor)
    {
        gameObject.GetComponent<CustomInteractionManager>().ForceDeselect(interactor);
    }
    public void ForceSelect(XRRayInteractor interactor, IXRSelectInteractable interactable)
    {
        gameObject.GetComponent<CustomInteractionManager>().SelectEnter(interactor, interactable);
    }
}



