using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class OpenDoorController : GlitchEffect
{

    [Header("Door rotatiion")]
    [SerializeField] Collider playerCollider;
    [SerializeField] Transform door;
    [SerializeField] float duration;

    [Header("Door rotation range")]
    [SerializeField] Quaternion openDoor = Quaternion.Euler(0, 110f, 0);
    private Quaternion closeDoor;
    [Header("Sound")]
    [SerializeField] AudioSource doorSound;



    private void Awake()
    {
        closeDoor = door.rotation;
        openDoor.x = closeDoor.x;
        openDoor.z = closeDoor.z;
        SetMaterialValues(0, 0, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(playerCollider))
        {
            StopAllCoroutines();
            StartCoroutine(GlitchScreenEffect(true));
            StartCoroutine(MoveDoor(openDoor));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.Equals(playerCollider))
        {
            StopAllCoroutines();
            StartCoroutine(GlitchScreenEffect(false));
            StartCoroutine(MoveDoor(closeDoor));
        }
    }


    IEnumerator MoveDoor(Quaternion targetRotation)
    {
        doorSound.Play();
        Quaternion initialRotation = door.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            door.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
             yield return null;

        }

        
    }
}
