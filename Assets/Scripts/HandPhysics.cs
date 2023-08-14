using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPhysics : MonoBehaviour
{
    public Transform target;
    private Rigidbody bodyRef;
    public Renderer ghostedHand;
    public float handDistance = 0.05f;
    private Collider[] handColliders;
    private float smoothSpeed = 100f;
    //private float angularSmoothSpeed = 200f;


    void Start()
    {
        bodyRef = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > handDistance)
        {
            ghostedHand.enabled = true;
        }
        else
        {
            ghostedHand.enabled = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position;
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, smoothSpeed * Time.fixedDeltaTime);
        bodyRef.velocity = (newPosition - currentPosition) / Time.fixedDeltaTime;

        //Quaternion targetRotation = target.rotation;
        //Quaternion currentRotation = transform.rotation;
        //Quaternion newRotation = Quaternion.Slerp(currentRotation, targetRotation, angularSmoothSpeed * Time.fixedDeltaTime);

        //Quaternion rotationDiff = newRotation * Quaternion.Inverse(currentRotation);
        //rotationDiff.ToAngleAxis(out float angle, out Vector3 axis);
        //Vector3 degreeDiff = angle * axis;
        //bodyRef.angularVelocity = (degreeDiff * Mathf.Deg2Rad / Time.fixedDeltaTime);

        transform.rotation = target.rotation;
    }

    public void Delay(float time)
    {
        Invoke(nameof(EnableColliders), time);
    }    

    public void EnableColliders()
    {
        foreach (Collider colliderChild in handColliders)
        {
            colliderChild.enabled = true;
        }
    }
    public void DisableColliders()
    {
        foreach (Collider colliderChild in handColliders)
        {
            colliderChild.enabled = false;
        }
    }

}
