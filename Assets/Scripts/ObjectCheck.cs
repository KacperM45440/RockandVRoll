using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectCheck : MonoBehaviour
{
    public XRDirectInteractor directLeftRef;
    public XRDirectInteractor directRightRef;
    private XRDirectInteractor lastInteractor = null;

    public HandPhysics physicsLeftRef;
    public HandPhysics physicsRightRef;
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
        Debug.Log(lastInteractor);

        if (lastInteractor == null)
        {
            return;
        }

        if (lastInteractor.Equals(directLeftRef))
        {
            physicsLeftRef.Delay(0.2f);
            lastInteractor = null;
            return;
        }

        if (lastInteractor.Equals(directRightRef))
        {
            physicsRightRef.Delay(0.2f);
            lastInteractor = null;
            return;
        }
    }
}
