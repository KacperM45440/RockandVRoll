using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotationKeeper : MonoBehaviour
{
    // Skrypt nie jest ju¿ odpowiedzialny za weryfikowanie obracania przedmiotu, natomiast nie ma sensu psuæ scen refaktoryzacj¹ dlatego zostaje z nazw¹ jaka jest
    // Je¿eli zostanie czas: zmieniæ nazwê na "ParentKeeper" albo "JointKeeper"

    public Vector3 preferredPosition;
    public Vector3 preferredRotation;
    public XRDirectInteractor directLeftRef;
    public XRDirectInteractor directRightRef;
    public Transform physicsLeftHand;
    public Transform physicsRightHand;
    public Transform handParentLeft;
    public Transform handParentRight;
    private Transform startParent;
    private bool leftSelecting;
    private bool rightSelecting;
    private List<FixedJoint> joints = new();

    private void Start()
    {
        startParent = transform.parent;
    }

    public void SetRotation(GameObject rotatedObject, GameObject givenHand)
    {
        rotatedObject.transform.eulerAngles = preferredRotation + givenHand.transform.eulerAngles;
    }

    public void SetPosition(XRRayInteractor currentInteractor, Transform givenAttach)
    {
        //givenAttach.localPosition = preferredPosition;
        givenAttach.position = preferredPosition;
        currentInteractor.attachTransform = givenAttach;
    }
    public void AssignParent()
    {
        CheckSelection();
        StartCoroutine(Assign());
    }

    public void CheckSelection()
    {
        leftSelecting = false;
        rightSelecting = false; 

        try
        {
            if (directLeftRef.interactablesSelected[0] != null && (directLeftRef.interactablesSelected[0] as XRGrabInteractable).name.Equals(gameObject.name))
            {
                leftSelecting = true;
            }
            else
            {
                leftSelecting = false;
            }
        }
        catch
        {
            leftSelecting = false;
        }

        try
        {
            if (directRightRef.interactablesSelected[0] != null && (directRightRef.interactablesSelected[0] as XRGrabInteractable).name.Equals(gameObject.name))
            {
                rightSelecting = true;
            }
            else
            {
                rightSelecting = false;
            }
        }
        catch
        {
            rightSelecting = false;
        }
    }

    public void ReturnParent()
    {
        transform.parent = startParent;
        foreach (FixedJoint joint in joints)
        {
            Destroy(joint);
        }
    }

    IEnumerator Assign()
    {
        yield return new WaitUntil(() => leftSelecting || rightSelecting);

        if (leftSelecting)
        {
            transform.parent = handParentLeft;
            StartCoroutine(CreateJoint());
        }

        if (rightSelecting)
        {
            transform.parent = handParentRight;
            StartCoroutine(CreateJoint());
        }
    }

    IEnumerator CreateJoint()
    {
        yield return new WaitUntil(() => leftSelecting || rightSelecting);

        if (leftSelecting)
        {
            FixedJoint newJoint = gameObject.AddComponent<FixedJoint>();
            newJoint.connectedBody = physicsLeftHand.GetComponent<Rigidbody>();
            joints.Add(newJoint);
        }

        if (rightSelecting)
        {
            FixedJoint newJoint = gameObject.AddComponent<FixedJoint>();
            newJoint.connectedBody = physicsRightHand.GetComponent<Rigidbody>();
            joints.Add(newJoint);
        }
    }
}
