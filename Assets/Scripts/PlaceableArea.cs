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

    // Ten skrypt przewidywany jest dla stref, w których nale¿y bêdzie od³o¿yæ przedmiot na miejsce (np: klucz w drzwiach, brakuj¹cy element uk³adanki, puste miejsce na szafce itp)
    // Je¿eli przedmiot zosta³ rzucony luŸno, pomijamy pierwszy cz³on warunkowy, odk³adamy go na miejsce a nastêpnie nakazujemy mu wykonaæ jak¹œ akcjê
    // Je¿eli przedmiot jest podsuniêty przez aktywnie trzymaj¹c¹ go rêke, na pocz¹tku trzeba go jeszcze z tej rêki wytr¹ciæ
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

    // Od³ó¿ przedmiot na miejsce, nastêpnie (opcjonalnie) wykonaj akcjê
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

        // Podobnie jak w RemotePickupBehaviour, nie ka¿dy z tych checków jest zawsze potrzebny, a ka¿dy sypnie b³êdem je¿eli nie bêdzie siê zgadza³, dlatego robimy tu œcianê try/catchy
        // Ten kawa³ek kodu sprawdza, czy w którymkolwiek z dostêpnych interactorów jest obecnie z³apany przedmiot, a dostêpne s¹: Lewa rêka promieñ, lewa rêka bezpoœrednio, prawa rêka promieñ oraz prawa rêka bezpoœrednio
        // Obiekty s¹ sprawdzane po nazwie, a nie przy pomocy GameObject.referenceEquals poniewa¿ 1) kostka na ziemii jest typu GameObject a z³apana XRGrabInteractable, a 2) z³apana kostka ma jeszcze doczepiane dziecko z DynamicAttacha i te sprawdzenia nie przechodz¹
        // Prawdopodobnie da³oby siê to omin¹æ gdyby da³o siê przerzutowaæ XRGrabInteractable > GameObject, ale nie znalaz³em takiego rozwi¹zania w internecie

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

        // Je¿eli potrzebny obiekt jest trzymany przez któr¹kolwiek z r¹k, wyrzuæ go z aktywnego zaznaczenia przez interactor (inaczej nie mo¿na zmieniæ mu pozycji)

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
