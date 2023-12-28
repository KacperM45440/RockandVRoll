using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTargetItem : MonoBehaviour
{
    public Outline outlineRef;
    public DoorSceneLoader loaderRef;

    public void ItemFound()
    {
        EnableOutline();
        StartCoroutine(BackToMenu());
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(loaderRef.LoadIn(false));
    }

    private void EnableOutline()
    {

    }
}
