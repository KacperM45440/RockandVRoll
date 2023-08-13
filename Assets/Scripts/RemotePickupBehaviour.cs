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
    public HandPhysics physicsRefLeft;
    public HandPhysics physicsRefRight;
    [SerializeField] private float gripSensitivity = 0.3f;

    public GameObject controllerLeft;
    public GameObject controllerRight;

    // Poniewa¿ jest to gra jednoosobowa i zestaw kontrolerów jest zawsze jeden, to z mo¿emy zrobiæ z managera interakcji singleton
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
        CheckForSelection();
    }

    // Podczas pracy bezpoœredniego interactora, wy³¹czamy interakcjê promieniem aby nie da³o siê ruszaæ rêk¹ dwóch obiektów naraz (b¹dŸ bugowaæ te obecnie z³apane)
    public void CheckForSelection()
    {
        if (controllerLeft.GetComponent<XRDirectInteractor>().hasSelection)
        {
            interactorRefLeft.enabled = false;
            interactorRefLeft.gameObject.GetComponent<XRInteractorLineVisual>().enabled = false;
        }
        else if (!controllerLeft.GetComponent<XRDirectInteractor>().hasSelection)
        {
            interactorRefLeft.enabled = true;
            interactorRefLeft.gameObject.GetComponent<XRInteractorLineVisual>().enabled = true;
        }

        if (controllerRight.GetComponent<XRDirectInteractor>().hasSelection)
        {
            interactorRefRight.enabled = false;
            interactorRefRight.gameObject.GetComponent<XRInteractorLineVisual>().enabled = false;
        }
        else if (!controllerRight.GetComponent<XRDirectInteractor>().hasSelection)
        {
            interactorRefRight.enabled = true;
            interactorRefRight.gameObject.GetComponent<XRInteractorLineVisual>().enabled = true;
        }
    }

    // SprawdŸ, czy klikane s¹ obecnie jakiekolwiek guziki
    // Teoretycznie lepiej by by³o zrobiæ jakiœ event przy guzikach który wywyo³a funkcjê ni¿ wciskaæ to w update, ale póki co jest git
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
            physicsRefRight.DisableColliders();
            RecallObject(controllerRight, interactorRefRight);
        }
        else if (!gripPressedRight)
        {
            ReleaseObject(interactorRefRight);
        }

        if (gripPressedLeft)
        {
            physicsRefLeft.DisableColliders();
            RecallObject(controllerLeft, interactorRefLeft);
        }
        else if (!gripPressedLeft)
        {
            ReleaseObject(interactorRefLeft);
        }
    }

    // Przywróæ zaznaczony (promieniem) przedmiot do rêki
    public void RecallObject(GameObject currentController, XRRayInteractor currentInteractor)
    {
        if (gripPressedRight && currentController == controllerRight || gripPressedLeft && currentController == controllerLeft)
        {
            isRecalled = true;

            try
            {
                // Funkcjê mo¿na wywo³aæ bez trzymania jakiegokolwiek przedmiotu i mo¿e ona sypn¹æ b³êdem, w przypadku takiego wyj¹tku po prostu z niej wychodzimy 
                grabbedObject = currentInteractor.interactablesSelected[0] as XRGrabInteractable;
            }
            catch 
            {
                isRecalled = false;
                return;
            }

            // Nie trzeba kombinowaæ z rêcznym przenoszeniem obiektu do rêki zmian¹ pozycji poniewa¿ XR Toolkit daje nam ju¿ tak¹ funckjê
            // Jedyne co trzeba zrobiæ to j¹ w³¹czyæ podczas "zrestartowania" zaznaczenia
            ForceDeselect(currentInteractor);
            currentInteractor.useForceGrab = true;
            ForceSelect(currentInteractor,grabbedObject);
        }
        StartCoroutine(WaitForRelease(currentInteractor));
    }

    // Koñcz¹c interakcjê wy³¹czamy ForceGrab aby nastêpny podniesiony przedmiot nie by³ natychmiastowo z³apany do d³oni
    public void ReleaseObject(XRRayInteractor currentInteractorRef)
    {
        currentInteractorRef.useForceGrab = false;
        grabbedObject = null;
    }

    // Wymuœ wypuszczenie obiektu przez interactor (tak, jak przy puszczeniu triggera)
    public void ForceDeselect(XRBaseInteractor interactor)
    {
        gameObject.GetComponent<CustomInteractionManager>().ForceDeselect(interactor);
    }

    // Wymuœ zaznaczenie obiektu przez interactor (tak, jakby rêcznie klikniêto trigger)
    public void ForceSelect(XRBaseInteractor interactor, IXRSelectInteractable interactable)
    {
        gameObject.GetComponent<CustomInteractionManager>().SelectEnter(interactor, interactable);
    }

    // OpóŸniamy zmianê zmiennej aby funkcja CheckInput() nie próbowa³a zbyt szybko egzekwowaæ swoich czêœci kodu
    // W przeciwnym wypadku kostka driftuje po promieniu tak d³ugo, jak wciœniêty jest grip
    IEnumerator WaitForRelease(XRRayInteractor currentInteractorRef)
    {
        yield return new WaitForSeconds(0.1f);
        isRecalled = false;

        if (currentInteractorRef.Equals(interactorRefRight))
        {
            physicsRefRight.Delay(0.2f);
        }

        if (currentInteractorRef.Equals(interactorRefLeft))
        {
            physicsRefLeft.Delay(0.2f);
        }
    }
}



