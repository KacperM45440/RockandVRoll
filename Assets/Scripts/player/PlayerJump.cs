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

    private float nextTriggerTime = 1;
    private Vector3 previousLeft = Vector3.zero;
    private Vector3 previousRight = Vector3.zero;
    private bool initialised = false;
    private bool canJump = false;

    private void Start()
    {
        initialised = true;
    }
    void Update()
    {
        if (nextTriggerTime > 0)
        {
            nextTriggerTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (nextTriggerTime <= 0)
        {
            {
                TryJump();
                nextTriggerTime = interval;
            }
        }
    }

    private void TryJump()
    {
        Vector3 leftMove = leftHand.position - previousLeft;
        Vector3 rightMove = rightHand.position - previousRight;

        if (leftMove.y > triggerDistance && rightMove.y > triggerDistance && groundDetector.isGrounded && initialised)
        {
            Jump();
        }

        previousLeft = leftHand.position;
        previousRight = rightHand.position;
    }
    private void Jump()
    {
        if (!canJump)
        {
            canJump = true;
            return;
        }

        rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
    }
}
