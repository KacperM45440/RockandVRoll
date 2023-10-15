using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectCheck : MonoBehaviour
{
    public XRDirectInteractor directLeftRef;
    public XRDirectInteractor directRightRef;

    public HandPhysics physicsLeftRef;
    public HandPhysics physicsRightRef;

    public ObjectSoundsSO objectSoundsRef;
    private bool canPlaySound = false;
    private XRDirectInteractor lastInteractor = null;

    public void CheckHand()
    {
        if (directLeftRef.isSelectActive)
        {
            physicsLeftRef.DisableColliders();
            lastInteractor = directLeftRef;
        }

        if (directRightRef.isSelectActive)
        {
            physicsRightRef.DisableColliders();
            lastInteractor = directRightRef;
        }
    }

    public void ObjectDropped()
    {
        if (lastInteractor == null)
        {
            return;
        }

        if (lastInteractor.Equals(directLeftRef))
        {
            physicsLeftRef.Delay(0.333f);
            lastInteractor = null;
            return;
        }

        if (lastInteractor.Equals(directRightRef))
        {
            physicsRightRef.Delay(0.333f);
            lastInteractor = null;
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canPlaySound)
        {
            canPlaySound = false;
            objectSoundsRef.PickedUp(GetComponent<AudioSource>());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        canPlaySound = true;
    }
}
