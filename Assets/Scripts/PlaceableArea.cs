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
    [SerializeField] private enum InteractionType { Stay = 1, Jump = 2, Spin = 3 }
    [SerializeField] private InteractionType chosenInteraction;

    private void OnTriggerEnter(Collider enteredCollider)
    {
        Debug.Log(enteredCollider.gameObject.name);
        if(GameObject.ReferenceEquals(enteredCollider.gameObject, neededObject))
        {
            if(!mustBeDropped)
            {
                try
                {
                    //1. Trzeba bedzie przestawiæ nie na praw¹ ³apê tylko na obie
                    //2. Zrobiæ to samo ale dla zwyk³ego interactora, to jest tylko dla dystansowego
                    RemotePickupBehaviour.Instance.ForceDeselect(RemotePickupBehaviour.Instance.interactorRefRight);
                }
                catch
                {
                    Debug.Log("co xd");
                }
                PlaceInArea(enteredCollider.gameObject);
                return;
            }


            //To chyba nie bedzie potrzebne

            //if(GameObject.ReferenceEquals(RemotePickupBehaviour.Instance.grabbedObject, neededObject))
            //{
            //    return;
            //}

            PlaceInArea(enteredCollider.gameObject);
        }
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
}
