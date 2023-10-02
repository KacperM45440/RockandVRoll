using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarrelController : MonoBehaviour
{
    public Rotator barrelDial;
    private float minValue = 0;
    private float maxValue = 30;
    private float currentBarrelValue = 1;
    private float signalThreshold = 20;
    private bool isReached = false;
    public float changeSignificancy = 1;
    public TextMeshPro barrelText;

    // Start is called before the first frame update
    void Start()
    {
        barrelText.text = "no";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        barrelDial.onDialChange.AddListener(ChangeBarrelValue);
    }

    private void OnDisable()
    {
        barrelDial.onDialChange.RemoveListener(ChangeBarrelValue);
    }

    public void ChangeBarrelValue(float direction) {
        float valueDestination = currentBarrelValue + (changeSignificancy * direction);
        if (valueDestination > maxValue)
        {
            currentBarrelValue = maxValue;
        }
        else if (valueDestination < minValue)
        {
            currentBarrelValue = minValue;
        }
        else
        {
            currentBarrelValue += (changeSignificancy * direction);
        }
        isReached = currentBarrelValue >= signalThreshold;
        barrelText.text = DisplayMessage(isReached);
        //Debug.Log(currentBarrelValue);
    }

    public string DisplayMessage(bool isReached) {
        if (!isReached)
        {
            return "no";
        }
        else {
            return "yes";
        }
    }
}
