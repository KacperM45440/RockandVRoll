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
            if (enteredCollider.gameObject.name.Equals(neededObject.name))
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
            }

            return;
        }

        if (enteredCollider.gameObject.name.Equals(neededObject.name))
        {
            PlaceInArea(enteredCollider.gameObject);
        }

    }
    public void PlaceInArea(GameObject givenObject)
    {
        // Mam dziwne wrazenie ze przy bezposrednim trzymaniu przedmiotu w rece czasami obiekt teleportuje sie do miejsca docelowego razem z graczem
        // Nie mam na razie pomyslu na to jak to zbadac ani co z tym zrobic

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

    public void DeselectBoth(Collider givenCollider)
    {
        bool leftHandHasSelection = leftControllerRef.GetComponent<XRDirectInteractor>().hasSelection || pickupRef.interactorRefLeft.hasSelection;
        bool rightHandHasSelection = rightControllerRef.GetComponent<XRDirectInteractor>().hasSelection || pickupRef.interactorRefRight.hasSelection;

        bool firstLeftCheck;
        bool secondLeftCheck;
        bool firstRightCheck;
        bool secondRightCheck;

        try
        {
            firstLeftCheck = (givenCollider.gameObject.name.Equals((pickupRef.interactorRefLeft.interactablesSelected[0] as XRGrabInteractable).name));
        }
        catch
        {
            firstLeftCheck = false;
        }

        try
        {
            secondLeftCheck = (givenCollider.gameObject.name.Equals((leftControllerRef.GetComponent<XRDirectInteractor>().interactablesSelected[0] as XRGrabInteractable).name));
        }
        catch
        {
            secondLeftCheck = false;
        }

        try
        {
            firstRightCheck = (givenCollider.gameObject.name.Equals((pickupRef.interactorRefRight.interactablesSelected[0] as XRGrabInteractable).name));
        }
        catch
        {
            firstRightCheck = false;
        }

        try
        {
            secondRightCheck = (givenCollider.gameObject.name.Equals((rightControllerRef.GetComponent<XRDirectInteractor>().interactablesSelected[0] as XRGrabInteractable).name));
        }
        catch
        {
            secondRightCheck = false;
        }

        if (leftHandHasSelection && (givenCollider.gameObject.name.Equals(neededObject.name)) && (firstLeftCheck || secondLeftCheck))
        { 
            pickupRef.ForceDeselect(pickupRef.interactorRefLeft);
            pickupRef.ForceDeselect(leftControllerRef.GetComponent<XRDirectInteractor>());
        }

        if (rightHandHasSelection && (givenCollider.gameObject.name.Equals(neededObject.name)) && (firstRightCheck || secondRightCheck))
        {
            pickupRef.ForceDeselect(pickupRef.interactorRefRight);
            pickupRef.ForceDeselect(rightControllerRef.GetComponent<XRDirectInteractor>());
        }
    }
}
