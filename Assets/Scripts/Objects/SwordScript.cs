using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public Transform bladeColliders;
    public ChestScript ChestRef;
    public GameObject keyLock;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals(keyLock.name))
        {
            return;
        }

        foreach (Transform child in bladeColliders)
        {
            if(child.GetComponent<Collider>().bounds.Intersects(keyLock.GetComponent<Collider>().bounds))
            {
                ChestRef.LockDestroyed();
            }
        }
    }
}
