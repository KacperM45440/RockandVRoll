using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Rotator : MonoBehaviour
{
    [SerializeField] Transform linkedDial;
    [SerializeField] public int snapRotationAmount = 25;
    [SerializeField] private float angleTolerance;
    [SerializeField] private GameObject RightHandModel;
    [SerializeField] private GameObject LeftHandModel;
    [SerializeField] bool shouldUseDummyHands;

    [SerializeField] private GameObject PhysicalRightHand;
    [SerializeField] private GameObject PhysicalLeftHand;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject LeftHand;

    public UnityEvent<float> onDialChange;
    private XRBaseInteractor interactor;
    private Transform interactorTransform;

    public bool initialising = true;
    float startAngle;

    private bool isInteracting = false;

    private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

    private void Start()
    {
        if (onDialChange == null)
        {
            onDialChange = new UnityEvent<float>();
        }
    }

    private void OnEnable()
    {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabEnd);
    }

    private void OnDisable()
    {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabEnd);
    }

    private void GrabEnd(SelectExitEventArgs arg0)
    {
        isInteracting = false;
        HandModelVisibility(false);
    }

    private void GrabbedBy(SelectEnterEventArgs arg0)
    {
        interactor = GetComponent<XRGrabInteractable>().interactorsSelecting[0] as XRBaseInteractor;
        interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;
        
        interactorTransform = interactor.GetComponent<Transform>();
        isInteracting = true;
        startAngle = GetInteractorRotation();

        HandModelVisibility(true);
        
    }

    private void HandModelVisibility(bool visibilityState)
    {
        // wizualia
        if (!shouldUseDummyHands)
        {
            return;
        }
        if (interactor.CompareTag("RightHand"))
        {
            RightHandModel.SetActive(visibilityState);
            NonDialHandVisible(!visibilityState, true);
        }
        else if (interactor.CompareTag("LeftHand"))
        {
            LeftHandModel.SetActive(visibilityState);
            NonDialHandVisible(!visibilityState, false);
        }
    }
    public float GetInteractorRotation()
    {
        return interactorTransform.eulerAngles.z;
    }

    void Update()
    {
        if (isInteracting)
        {
            HandleRotation();
        }
    }

    private bool IsEdgeCase(float turnAngle)
    {
        return Mathf.Abs(turnAngle) > 360f - (1.1 * angleTolerance);
    }
    private float GetTurnAngle(float currentAngle)
    {
        float turnAngle = currentAngle - startAngle;

        if (IsEdgeCase(turnAngle))
        {
            return Mathf.Sign(turnAngle) * 360 - turnAngle;
        }
        return -turnAngle;
    }
    private void HandleRotation()
    {
        float currentAngle = GetInteractorRotation();
        float rotationAngle = GetTurnAngle(currentAngle);
        if (Mathf.Abs(rotationAngle) > angleTolerance)
        {
            float turnDirection = Mathf.Sign(rotationAngle);
            RotateDial(turnDirection);
            onDialChange.Invoke(turnDirection);
            startAngle = currentAngle;
        }
    }

    private void RotateDial(float turnDirection)
    {
        //linkedDial.eulerAngles = new Vector3(
        //    linkedDial.localEulerAngles.x,
        //    linkedDial.localEulerAngles.y,
        //    linkedDial.localEulerAngles.z + (snapRotationAmount * turnDirection)
        //);

        linkedDial.localEulerAngles = new Vector3(
            0,
            0,
            linkedDial.localEulerAngles.z + (snapRotationAmount * turnDirection)
);

    }

    private void NonDialHandVisible(bool isVisibile, bool isRight) {
        if (isRight)
        {
            PhysicalRightHand.SetActive(isVisibile);
            RightHand.SetActive(isVisibile);
        }
        else {
            PhysicalLeftHand.SetActive(isVisibile);
            LeftHand.SetActive(isVisibile);
        }
    }
}