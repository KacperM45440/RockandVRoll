using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDoorOpen : MonoBehaviour
{
    public enum State { Open, Close };
    public State currentState;
    [SerializeField] Transform door;
    [SerializeField] Collider playerCollider;
    private float time = 3f;

    private Quaternion rotationRange;
    private Quaternion rotationZero;


    [Header("Sounds")]
    [SerializeField] AudioSource openDoorSound;
    [SerializeField] AudioSource closeDoorSound;


    private void Awake()
    {
        rotationZero = door.rotation;
        rotationRange = Quaternion.Euler(0,door.rotation.y + 100f,0);
        door.rotation = rotationZero;

        currentState = State.Close;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.Equals(playerCollider))
        {
            currentState = State.Open;
            StopAllCoroutines();
            closeDoorSound.Stop();
            openDoorSound.Play();
            StartCoroutine(DoorAnimation(rotationRange));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.Equals(playerCollider)) 
        { 
            currentState = State.Close;
            StopAllCoroutines();
            closeDoorSound.Play();
            openDoorSound.Stop();
            StartCoroutine(DoorAnimation(rotationZero));

        }
    }

    private IEnumerator DoorAnimation(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion initialRotation = door.transform.rotation;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / time);
            door.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        
        door.transform.rotation = targetRotation;
    }




}
