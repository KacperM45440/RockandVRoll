using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MassKeeper : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable interactableRef;
    [SerializeField] private Rigidbody bodyRef;
    [SerializeField] private Vector3 newCenter;

    private void Start()
    {
        OnValidate();
        interactableRef.selectEntered.AddListener((X) => bodyRef.centerOfMass = newCenter);
        interactableRef.selectExited.AddListener((X) => bodyRef.ResetCenterOfMass());
    }

    private void OnValidate()
    {
        if(!interactableRef)
        {
            interactableRef = GetComponent<XRGrabInteractable>();
        }

        if(!bodyRef)
        {
            bodyRef = GetComponent<Rigidbody>();
        }
    }
}
