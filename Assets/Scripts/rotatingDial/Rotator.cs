using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Rotator : MonoBehaviour {
    [SerializeField] Transform linkedDial;
    [SerializeField] public int snapRotationAmount = 25;
    [SerializeField] private float angleTolerance;
    [SerializeField] private GameObject RightHandModel;
    [SerializeField] private GameObject LeftHandModel;
    [SerializeField] bool shouldUseDummyHands;

    public UnityEvent<float> onDialChange;
    private XRBaseInteractor interactor;
    private float startAngle;
    private bool requiresStartAngle = true;
    private bool shouldGetHandRotation = false;

    public bool initialising = true;
    public float previousDialValue = 0;

    private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

    private void Start() {
        if (onDialChange == null) onDialChange = new UnityEvent<float>();
    }

    private void OnEnable() {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabEnd);
    }

    private void OnDisable() {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabEnd);
    }

    private void GrabEnd(SelectExitEventArgs arg0) {
        shouldGetHandRotation = false;
        requiresStartAngle = true;
        HandModelVisibility(false);
    }

    private void GrabbedBy(SelectEnterEventArgs arg0) {
        interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
        interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;

        shouldGetHandRotation = true;
        startAngle = 0f;

        HandModelVisibility(true);
    }

    private void HandModelVisibility(bool visibilityState) {
        if (!shouldUseDummyHands) return;
        if (interactor.CompareTag("RightHand")) RightHandModel.SetActive(visibilityState);
        else LeftHandModel.SetActive(visibilityState);
        // to s¹ tylko wizualia
    }
    void Update() {
        if (shouldGetHandRotation) {
            var rotationAngle = GetInteractorRotation(); //gets the current controller angle
            GetRotationDistance(rotationAngle);
        }
    }

    public float GetInteractorRotation() => interactor.GetComponent<Transform>().eulerAngles.z;

    private void GetRotationDistance(float currentAngle) {
        if (!requiresStartAngle) {
            var angleDifference = Mathf.Abs(startAngle - currentAngle);
            if (angleDifference > angleTolerance) {

                if (angleDifference > 270f) { //checking to see if the user has gone from 0 -> 360

                    float angleCheck;
                    if (startAngle < currentAngle) {
                        angleCheck = CheckAngle(currentAngle, startAngle);

                        if (angleCheck < angleTolerance) return;
                        else {
                            RotateDialClockwise();
                            startAngle = currentAngle;
                        }
                    }
                    else if (startAngle > currentAngle) {
                        angleCheck = CheckAngle(currentAngle, startAngle);
                        if (angleCheck < angleTolerance) return;
                        else {
                            RotateDialAntiClockwise();
                            startAngle = currentAngle;
                        }
                    }
                }
                else {


                    if (startAngle < currentAngle) {
                        RotateDialAntiClockwise();
                        startAngle = currentAngle;
                    }
                    else if (startAngle > currentAngle) {
                        RotateDialClockwise();
                        startAngle = currentAngle;
                    }
                }

            }
        }
        else {
            requiresStartAngle = false;
            startAngle = currentAngle;
        }
    }
    private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;

    private void RotateDialClockwise() {
        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y, linkedDial.localEulerAngles.z + snapRotationAmount);
        DialChanged(linkedDial.localEulerAngles.z);
    }

    private void RotateDialAntiClockwise() {
        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y, linkedDial.localEulerAngles.z - snapRotationAmount);
        DialChanged(linkedDial.localEulerAngles.z);
    }

    public void DialChanged(float dialValue) {
        float dialDifference = dialValue - previousDialValue;
        if (!initialising) previousDialValue = dialValue;
        if (initialising) {
            Debug.Log("Initializing");
            initialising = false;
        }
        if (dialDifference > 0 && !(dialDifference > (360 - snapRotationAmount - 1)) || dialDifference < (-360 + snapRotationAmount + 1)) {
            onDialChange.Invoke(1);
        }
        if (dialDifference < 0 && !(dialDifference < (-360 + snapRotationAmount + 1)) || dialDifference > (360 - snapRotationAmount - 1)) {
            onDialChange.Invoke(-1);
        }
        //Debug.Log($"percentage power {Mathf.Ceil(currentDialValue)}");
    }
}
