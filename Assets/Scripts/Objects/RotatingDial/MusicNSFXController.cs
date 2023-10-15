using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class MusicNSFXController : MonoBehaviour
{
    [SerializeField] private AudioMixer musicNSFX;

    public MusicDataSO dataRef;
    public Rotator sfxDial;
    public Rotator musicDial;
    public TextMeshPro sfxText;
    public TextMeshPro musicText;

    private void Start()
    {
        sfxText.text = ((int)(dataRef.currSoundValue * 10)).ToString();
        musicText.text = ((int)(dataRef.currMusicValue * 10)).ToString();
    }

    private void OnEnable()
    {
        sfxDial.onDialChange.AddListener(ChangeSFXMixerValue);
        musicDial.onDialChange.AddListener(ChangeMusicMixerValue);
    }

    private void OnDisable()
    {
        sfxDial.onDialChange.RemoveListener(ChangeSFXMixerValue);
        musicDial.onDialChange.RemoveListener(ChangeMusicMixerValue);
    }
    private void Update()
    {
        // funkcja do debugu, mozna skasowac 
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("less");
            dataRef.currSoundValue -= 0.1f;
            SetVolume(dataRef.currSoundValue, "SoundParam");
            sfxText.text = ((int)(dataRef.currSoundValue * 10)).ToString();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("more");
            dataRef.currSoundValue += 0.1f;
            SetVolume(dataRef.currSoundValue, "SoundParam");
            sfxText.text = ((int)(dataRef.currSoundValue * 10)).ToString();
        }
    }
    public void SetVolume(float dialvalue, string param)
    {
        musicNSFX.SetFloat(param, Mathf.Log10(dialvalue) * 20);
    }
    public void ChangeSFXMixerValue(float direction)
    {
        float valueDestination = dataRef.currSoundValue + (dataRef.changeSignificancy * direction);
        if (valueDestination > dataRef.maxValue)
        {
            dataRef.currSoundValue = dataRef.maxValue;
        }
        else if (valueDestination < dataRef.minValue)
        {
            dataRef.currSoundValue = dataRef.minValue;
        }
        else
        {
            dataRef.currSoundValue += (dataRef.changeSignificancy * direction);
        }
        SetVolume(dataRef.currSoundValue, "SoundParam");
        sfxText.text = ((int) (dataRef.currSoundValue * 10)).ToString();
    }
    public void ChangeMusicMixerValue(float direction)
    {
        float valueDestination = dataRef.currMusicValue + (dataRef.changeSignificancy * direction);
        if (valueDestination > dataRef.maxValue)
        {
            dataRef.currMusicValue = dataRef.maxValue;
        }
        else if (valueDestination < dataRef.minValue)
        {
            dataRef.currMusicValue = dataRef.minValue;
        }
        else
        {
            dataRef.currMusicValue += (dataRef.changeSignificancy * direction);
        }
        SetVolume(dataRef.currMusicValue, "MusicParam");
        musicText.text = ((int) (dataRef.currMusicValue * 10)).ToString();
    }
}
