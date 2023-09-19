using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ObjectCheck))]
[RequireComponent(typeof(ObjectSounds))]
[RequireComponent(typeof(RotationKeeper))]
[RequireComponent(typeof(XRGrabInteractable))]

public class AutoGrabbable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ObjectCheck objectCheckReference = GetComponent<ObjectCheck>();
        objectCheckReference.directLeftRef ??= ObjectsReferenceManager.instance.directLeftRef;
        objectCheckReference.directRightRef ??= ObjectsReferenceManager.instance.directRightRef;

        objectCheckReference.physicsLeftRef ??= ObjectsReferenceManager.instance.physicsLeftRef;
        objectCheckReference.physicsRightRef ??= ObjectsReferenceManager.instance.physicsRightRef;


        RotationKeeper rotationKeeperReference = GetComponent<RotationKeeper>();
        rotationKeeperReference.physicsLeftHand ??= ObjectsReferenceManager.instance.physicsLeftHand;
        rotationKeeperReference.physicsRightHand ??= ObjectsReferenceManager.instance.physicsRightHand;

        rotationKeeperReference.handParentLeft ??= ObjectsReferenceManager.instance.handParentLeft;
        rotationKeeperReference.handParentRight ??= ObjectsReferenceManager.instance.handParentRight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
