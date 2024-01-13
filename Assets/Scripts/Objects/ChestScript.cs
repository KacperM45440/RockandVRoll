using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestScript : MonoBehaviour
{
    public GameObject usedKey;
    public GameObject keyLock;
    public Transform chestLid;
    public Animator chestAnimator;
    public void LockDestroyed()
    {
        keyLock.SetActive(false);
        keyLock.transform.parent.gameObject.SetActive(false);
        chestAnimator.SetTrigger("OpenLid");
    }
    public void OpenChest()
    {
        StartCoroutine(DisableKey());
        chestAnimator.SetTrigger("OpenLid");
    }
    // Po otworzeniu skrzyni, wylaczamy mozliwosc podniesienia klucza, poniewaz zakladamy, ze jeden klucz jest do jednego zamka
    IEnumerator DisableKey()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(usedKey.GetComponent<XRGrabInteractable>());
        Destroy(usedKey.GetComponent<Rigidbody>());
    }
    public void SpawnParticles()
    {
        // todo
    }
}
