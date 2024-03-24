using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachStack : MonoBehaviour
{
    private Transform parentRef;
    private void Start()
    {
        parentRef = transform.parent;
    }
    private void OnCollisionEnter(Collision collision)
    {
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
}
