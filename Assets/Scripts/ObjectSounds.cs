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
    public void PickedUp()
    {
        sourceRef.PlayOneShot(ChooseSound(), 0.5f);
    }

    private AudioClip ChooseSound()
    {
        AudioClip nextSound;
        while (i.Equals(previousSound))
        {
            i = Random.Range(1, 4);
        }

        if (i.Equals(1))
        {
            nextSound = pickup1;
            previousSound = i;
        }
        else if (i.Equals(2))
        {
            nextSound = pickup2;
            previousSound = i;
        }
        else
        {
            nextSound = pickup3;
            previousSound = i;
        }

        Debug.Log(i);
        return nextSound;
    }
}
