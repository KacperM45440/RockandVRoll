using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorSceneLoader : MonoBehaviour
{
    // próbowa³em tak zrobiæ, nie da siê
    public string sceneName;

    private void OnTriggerEnter(Collider other) 
    {
        // przypisac collider gracza i sprawdzic czy collider ktory wszedl to jest ten z referencji / to be done
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

        yield return new WaitUntil(() => !operation.isDone && !transitionDone);

        operation.allowSceneActivation = true;
        ScreenFader.Instance.FadeIn(); //zak³adaj¹c, ¿e jest DontDestroyOnLoad na graczu to powinno zrobiæ FadeIn
    }
}
