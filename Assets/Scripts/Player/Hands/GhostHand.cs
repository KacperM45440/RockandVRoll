using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GhostHand : MonoBehaviour
{
    private HandPhysics physicsRef;
    private Transform target;
    private float handDistance;
    private Renderer ghostedHand;

    private void Start()
    {
        physicsRef = GetComponent<HandPhysics>();
        target = physicsRef.target;
        handDistance = physicsRef.handDistance;
        ghostedHand = physicsRef.ghostedHand;
    }
    void FixedUpdate()
    {
        DisplayGhostHand();
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

        if (physicsRef.enabled)
        {
            transform.rotation = target.rotation;
        }
    }
}
