using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore;
public class DoorSceneLoader : MonoBehaviour
{
    // public string sceneName;
    [Header("Black Screen Loader")]

    public UnityEngine.Object choosenLevel;
    public float fadeInTime;
    public float fadeOutTime;
    public Collider playerCollider;

    [Header("Glitch Screen effect")]

    public Material ScreenGlithMaterial;
    public float NoiseAmount;
    public float GlithStrength;
    [Range(0f, 1f)]
    public float ScanLinesStrength;

    [Header("Sound")]
    public AudioSource audioSource;

    private void Awake()
    {
        //Ustawia glith ekranu na zero
        GlithDebug(0, 0, 1);

    }
    private void OnTriggerEnter(Collider other)
    {
        // przypisac collider gracza i sprawdzic czy collider ktory wszedl to jest ten z referencji /done
        // player experience przy ³adowaniu przejœcia by³by miserable - portal netherowy jest mêcz¹cy z czasem (a to tylko jeden portal do jednego miejsca, a nie wszystkich miejsc)
        // no chyba ¿e na czas fade, to to by mia³o jeszcze sens
       
        if (other.Equals(playerCollider))
        {
            // StartCoroutine(LoadIn(false));

            StopAllCoroutines();
            StartCoroutine(LoadLevel(NoiseAmount, GlithStrength, ScanLinesStrength, true));
            Debug.Log("Player In");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.Equals(playerCollider))
        {
            StopAllCoroutines();
            StartCoroutine(LoadLevel(NoiseAmount, GlithStrength, ScanLinesStrength, false));
            Debug.Log("Player Out");


        }
    }

    public float elapsedTime = 0f;
    public float percent =0f;
    public IEnumerator LoadLevel(float noise, float glith, float lines, bool entering)
    {

        while (elapsedTime <= fadeOutTime + fadeInTime) 
        {
            if(elapsedTime >=0)
            {
             var check = entering? elapsedTime += Time.deltaTime : elapsedTime -= Time.deltaTime;

            }
            if(elapsedTime < 0)
            {
                elapsedTime = 0;
            }
            
            percent = elapsedTime / (fadeOutTime + fadeInTime);
            if(entering &&  percent >= 0)
            {
                audioSource.Play();
            }
            else if(!entering && percent <= 0)
            {
                audioSource.Stop();
            }
           
            //audioSource.volume = percent;
            GlithDebug(noise * percent, glith * percent, lines * (1-percent));

            yield return null;
        }
        Debug.Log("Player has been transferred to: " + choosenLevel.name);
        GlithDebug(0, 0, 1);
        SceneManager.LoadScene(choosenLevel.name);

        yield return null;
    }


    private void GlithDebug(float noise, float glith, float lines)
    {
        SetMaterialValues("_NoiseAmount", noise);
        SetMaterialValues("_GlitchStrength", glith);
        SetMaterialValues("_ScanLinesStrength", lines);
    }

    private void SetMaterialValues(string name, float value)
    {
        ScreenGlithMaterial.SetFloat(name, value);
       

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