using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicNSFXController : MonoBehaviour, IDial
{
    UnityEvent changeValue;
    //private float minValue = 0.01f;
    //private float maxValue = 1f;
    public float changeSignificancy = 0.03f;
    public bool initialising = true;
    public float previousDialValue = 0;
    public Rotator rotator;
    void Start()
    {
        if (changeValue == null) changeValue = new UnityEvent();
        GetComponent<Rotator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DialChanged(float dialValue) {
        float currentDialValue = dialValue;
        
        float dialDifference = currentDialValue - previousDialValue;
        if (!initialising) previousDialValue = currentDialValue;
        if (initialising) {
            Debug.Log("Initializing");
            initialising = false;
        }
        if (dialDifference > 0 && !(dialDifference > (360 - rotator.snapRotationAmount - 1)) || dialDifference < (-360 + rotator.snapRotationAmount + 1)) Debug.Log("Up");
        if (dialDifference < 0 && !(dialDifference < (-360 + rotator.snapRotationAmount + 1)) || dialDifference > (360 - rotator.snapRotationAmount - 1)) Debug.Log("Down");
        Debug.Log($"percentage power {Mathf.Ceil(currentDialValue)}");
    }
}
