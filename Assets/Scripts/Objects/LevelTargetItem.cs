using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LevelTargetItem : MonoBehaviour
{
    public Outline outlineRef;
    public DoorSceneLoader loaderRef;
    public XRGrabInteractable targetItemGrabbable;
    private bool isFirstSelect = true;

    public void ItemInSocket()
    {
        if (isFirstSelect)
        {
            isFirstSelect = false;
            return;
        }

        targetItemGrabbable.enabled = true;
    }

    public void ItemFound()
    {
        EnableOutline();
        StartCoroutine(BackToMenu());
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(loaderRef.LoadInMenu(false));
    }

    private void EnableOutline()
    {
        outlineRef.enabled = true;
    }
}
