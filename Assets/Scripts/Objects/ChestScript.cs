using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestScript : MonoBehaviour
{
    public GameObject usedKey;
    public Transform chestLid;
    public Animator chestAnimator;

    public void OpenChest()
    {
        StartCoroutine(DisableKey());
        chestAnimator.SetTrigger("OpenLid");
    }
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
