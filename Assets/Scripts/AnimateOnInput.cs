using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateOnInput : MonoBehaviour
{
    public Animator handAnimator;
    public InputActionProperty grabAnimation;
    // Update is called once per frame
    void Update()
    {
        float triggerValue = grabAnimation.action.ReadValue<float>();
        handAnimator.SetFloat("grabTrigger", triggerValue);
        Debug.Log(triggerValue);
    }
}
