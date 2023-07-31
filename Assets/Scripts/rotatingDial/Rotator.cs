using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Rotator : MonoBehaviour
{
    [SerializeField] Transform linkedDial;
    [SerializeField] private int snapRotationAmount = 25;
    [SerializeField] private float angleTolerance;
    [SerializeField] private GameObject RightHandModel;
    [SerializeField] private GameObject LeftHandModel;
    [SerializeField] bool shouldUseDummyHands;

    private XRBaseInteractor interactor;
    private float startAngle;
    private bool requiresStartAngle = true;
    private bool shouldGetHandRotation = false;

    private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

    private void OnEnable() {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabEnd);
        //dzia³a OnEnable
    }

    private void OnDisable() {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabEnd);
        //nie wy³¹cza siê instantly sam
    }

    private void GrabEnd(SelectExitEventArgs arg0) {
        shouldGetHandRotation = false;
        requiresStartAngle = true;
        HandModelVisibility(false);
        Debug.Log("stopsGrabbing"); //nope
    }

    private void GrabbedBy(SelectEnterEventArgs arg0) {
        interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
        interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;

        shouldGetHandRotation = true;
        startAngle = 0f;

        HandModelVisibility(true);
        Debug.Log("triesGrabbing"); //nope
    }

    private void HandModelVisibility(bool visibilityState) {
        if (!shouldUseDummyHands) return;
        if (interactor.CompareTag("RightHand")) RightHandModel.SetActive(visibilityState);
        else LeftHandModel.SetActive(visibilityState);
        // to s¹ tylko wizualia
    }
    void Update()
    {
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
                    Debug.Log("isFrom0To360");

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
                    } else if (startAngle > currentAngle) {
                        RotateDialClockwise();
                        startAngle = currentAngle;
                    }
                }

            }
        } else {
            requiresStartAngle = false;
            startAngle = currentAngle;
        }
    }
    private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;

    private void RotateDialClockwise() {
        Debug.Log("triesTurningClockwise"); //nope
        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y, linkedDial.localEulerAngles.z + snapRotationAmount);
        if (TryGetComponent<IDial>(out IDial dial)) dial.DialChanged(linkedDial.localEulerAngles.z);
    }

    private void RotateDialAntiClockwise() {
        Debug.Log("triesTurningAntiClockwise"); //nope
        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y, linkedDial.localEulerAngles.z - snapRotationAmount);
        if (TryGetComponent<IDial>(out IDial dial)) dial.DialChanged(linkedDial.localEulerAngles.z);
    }
}
