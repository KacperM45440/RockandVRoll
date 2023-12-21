using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Linq;
using UnityEngine.InputSystem;

public class RemotePickupBehaviour : XRBaseInteractor
{
    public Animator leftHandAnimator;
    public Animator rightHandAnimator;
    public Animator leftGhostAnimator;
    public Animator rightGhostAnimator;

    private static RemotePickupBehaviour _instance;
    public static RemotePickupBehaviour Instance { get { return _instance; } }

    private bool canLeave = true;
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
    [SerializeField] private InputAction leftGripInput;
    [SerializeField] private InputAction rightGripInput;
    [SerializeField] private InputAction leftTriggerInput;
    [SerializeField] private InputAction rightTriggerInput;
    [SerializeField] private float buttonSensitivity = 0.25f;

    public GameObject controllerLeft;
    public GameObject controllerRight;

    // Poniewaz jest to gra jednoosobowa i zestaw kontrolerów jest zawsze jeden, to z mozemy zrobic z managera interakcji singleton
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

    private void OnEnable()
    {
        // Enable the action when the script is enabled
        leftGripInput.Enable();
        rightGripInput.Enable();
        leftTriggerInput.Enable();
        rightTriggerInput.Enable();
    }

    private void OnDisable()
    {
        // Disable the action when the script is disabled
        leftGripInput.Disable();
        rightGripInput.Disable();
        leftTriggerInput.Disable();
        rightTriggerInput.Disable();
    }

    public void Update()
    {
        CheckForInput();
        CheckForSelection();
    }

