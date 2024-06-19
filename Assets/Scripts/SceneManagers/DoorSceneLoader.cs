using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorSceneLoader : MonoBehaviour
{
   // [Header("Glitch effect")]
   // [SerializeField] Material material;
   // [SerializeField] float noiseAmount;
   // [SerializeField] float glitchStrength;
   // [Range(0.0f, 1.0f)]
   // [SerializeField] float scanLinesStrength;


    // public string sceneName;
    [Header("Load Level")]
    //public UnityEngine.Object chosenLevel;
    public float fadeInTime = 0.5f;
    public float fadeOutTime = 1f;
    public Collider playerCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(playerCollider))
        {
            StartCoroutine(LoadIn(false));
        }
    }
    public IEnumerator LoadIn(bool loadIn)
    {
        bool transitionDone = false;
        if (loadIn)
        {
            ScreenFader.Instance.FadeIn(fadeInTime);
        }
        else
        {
            ScreenFader.Instance.FadeOut(fadeOutTime, () => transitionDone = true);
            yield return new WaitUntil(() => transitionDone);
            //SceneManager.LoadScene(chosenLevel.name);
            SceneManager.LoadScene("Poziom 1");
        }
    }

    public IEnumerator LoadInMenu(bool loadIn)
    {
        bool transitionDone = false;
        if (loadIn)
        {
            ScreenFader.Instance.FadeIn(fadeInTime);
        }
        else
        {
            ScreenFader.Instance.FadeOut(fadeOutTime, () => transitionDone = true);
            yield return new WaitUntil(() => transitionDone);
            //SceneManager.LoadScene(chosenLevel.name);
            SceneManager.LoadScene("Menu");
        }
    }
}