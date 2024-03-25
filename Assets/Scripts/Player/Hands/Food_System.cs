using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_System : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Debug.Log("Food eaten");
            Destroy(other.gameObject);

        }

    }
}