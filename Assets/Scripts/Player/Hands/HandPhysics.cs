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
    public Transform colliderTransform;
    private Collider[] handColliders;
    private float smoothSpeed = 1f;

    void Start()
    {
        bodyRef = GetComponent<Rigidbody>();
        handColliders = colliderTransform.GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        DisplayGhostHand();
    }

    void FixedUpdate()
    {
        FollowGhostHand();
    }

    // Jezeli pozycja kontrolera bedzie roznic sie zbyt bardzo od obecnej pozycji fizycznych dloni, pokaz przezroczyste dlonie w jego obecnej pozycji
    public void DisplayGhostHand()
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

        transform.rotation = target.rotation;
    }

    // Kaz fizycznym dloniom podazac za obecna pozycja kontrolera
    public void FollowGhostHand()
    {
        Vector3 targetPosition = target.position;
        Vector3 currentPosition = transform.position;

        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, smoothSpeed * Time.fixedDeltaTime);
        bodyRef.velocity = 120 * (newPosition - currentPosition) / Time.fixedDeltaTime;
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
