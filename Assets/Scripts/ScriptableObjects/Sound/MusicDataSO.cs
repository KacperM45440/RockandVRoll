using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MusicDataSO : ScriptableObject
{
    // dodac { get; set; } ?
    public float minValue = 0.0001f;
    public float maxValue = 1f;
    public float currSoundValue = 0.1f;
    public float currMusicValue = 0.1f;
    public float changeSignificancy = 0.005f;
}