    // Przywroc zaznaczony (promieniem) przedmiot do reki
    public void RecallObject(GameObject currentController, XRRayInteractor currentInteractor)
    {
        if (isRecalled)
        {
            return;
        }

        // Rozrozniamy pomiedzy lewa a prawa reka, poniewaz musimy znac ich obecne stany na potrzeby wiekszosci skryptow
        // Oznacza to potrzebe wyroznienia podobnych metod dla kazdej reki, nawet jezeli nie wyglada to elegancko 
        if ((triggerPressedRight && currentController == controllerRight) || (triggerPressedLeft && currentController == controllerLeft))
        {
            isRecalled = true;

            try
            {
                // Funkcje mozna wywolac bez trzymania jakiegokolwiek przedmiotu i moze ona sypnac bledem, w przypadku takiego wyjatku po prostu z niej wychodzimy 
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

            // Nie trzeba kombinowac z recznym przenoszeniem obiektu do reki zmianami pozycji poniewaz XR Toolkit daje nam juz taka funckje
            // Jedyne co trzeba zrobic to ja wlaczyc podczas "zrestartowania" zaznaczenia
            // Podczas tego procesu wylaczamy rowniez chwilowo kolizje w obiekcie, by obiekt nie utykal pomiedzy scianami lub innymi przeszkodami

            PrepareRotation(currentInteractor);
            DisableCollisionsInObject();
            canLeave = false;
            ForceDeselect(currentInteractor);


            currentInteractor.useForceGrab = true;
            ForceSelect(currentInteractor, grabbedObject);
            if(IsRightInteractBool(currentInteractor))
            {
                rightHandAnimator.SetTrigger("recallObject");
                rightGhostAnimator.SetTrigger("recallObject");
            }
            else
            {
                leftHandAnimator.SetTrigger("recallObject");
                leftGhostAnimator.SetTrigger("recallObject");
            }    
            StartCoroutine(EnableCollisionsInObject(currentInteractor));
        }
    }

    // Podczas pracy bezposredniego interactora, wylaczamy interakcje promieniem aby nie dalo sie ruszac reka dwoch obiektów naraz (beda bugowac te obecnie zlapane)
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

    // Sprawdz, czy klikane sa obecnie jakiekolwiek guziki
    
    // Teoretycznie lepiej by bylo zrobic jakis event przy guzikach który wywyola funkcje niz wciskac to w update, ale póki co jest git
    // Oznacza to rowniez koniecznosc drobnej poprawki przy logice animacji
    public void CheckForInput()
    {
        gripPressedRight = rightGripInput.ReadValue<float>() > buttonSensitivity;
        gripPressedLeft = leftGripInput.ReadValue<float>() > buttonSensitivity;
        triggerPressedRight = rightTriggerInput.ReadValue<float>() > buttonSensitivity;
        triggerPressedLeft = leftTriggerInput.ReadValue<float>() > buttonSensitivity;

        AnimationLogic();

        if (isRecalled)
        {
            return;
        }

        if (triggerPressedRight)
        {
            RecallObject(controllerRight, interactorRefRight); //merge into one
        }

        if (triggerPressedLeft)
        {
            RecallObject(controllerLeft, interactorRefLeft);
        }
    }

    public void AnimationLogic()
    {
        rightHandAnimator.SetFloat("grabRemote", rightGripInput.ReadValue<float>());
        leftHandAnimator.SetFloat("grabRemote", leftGripInput.ReadValue<float>());
        rightGhostAnimator.SetFloat("grabRemote", rightGripInput.ReadValue<float>());
        leftGhostAnimator.SetFloat("grabRemote", leftGripInput.ReadValue<float>());

        rightHandAnimator.SetFloat("grabDirect", rightTriggerInput.ReadValue<float>());
        leftHandAnimator.SetFloat("grabDirect", leftTriggerInput.ReadValue<float>());
        rightGhostAnimator.SetFloat("grabDirect", rightTriggerInput.ReadValue<float>());
        leftGhostAnimator.SetFloat("grabDirect", leftTriggerInput.ReadValue<float>());

        if (gripPressedRight && interactorRefRight.hasSelection)
        {
            rightHandAnimator.SetFloat("clawTime", Mathf.Lerp(rightHandAnimator.GetFloat("clawTime"), 1f, Time.deltaTime * 2f));
            rightHandAnimator.SetFloat("turnTime", Mathf.Lerp(rightHandAnimator.GetFloat("turnTime"), rightGripInput.ReadValue<float>(), Time.deltaTime * 4f));
            rightGhostAnimator.SetFloat("clawTime", Mathf.Lerp(rightGhostAnimator.GetFloat("clawTime"), 1f, Time.deltaTime * 2f));
            rightGhostAnimator.SetFloat("turnTime", Mathf.Lerp(rightGhostAnimator.GetFloat("turnTime"), rightGripInput.ReadValue<float>(), Time.deltaTime * 4f));
        }
        else
        {
            rightHandAnimator.SetFloat("clawTime", Mathf.Lerp(rightHandAnimator.GetFloat("clawTime"), 0, Time.deltaTime * 2f));
            rightHandAnimator.SetFloat("turnTime", Mathf.Lerp(rightHandAnimator.GetFloat("turnTime"), 0, Time.deltaTime * 4f));
            rightGhostAnimator.SetFloat("clawTime", Mathf.Lerp(rightGhostAnimator.GetFloat("clawTime"), 0, Time.deltaTime * 2f));
            rightGhostAnimator.SetFloat("turnTime", Mathf.Lerp(rightGhostAnimator.GetFloat("turnTime"), 0, Time.deltaTime * 4f));
        }

        if (gripPressedLeft && interactorRefLeft.hasSelection)
        {
            leftHandAnimator.SetFloat("clawTime", Mathf.Lerp(leftHandAnimator.GetFloat("clawTime"), 1f, Time.deltaTime * 2f));
            leftHandAnimator.SetFloat("turnTime", Mathf.Lerp(leftHandAnimator.GetFloat("turnTime"), leftGripInput.ReadValue<float>(), Time.deltaTime * 4f));
            leftGhostAnimator.SetFloat("clawTime", Mathf.Lerp(leftGhostAnimator.GetFloat("clawTime"), 1f, Time.deltaTime * 2f));
            leftGhostAnimator.SetFloat("turnTime", Mathf.Lerp(leftGhostAnimator.GetFloat("turnTime"), leftGripInput.ReadValue<float>(), Time.deltaTime * 4f));
        }
        else
        {
            leftHandAnimator.SetFloat("clawTime", Mathf.Lerp(leftHandAnimator.GetFloat("clawTime"), 0, Time.deltaTime * 2f));
            leftHandAnimator.SetFloat("turnTime", Mathf.Lerp(leftHandAnimator.GetFloat("turnTime"), 0, Time.deltaTime * 4f));
            leftGhostAnimator.SetFloat("clawTime", Mathf.Lerp(leftGhostAnimator.GetFloat("clawTime"), 0, Time.deltaTime * 2f));
            leftGhostAnimator.SetFloat("turnTime", Mathf.Lerp(leftGhostAnimator.GetFloat("turnTime"), 0, Time.deltaTime * 4f));
        }
    }

    // Zresetuj stan animacji do idle'owych w momencie gdy straci kontakt z trzymanym obiektem
    //public void LostFocus(XRRayInteractor currentInteractor)
    //{
    //    if (!canLeave)
    //    {
    //        return;
    //    }

    //    if (IsRightInteractBool(currentInteractor))
    //    {
    //        //rightHandAnimator.SetTrigger("lostFocus");
    //        rightHandAnimator.SetFloat("clawTime", Mathf.Lerp(rightHandAnimator.GetFloat("clawTime"), 0, Time.deltaTime * 2f));
    //        rightHandAnimator.SetFloat("turnTime", Mathf.Lerp(rightHandAnimator.GetFloat("turnTime"), 0, Time.deltaTime * 4f));
    //    }
    //    else
    //    {
    //        //leftHandAnimator.SetTrigger("lostFocus");
    //        leftHandAnimator.SetFloat("clawTime", Mathf.Lerp(leftHandAnimator.GetFloat("clawTime"), 0, Time.deltaTime * 2f));
    //        leftHandAnimator.SetFloat("turnTime", Mathf.Lerp(leftHandAnimator.GetFloat("turnTime"), 0, Time.deltaTime * 4f));
    //    }
    //}
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

        canLeave = true;

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
    
    // Ustaw obiekt tak, by zostal przywolany w korzystnej pozycji do zlapania go dlonia
    private void PrepareRotation(XRRayInteractor currentInteractor)
    {
        rotationRef = grabbedObject.GetComponent<RotationKeeper>();
        rotationRef.preferredRotation.x *= IsRightInteractFloat(currentInteractor);
        rotationRef.preferredRotation.y *= IsRightInteractFloat(currentInteractor);
        rotationRef.preferredRotation.z *= IsRightInteractFloat(currentInteractor);
        rotationRef.SetRotation(grabbedObject.gameObject, HandFromRay(currentInteractor));
    }

    // Wymus wypuszczenie obiektu przez interactor (tak, jak przy puszczeniu triggera)
    public void ForceDeselect(XRBaseInteractor interactor)
    {
        gameObject.GetComponent<CustomInteractionManager>().ForceDeselect(interactor);
    }

    // Wymus zaznaczenie obiektu przez interactor (tak, jakby recznie klikniêto trigger)
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

    // Opózniamy zmiane zmiennej aby funkcja CheckInput() nie próbowala zbyt szybko egzekwowac swoich czesci kodu
    // W przeciwnym wypadku kostka driftuje po promieniu tak dlugo, jak wcisniety jest grip
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
            //rightHandAnimator.SetBool("grabbing", false);
        }
        else
        {
            yield return new WaitUntil(() => (!triggerPressedLeft || !gripPressedLeft));
            ReleaseObject(interactorRefLeft);
            //leftHandAnimator.SetBool("grabbing", false);
        }
    }

    // Konczac interakcje wylaczamy ForceGrab aby nastepny podniesiony przedmiot nie byl natychmiastowo zlapany do dloni
    public void ReleaseObject(XRRayInteractor currentInteractorRef)
    {
        currentInteractorRef.useForceGrab = false;
        grabbedObject = null;
    }

    // Ponizsze funkcje pomagaja okreslic czy chodzi nam o lewa czy prawa reke, i zwracaja odpowiedni typ danych
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



