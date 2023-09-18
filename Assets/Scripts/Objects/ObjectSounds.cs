using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSounds : MonoBehaviour
{
    public AudioClip pickup1;
    public AudioClip pickup2;
    public AudioClip pickup3;
    public AudioSource sourceRef;
    private int previousSound;
    private int i;
    private bool canPlaySound = false;
    public void PickedUp()
    {
        sourceRef.PlayOneShot(ChooseSound(pickup1, pickup2, pickup3), 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canPlaySound)
        {
            canPlaySound = false;
            sourceRef.PlayOneShot(ChooseSound(pickup1, pickup2, pickup3), 0.5f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        canPlaySound = true;
    }
    private AudioClip ChooseSound(AudioClip clip1, AudioClip clip2, AudioClip clip3)
    {
        AudioClip nextSound;
        while (i.Equals(previousSound))
        {
            i = Random.Range(1, 4);
        }

        if (i.Equals(1))
        {
            nextSound = clip1;
            previousSound = i;
        }
        else if (i.Equals(2))
        {
            nextSound = clip2;
            previousSound = i;
        }
        else
        {
            nextSound = clip3;
            previousSound = i;
        }

        return nextSound;
    }
}
