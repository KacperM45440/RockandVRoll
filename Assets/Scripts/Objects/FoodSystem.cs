using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FoodSystem : MonoBehaviour
{    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Food"))
        {
            return;
        }

        if(other.GetType().Equals(typeof(MeshCollider)))
        {
            Destroy(other.gameObject);
        }
        else
        {
            Destroy(other.GetComponentInParent<XRGrabInteractable>().gameObject);
        }
    }
}