using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelReset : MonoBehaviour
{
    public InputAction leftButtonAction;
    public InputAction rightButtonAction;
    private float timer;
    private float restartTime = 5.0f;

    public Slider restartSlider;

    // Start is called before the first frame update
    private void Start()
    {
        restartSlider.maxValue = restartTime;
        restartSlider.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        leftButtonAction.Enable();
        rightButtonAction.Enable();
    }

    private void OnDisable()
    {
        leftButtonAction.Disable();
        rightButtonAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        float leftPressed = leftButtonAction.ReadValue<float>();
        float rightPressed = rightButtonAction.ReadValue<float>();

        if (Mathf.Approximately(leftPressed, 1) && Mathf.Approximately(rightPressed, 1))
        {
            restartSlider.gameObject.SetActive(true);
            timer += Time.deltaTime;
            restartSlider.value = timer;
            if (timer > restartTime)
            {
                Debug.Log("Restart"); //tu by sie restartowala scena
            }
        }
        else {
            restartSlider.gameObject.SetActive(false);
            timer = 0f;
        }
    }
}