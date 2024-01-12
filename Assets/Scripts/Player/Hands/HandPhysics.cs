using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPhysics : MonoBehaviour
{
    public Transform target;
    private Rigidbody bodyRef;
    public Renderer ghostedHand;
    public float handDistance = 0.25f;
    public Transform colliderTransform;
    private Collider[] handColliders;

    //public float addForceStrength;
    //public float velocityStrength;

    public float toVel = 2.5f;
    public float maxVel = 15.0f;
    public float maxForce = 40.0f;
    public float gain = 5f;

    void Start()
    {
        bodyRef = GetComponent<Rigidbody>();
        handColliders = colliderTransform.GetComponentsInChildren<Collider>();
    }

    void FixedUpdate()
    {
        FollowGhostHand();
    }

    // Kaz fizycznym dloniom podazac za obecna pozycja kontrolera
    public void FollowGhostHand()
    {
        Vector3 targetPosition = target.position;
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, 1f);

        //bodyRef.velocity = (newPosition - currentPosition) / Time.deltaTime;

        //bodyRef.velocity = Vector3.ClampMagnitude(bodyRef.velocity, velocityStrength);

        Vector3 dist = targetPosition - transform.position;
        //dist.y = 0; // ignore height differences
                    // calc a target vel proportional to distance (clamped to maxVel)
        Vector3 tgtVel = Vector3.ClampMagnitude(toVel * dist, maxVel);
        // calculate the velocity error
        Vector3 error = tgtVel - bodyRef.velocity;
        // calc a force proportional to the error (clamped to maxForce)
        Vector3 force = Vector3.ClampMagnitude(gain * error, maxForce);
        bodyRef.AddForce(force);



        //if (Vector3.Distance(currentPosition, newPosition) < 0.1f)
        //{
        //    bodyRef.velocity *= 0.2f;
        //}
        //else
        //{
        //    bodyRef.AddForce(addForceStrength * Time.deltaTime * (newPosition - currentPosition), ForceMode.Impulse);
        //}
        
        
    }
    public void Delay(float time)
    {
        Invoke(nameof(EnableColliders), time);
    }    

    // Wlacz lub wylacz collidery w fizycznej rece
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
