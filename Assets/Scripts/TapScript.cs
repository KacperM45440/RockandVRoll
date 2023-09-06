using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapScript : MonoBehaviour
{
    private bool mugPlaced;
    public GameObject drinkCapsule;
    public Animator drinkAnimator;
    public void MugPlaced()
    {
        mugPlaced = true;
    }

    public void MugExited()
    {
        mugPlaced = false;
    }


    //todo: postawiæ na kraniku pstryczek zeby to dzialalo
    public void SwitchFlicked()
    {
        if (!mugPlaced)
        {
            return;
        }

        PourDrink();
    }

    private void PourDrink()
    {
        if (drinkCapsule.activeSelf.Equals(true))
        {
            return;
        }

        drinkCapsule.SetActive(true);
        drinkAnimator.SetTrigger("FillUp");
    }
}
