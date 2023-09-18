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
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;


    private static RemotePickupBehaviour _instance;
    public static RemotePickupBehaviour Instance { get { return _instance; } }

    private bool isRecalled = false;
    private bool gripPressedRight = false;
    private bool gripPressedLeft = false;
    private bool triggerPressedLeft = false;
    private bool triggerPressedRight = false;
    private RotationKeeper rotationRef;
    public XRGrabInteractable grabbedObject;
    public XRRayInteractor usedInteractor;
    public XRRayInteractor interactorRefLeft;
    public XRRayInteractor interactorRefRight;
    public HandPhysics physicsRefLeft;
    public HandPhysics physicsRefRight;
    [SerializeField] private float buttonSensitivity = 0.25f;

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

    // Przywróæ zaznaczony (promieniem) przedmiot do rêki
    public void RecallObject(GameObject currentController, XRRayInteractor currentInteractor)
    {
        if (isRecalled)
        {
            return;
        }

        if ((triggerPressedRight && currentController == controllerRight) || (triggerPressedLeft && currentController == controllerLeft))
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

            if (triggerPressedRight)
            {
                physicsRefRight.DisableColliders();
            }

            if (triggerPressedLeft)
            {
                physicsRefLeft.DisableColliders();
            }

            // Nie trzeba kombinowaæ z rêcznym przenoszeniem obiektu do rêki zmian¹ pozycji poniewa¿ XR Toolkit daje nam ju¿ tak¹ funckjê
            // Jedyne co trzeba zrobiæ to j¹ w³¹czyæ podczas "zrestartowania" zaznaczenia

            PrepareRotation(currentInteractor);
            DisableCollisionsInObject();
            ForceDeselect(currentInteractor);

            currentInteractor.useForceGrab = true;
            ForceSelect(currentInteractor, grabbedObject);
            StartCoroutine(EnableCollisionsInObject(currentInteractor));
        }
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

        gripPressedRight = Input.GetAxis("XRI_Right_Grip") > buttonSensitivity;
        gripPressedLeft = Input.GetAxis("XRI_Left_Grip") > buttonSensitivity;
        triggerPressedRight = Input.GetAxis("XRI_Right_Trigger") > buttonSensitivity;
        triggerPressedLeft = Input.GetAxis("XRI_Left_Trigger") > buttonSensitivity;

        if (triggerPressedRight)
        {
            RecallObject(controllerRight, interactorRefRight); //merge into one
            rightHandAnimator.SetBool("grabbing", true);
        }

        if (triggerPressedLeft)
        {
            RecallObject(controllerLeft, interactorRefLeft);
            leftHandAnimator.SetBool("grabbing", true);
        }
        
        if (!triggerPressedRight) 
        {
            rightHandAnimator.SetBool("telekinesis", false);
        }
        
        if (!triggerPressedLeft)
        {
            leftHandAnimator.SetBool("telekinesis", false);
        }
    }

    private void DisableCollisionsInObject()
    {
        try
        {
            foreach (Transform child in grabbedObject.transform.GetChild(0))
            {
                child.gameObject.SetActive(false);
            }
        }
        catch { }
    }
    private IEnumerator EnableCollisionsInObject(XRRayInteractor currentInteractor)
    {
        yield return new WaitForSeconds(0.1f);
        try
        {
            foreach (Transform child in grabbedObject.transform.GetChild(0))
            {
                child.gameObject.SetActive(true);
            }
        }
        catch { }

        StartCoroutine(WaitForRelease(currentInteractor));
        StartCoroutine(WaitForObject(currentInteractor));
    }
    private void PrepareRotation(XRRayInteractor currentInteractor)
    {
        rotationRef = grabbedObject.GetComponent<RotationKeeper>();
        rotationRef.preferredRotation.x *= IsRightInteractFloat(currentInteractor);
        rotationRef.preferredRotation.y *= IsRightInteractFloat(currentInteractor);
        rotationRef.preferredRotation.z *= IsRightInteractFloat(currentInteractor);
        rotationRef.SetRotation(grabbedObject.gameObject, HandFromRay(currentInteractor));
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

        try
        {
            grabbedObject = interactor.interactablesSelected[0] as XRGrabInteractable;
        }
        catch
        {
            isRecalled = false;
        }
    }

    // OpóŸniamy zmianê zmiennej aby funkcja CheckInput() nie próbowa³a zbyt szybko egzekwowaæ swoich czêœci kodu
    // W przeciwnym wypadku kostka driftuje po promieniu tak d³ugo, jak wciœniêty jest grip
    IEnumerator WaitForRelease(XRRayInteractor interactor)
    {
        yield return new WaitForSeconds(0.1f);
        isRecalled = false;

        if (IsRightInteractBool(interactor))
        {
            physicsRefRight.Delay(0.333f);
        }
        else
        {
            physicsRefLeft.Delay(0.333f);
        }
    }

    IEnumerator WaitForObject(XRRayInteractor interactor)
    {
        if (IsRightInteractBool(interactor))
        {
            yield return new WaitUntil(() => (!triggerPressedRight || !gripPressedRight));
            ReleaseObject(interactorRefRight);
            //rightHandAnimator.SetBool("telekinesis", false);
            rightHandAnimator.SetBool("grabbing", false);
        }
        else
        {
            yield return new WaitUntil(() => (!triggerPressedLeft || !gripPressedLeft));
            ReleaseObject(interactorRefLeft);
            //leftHandAnimator.SetBool("telekinesis", false);
            leftHandAnimator.SetBool("grabbing", false);
        }
    }

    // Koñcz¹c interakcjê wy³¹czamy ForceGrab aby nastêpny podniesiony przedmiot nie by³ natychmiastowo z³apany do d³oni
    public void ReleaseObject(XRRayInteractor currentInteractorRef)
    {
        currentInteractorRef.useForceGrab = false;
        grabbedObject = null;
    }

    public bool IsRightInteractBool(XRRayInteractor currentInteractorRef)
    {
        if (currentInteractorRef.Equals(interactorRefRight))
        {
            return true;
        }

        if (currentInteractorRef.Equals(interactorRefLeft))
        {
            return false;
        }

        throw new Exception("Neither right nor left");
    }

    public float IsRightInteractFloat(XRRayInteractor currentInteractorRef)
    {
        if (currentInteractorRef.Equals(interactorRefRight))
        {
            return 1f;
        }

        if (currentInteractorRef.Equals(interactorRefLeft))
        {
            return -1f;
        }

        throw new Exception("Neither right nor left");
    }

    public GameObject HandFromRay(XRRayInteractor currentInteractorRef)
    {
        if (currentInteractorRef.Equals(interactorRefRight))
        {
            return physicsRefRight.gameObject;
        }

        if (currentInteractorRef.Equals(interactorRefLeft))
        {
            return physicsRefLeft.gameObject;
        }

        throw new Exception("Neither right nor left");
    }
}



