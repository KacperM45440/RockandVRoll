using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public MusicDataSO dataRef;
    public AudioMixer mixerRef;
    void Start()
    {
        mixerRef.SetFloat("SoundParam", Mathf.Log10(dataRef.currSoundValue) * 20);
        mixerRef.SetFloat("MusicParam", Mathf.Log10(dataRef.currMusicValue) * 20);    
    }
}
