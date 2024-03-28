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
    public UnityEngine.Object choosenLevel;
    public float fadeInTime = 0.5f;
    public float fadeOutTime = 1f;
    public Collider playerCollider;

    private void OnTriggerEnter(Collider other)
    {
        // przypisac collider gracza i sprawdzic czy collider ktory wszedl to jest ten z referencji /done
        // player experience przy ³adowaniu przejœcia by³by miserable - portal netherowy jest mêcz¹cy z czasem (a to tylko jeden portal do jednego miejsca, a nie wszystkich miejsc)
        // no chyba ¿e na czas fade, to to by mia³o jeszcze sens
       
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
            SceneManager.LoadScene(choosenLevel.name);
        }
    }
}