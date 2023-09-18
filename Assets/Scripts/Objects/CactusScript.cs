using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CactusScript : MonoBehaviour
{
    public Haptic hapticFeedback;
    public Collider cactusCollider;
    public Transform physicalHandRight;
    public Transform physicalHandLeft;
    public Transform leftHaptic;
    public Transform rightHaptic;
    private void OnCollisionEnter(Collision collision)
    {
        if(!cactusCollider.bounds.Intersects(collision.collider.bounds))
        {
            return;
        }

        if (collision.gameObject.name.Equals(physicalHandLeft.name))
        {
            hapticFeedback.TriggerHapticRef(leftHaptic.gameObject.GetComponent<ActionBasedController>(), 0.5f, 0.2f);
        }

        if (collision.gameObject.name.Equals(physicalHandRight.name))
        {
            hapticFeedback.TriggerHapticRef(rightHaptic.gameObject.GetComponent<ActionBasedController>(), 0.5f, 0.2f);
        }
    }
}

