using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Rotator : MonoBehaviour
{
    [SerializeField] Transform linkedDial;
    [SerializeField] public int snapRotationAmount = 25;
    [SerializeField] private float angleTolerance;

    [SerializeField] private GameObject PhysicalRightHand;
    [SerializeField] private GameObject PhysicalLeftHand;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject LeftHand;

    private XRBaseInteractor interactor;
    private Transform interactorTransform;

    public bool initialising = true;
    float startAngle;

    private bool isInteracting = false;

    private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

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
    }

    private void GrabbedBy(SelectEnterEventArgs arg0)
    {
        interactor = GetComponent<XRGrabInteractable>().interactorsSelecting[0] as XRBaseInteractor;
        interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;
        
        interactorTransform = interactor.GetComponent<Transform>();
        isInteracting = true;
        startAngle = GetInteractorRotation();
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

        linkedDial.localEulerAngles = new Vector3
        (
            0,
            0,
            linkedDial.localEulerAngles.z + (snapRotationAmount * turnDirection)
        );
    }
}