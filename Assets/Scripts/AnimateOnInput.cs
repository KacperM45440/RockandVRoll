using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateOnInput : MonoBehaviour
{
    public Animator handAnimator;
    [SerializeField] private InputActionProperty grabAnimation;

    private void OnEnable()
    {
        grabAnimation.action.performed += GrabStarted;
        grabAnimation.action.canceled += GrabCanceled;
        grabAnimation.action.Enable();
    }
    private void OnDisable()
    {
        grabAnimation.action.performed -= GrabStarted;
        grabAnimation.action.canceled -= GrabCanceled;
        grabAnimation.action.Disable();
    }
    public void GrabStarted(InputAction.CallbackContext obj)
    {
        float triggerValue = grabAnimation.action.ReadValue<float>();
        handAnimator.SetFloat("grabTrigger", triggerValue);
    }

    public void GrabCanceled(InputAction.CallbackContext obj)
    {
        float triggerValue = grabAnimation.action.ReadValue<float>();
        handAnimator.SetFloat("grabTrigger", triggerValue);
    }
}
