using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Ten skrypt mial byc odpowiedzialny za wyzerowanie srodka masy obiektu, co mialo pomoc naprawic niektore problemy z interakcja i zaczepieniem
// Nie wniosl on nic, ale zostaje jako przyklad uzycia listenerow na scenie

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
