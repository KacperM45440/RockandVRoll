using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DoorSceneLoader : MonoBehaviour
{
    // public string sceneName;
    public UnityEngine.Object choosenLevel;
    public float fadeInTime = 0.5f;
    public float fadeOutTime = 1;
    public Collider playerCollider;

    private void OnTriggerEnter(Collider other)
    {
        // przypisac collider gracza i sprawdzic czy collider ktory wszedl to jest ten z referencji /done
        // player experience przy ³adowaniu przejœcia by³by miserable - portal netherowy jest mêcz¹cy z czasem (a to tylko jeden portal do jednego miejsca, a nie wszystkich miejsc)
        // no chyba ¿e na czas fade, to to by mia³o jeszcze sens
        if (other == playerCollider)
        {
            StartCoroutine(Load());
        }
    }

    public IEnumerator Load()
    {
        bool transitionDone = false;
        ScreenFader.Instance.FadeOut(fadeOutTime, () => transitionDone = true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(choosenLevel.name);
        operation.allowSceneActivation = false;

        yield return new WaitUntil(() => !operation.isDone && !transitionDone);

        operation.allowSceneActivation = true;
        ScreenFader.Instance.FadeIn(fadeInTime); //zak³adaj¹c, ¿e jest DontDestroyOnLoad na graczu to powinno zrobiæ FadeIn
    }
}