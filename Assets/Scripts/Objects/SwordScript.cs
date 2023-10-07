using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public Transform bladeColliders;
    public ChestScript ChestRef;
    public GameObject keyLock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals(keyLock.name))
        {
            return;
        }

        // Do usprawnienia
        // Easter egg w ktorym mozna uzyc miecza do rozwalenia klodki zamiast korzystania z klucza
        foreach (Transform child in bladeColliders)
        {
            if(child.GetComponent<Collider>().bounds.Intersects(keyLock.GetComponent<Collider>().bounds))
            {
                ChestRef.LockDestroyed();
            }
        }
    }
}
