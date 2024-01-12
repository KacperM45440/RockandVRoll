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

        bodyRef.velocity = (newPosition - currentPosition) / Time.deltaTime;
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
