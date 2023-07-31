using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorSceneLoader : MonoBehaviour
{   
    public string sceneName;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            StartCoroutine (Load());
        }
    }

    public IEnumerator Load() {
        bool transitionDone = false;
        ScreenFader.Instance.FadeOut(()=>transitionDone = true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone && !transitionDone) {
            yield return null;
        }
        operation.allowSceneActivation = true;
    }
}
