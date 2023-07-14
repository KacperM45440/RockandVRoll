using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaceableArea : MonoBehaviour
{
    public Vector3 areaPosition;
    public bool disableOnPlacement;
    public bool mustBeDropped;
    public GameObject neededObject;
    public GameObject leftControllerRef;
    public GameObject rightControllerRef;
    public RemotePickupBehaviour pickupRef;
    [SerializeField] private enum InteractionType { Stay = 1, Jump = 2, Spin = 3 }
    [SerializeField] private InteractionType chosenInteraction;

    private void OnTriggerEnter(Collider enteredCollider)
    {
        if (!mustBeDropped)
        {
            try
            {
                DeselectBoth(enteredCollider);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            
            PlaceInArea(enteredCollider.gameObject);
            return;
        }

        PlaceInArea(enteredCollider.gameObject);

    }
    public void PlaceInArea(GameObject givenObject)
    {
        switch ((int)chosenInteraction)
        {
            case 1:
                givenObject.transform.position = areaPosition;
                break;
        }

        if (disableOnPlacement)
        {
            givenObject.GetComponent<XRGrabInteractable>().enabled = false;
        }
    }

    private void Update()
    {
        Debug.Log(pickupRef.interactorRefLeft.interactablesSelected[0] as XRGrabInteractable);
    }
    public void DeselectBoth(Collider givenCollider)
    {
        bool leftHandHasSelection = leftControllerRef.GetComponent<XRDirectInteractor>().hasSelection || pickupRef.interactorRefLeft.hasSelection;
        bool rightHandHasSelection = rightControllerRef.GetComponent<XRDirectInteractor>().hasSelection || pickupRef.interactorRefRight.hasSelection;

        bool firstLeftCheck;
        bool secondLeftCheck;
        bool firstRightCheck;
        bool secondRightCheck;

        // Zadne z tych checkow nie przechodza poniewaz porownywane obiekty nie sa takie same
        // Roznica jest taka ze po zlapaniu do kostki doczepiany jest dynamic attach, trzeba bedzie poczytac w jaki sposob dobrze zrobic manualny zeby to sie (mam nadzieje) nie dzialo

        try
        {
            firstLeftCheck = GameObject.ReferenceEquals(givenCollider.gameObject, pickupRef.interactorRefLeft.interactablesSelected[0] as XRGrabInteractable);
        }
        catch
        {
            firstLeftCheck = false;
        }

        Debug.Log("Ray left check: " +firstLeftCheck);

        try
        {
            secondLeftCheck = GameObject.ReferenceEquals(givenCollider.gameObject, leftControllerRef.GetComponent<XRDirectInteractor>().interactablesSelected[0] as XRGrabInteractable);
        }
        catch
        {
            secondLeftCheck = false;
        }

        Debug.Log("Direct left check: " + secondLeftCheck);

        try
        {
            firstRightCheck = GameObject.ReferenceEquals(givenCollider.gameObject, pickupRef.interactorRefRight.interactablesSelected[0] as XRGrabInteractable);
        }
        catch
        {
            firstRightCheck = false;
        }

        Debug.Log("Ray right check: " + firstRightCheck);

        try
        {
            secondRightCheck = GameObject.ReferenceEquals(givenCollider.gameObject, rightControllerRef.GetComponent<XRDirectInteractor>().interactablesSelected[0] as XRGrabInteractable);
        }
        catch
        {
            secondRightCheck = false;
        }

        Debug.Log("Direct right check: " + secondRightCheck);

        if (leftHandHasSelection && GameObject.ReferenceEquals(givenCollider.gameObject, neededObject) && (firstLeftCheck || secondLeftCheck))
        {
            // Sprawdzanie jeszcze tagu to chyba overkill, ale niech zostanie
            if (pickupRef.grabbedObject.CompareTag(neededObject.tag))
            {
                pickupRef.ForceDeselect(pickupRef.interactorRefLeft);
                pickupRef.ForceDeselect(leftControllerRef.GetComponent<XRDirectInteractor>());
            }
        }

        if (rightHandHasSelection && GameObject.ReferenceEquals(givenCollider.gameObject, neededObject) && (firstRightCheck || secondRightCheck))
        {
            if (pickupRef.grabbedObject.CompareTag(neededObject.tag))
            {
                pickupRef.ForceDeselect(pickupRef.interactorRefRight);
                pickupRef.ForceDeselect(rightControllerRef.GetComponent<XRDirectInteractor>());
            }
        }
    }
}
