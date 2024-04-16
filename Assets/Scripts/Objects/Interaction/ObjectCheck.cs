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
    private GameObject lastHand = null;
    private Transform parentRef;
    
    private void Start()
    {
        parentRef = transform.parent;
    }

    public void CheckHand()
    {
        if (directLeftRef.isSelectActive)
        {
            physicsLeftRef.DisableColliders();
            lastInteractor = directLeftRef;
            lastHand = physicsLeftRef.gameObject;
        }

        if (directRightRef.isSelectActive)
        {
            physicsRightRef.DisableColliders();
            lastInteractor = directRightRef;
            lastHand = physicsRightRef.gameObject;
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

    public void StickHandToObject()
    {
        lastHand.transform.parent = transform;
        lastHand.GetComponent<HandPhysics>().enabled = false;
        lastHand.GetComponent<Rigidbody>().useGravity = false;
        lastHand.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void UnstickHand()
    {
        lastHand.transform.parent = lastInteractor.transform.parent;
        lastHand.GetComponent<Rigidbody>().isKinematic = false;
        lastHand.GetComponent<Rigidbody>().useGravity = true;
        lastHand.GetComponent<HandPhysics>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canPlaySound)
        {
            StartCoroutine(StartSoundTimeout());
            objectSoundsRef.PickedUp(GetComponent<AudioSource>());
        }

        if (collision.rigidbody != null)
        {
            ConnectObject(collision.rigidbody);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        DetachObject();
    }

    private void DetachObject()
    {
        transform.parent = parentRef;
    }

    private void ConnectObject(Rigidbody givenBody)
    {
        transform.parent = givenBody.transform.parent;
    }

    private IEnumerator StartSoundTimeout()
    {
        canPlaySound = false;
        yield return new WaitForSeconds(0.5f);
        canPlaySound = true;
    }    
}
