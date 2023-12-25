using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTargetItem : MonoBehaviour
{
    public Outline outlineRef;
    public DoorSceneLoader loaderRef;

    public void ItemFound()
    {
        Debug.Log("got it!");
        EnableOutline();
        BackToMenu();
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(loaderRef.Load());
    }

    private void EnableOutline()
    {

    }
}
