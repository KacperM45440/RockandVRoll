using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundDetector : MonoBehaviour
{
    public bool isGrounded { get; private set; }

    public UnityEvent OnGroundTouched;
    public UnityEvent OnGroundLeft;

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
        OnGroundTouched.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
        OnGroundLeft.Invoke();
        StartCoroutine(RestartGround());
    }

    private IEnumerator RestartGround()
    {
        float counter = 0;
        float waitTime = 2;
        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            if (isGrounded)
            {
                yield break;
            }
            yield return null;
        }

        isGrounded = true;
    }
}
