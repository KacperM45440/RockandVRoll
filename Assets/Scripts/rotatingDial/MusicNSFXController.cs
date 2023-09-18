using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class MusicNSFXController : MonoBehaviour
{
    [SerializeField] private AudioMixer musicNSFX;

    private float minValue = 0.0001f;
    private float maxValue = 1f;
    private float currSoundValue = 0.1f;
    private float currMusicValue = 0.1f;
    public float changeSignificancy = 0.005f;
    public Rotator sfxDial;
    public Rotator musicDial;
    public TextMeshPro sfxText;
    public TextMeshPro musicText;

    private void Start()
    {
        sfxText.text = ((int)(currSoundValue * 100)).ToString();
        musicText.text = ((int)(currMusicValue * 100)).ToString();
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
    public void SetVolume(float dialvalue, string param)
    {
        musicNSFX.SetFloat(param, Mathf.Log10(dialvalue) * 20);
    }
    public void ChangeSFXMixerValue(float direction)
    {
        float valueDestination = currSoundValue + (changeSignificancy * direction);
        if (valueDestination > maxValue)
        {
            currSoundValue = maxValue;
        }
        else if (valueDestination < minValue)
        {
            currSoundValue = minValue;
        }
        else
        {
            currSoundValue += (changeSignificancy * direction);
        }
        SetVolume(currSoundValue, "SoundParam");
        sfxText.text = ((int) (currSoundValue * 100)).ToString();
    }
    public void ChangeMusicMixerValue(float direction)
    {
        float valueDestination = currMusicValue + (changeSignificancy * direction);
        if (valueDestination > maxValue)
        {
            currMusicValue = maxValue;
        }
        else if (valueDestination < minValue)
        {
            currMusicValue = minValue;
        }
        else
        {
            currMusicValue += (changeSignificancy * direction);
        }
        SetVolume(currMusicValue, "MusicParam");
        musicText.text = ((int) (currMusicValue * 100)).ToString();
    }
}
