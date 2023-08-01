using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorSceneLoader : MonoBehaviour
{
    // o ile chcemy stringa, tak nie odwolujemy sie do sceny po nazwie jeszcze w tym miejscu
    // zrob z tego "public Scene mainScene" i parametr w linijce 26 przerob na "mainScene.name"
    public string sceneName;

    private void OnTriggerEnter(Collider other) 
    {
        // przypisac collider gracza i sprawdzic czy collider ktory wszedl to jest ten z referencji
        if (other.CompareTag("Player")) 
        {
            StartCoroutine(Load());
        }
    }

    public IEnumerator Load() 
    {
        bool transitionDone = false;
        ScreenFader.Instance.FadeOut(()=>transitionDone = true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        
        // z tego zrobic "yield return new WaitUntil(() => costam)" i zobaczyc czy dalej dziala
        while (!operation.isDone && !transitionDone) 
        {
            yield return null;
        }
        operation.allowSceneActivation = true;
    }
}
