using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float triggerDistance;
    public float interval;
    public float jumpVelocity;

    public Transform leftHand;
    public Transform rightHand;
    public Rigidbody rb;
    public GroundDetector groundDetector;

    private float nextTriggerTime;
    private Vector3 previousLeft = Vector3.zero;
    private Vector3 previousRight = Vector3.zero;

    private void Start()
    {

    }
    void Update()
    {
        if (nextTriggerTime > 0)
        {
            nextTriggerTime -= Time.deltaTime;
        }
        else
        {
            tryJump();
            nextTriggerTime = interval;
        }
    }

    private void tryJump()
    {
        Vector3 leftMove = leftHand.position - previousLeft;
        Vector3 rightMove = rightHand.position - previousRight;

        if (leftMove.y > triggerDistance && rightMove.y > triggerDistance && groundDetector.isGrounded)
        {
            Jump();
        }
        previousLeft = leftHand.position;
        previousRight = rightHand.position;
    }
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
    }
}
