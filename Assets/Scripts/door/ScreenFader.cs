using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    public float fadeTime = 1f;

    // sure
    private Renderer rendererRef;

    private void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
        }

        else 
        {
            Destroy(gameObject);
        }
    }
    private void Start() 
    {
        // komponent zamkniêty w sobie, wiêc nie trzeba przypisania
        rendererRef = GetComponent<MeshRenderer>();
        rendererRef.material.SetColor("_Color", new Color(0f, 0f, 0f, 1f));
        FadeIn();
    }

    private IEnumerator Fade(float targetAlpha, Action? doneCallback = null) 
    {
        var currentColor = rendererRef.material.GetColor("_Color");
        var currentAlpha = currentColor.a;
        float time = 0f;

        while (time <= fadeTime) 
        {
            time += Time.deltaTime;
            var alpha = Mathf.Lerp(currentAlpha, targetAlpha, time / fadeTime);
            rendererRef.material.SetColor("_Color", new Color(0f, 0f, 0f, alpha));
            yield return null;
        }

        if (doneCallback != null) 
        {
            doneCallback.Invoke();
        }
    }

    public void FadeOut(Action onDone = null) 
    {
        StartCoroutine(Fade(1, onDone));
    }

    public void FadeIn(Action onDone = null) 
    {
        StartCoroutine(Fade(0, onDone));
    }
}
