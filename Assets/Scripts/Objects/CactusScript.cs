using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CactusScript : MonoBehaviour
{
    public Haptic hapticFeedback;
    public Collider cactusCollider;
    private void OnTriggerEnter(Collider other)
    {
        if(cactusCollider.bounds.Intersects(other.bounds))
        {
            Debug.Log("hello");
            hapticFeedback.TriggerHapticRef(other.gameObject.GetComponent<ActionBasedController>(), 0.5f, 0.2f);
        }
    }
}
