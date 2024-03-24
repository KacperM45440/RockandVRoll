using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectSoundsSO : ScriptableObject
{
    public AudioClip pickup1;
    public AudioClip pickup2;
    public AudioClip pickup3;
    private int previousSound;
    private int i;

    private void OnEnable()
    {
        // zamiast Awake()
    }
    public void PickedUp(AudioSource givenSource)
    {
        givenSource.PlayOneShot(ChooseSound(), 0.5f);
    }

    private AudioClip ChooseSound()
    {
        List<AudioClip> soundList = new()
        {
            pickup1,
            pickup2,
            pickup3
        };

        while (i.Equals(previousSound))
        {
            i = Random.Range(0, 3);
        }

        previousSound = i;
        return soundList[i];
    }
}

