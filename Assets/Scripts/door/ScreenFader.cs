using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    float fadeTime = 1f;

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
        FadeIn(fadeTime);
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

    public void FadeOut(float fadeTime, Action onDone = null)
    {
        this.fadeTime = fadeTime;
        StartCoroutine(Fade(1, onDone));
    }

    public void FadeIn(float fadeTime, Action onDone = null)
    {
        this.fadeTime = fadeTime;
        StartCoroutine(Fade(0, onDone));
    }
}


// Do œciany
// bêdzie trzeba importy
// jakiœ gameObject w okolicy g³owy który ma collider trigger jest potrzebny tutaj
public class WallFadeDetector : MonoBehaviour
{
    public float wallFadeOut = 0.2f;
    public float wallFadeIn = 0.2f;
    private void OnTriggerEnter(Collider other)
    {
        // Przyda³oby siê sprawdziæ czy other jest œcian¹ w jakiœ sposób. Nie jestem pewien jak to zrobiæ dok³adnie tbh. Ale wywo³anie fadera w ten sposób
        ScreenFader.Instance.FadeOut(wallFadeOut);
    }
    private void OnTriggerExit(Collider other)
    {
        ScreenFader.Instance.FadeIn(wallFadeIn);
    }

}