using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateOnInput : MonoBehaviour
{
    public Animator handAnimator;
    public RemotePickupBehaviour remoteRef;
    [SerializeField] private InputActionProperty grabAnimation;

    private void OnEnable()
    {
        grabAnimation.action.performed += RemoteStarted;
        grabAnimation.action.canceled += RemoteCanceled;
        grabAnimation.action.Enable();
    }
    private void OnDisable()
    {
        grabAnimation.action.performed -= RemoteStarted;
        grabAnimation.action.canceled -= RemoteCanceled;
        grabAnimation.action.Disable();
    }
    public void RemoteStarted(InputAction.CallbackContext obj)
    {
        //float triggerValue = grabAnimation.action.ReadValue<float>();
        StartCoroutine(Delay());
    }

    public void RemoteCanceled(InputAction.CallbackContext obj)
    {
        float triggerValue = grabAnimation.action.ReadValue<float>();
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        try
        {
            if (!remoteRef.interactorRefRight.hasSelection)
            {
                remoteRef.rightHandAnimator.SetTrigger("remoteCatch");
            }

            if (!remoteRef.interactorRefLeft.hasSelection)
            {
                remoteRef.leftHandAnimator.SetTrigger("remoteCatch");
            }
        }
        catch
        {
            Debug.Log("nie ma xd");
        }
    }
}
