using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapScript : MonoBehaviour
{
    private bool mugPlaced;
    public GameObject drinkCapsule;
    public Animator drinkAnimator;
    //public GameObject rightHand;
    //public GameObject leftHand;
    public void MugPlaced()
    {
        mugPlaced = true;
    }

    public void MugExited()
    {
        mugPlaced = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.name.Equals(rightHand.name) || other.name.Equals(leftHand.name))
    //    {
    //        SwitchFlicked();
    //    }
    //}

    //todo: postawiæ na kraniku pstryczek zeby to dzialalo
    public void SwitchFlicked()
    {
        Debug.Log("flicked");
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
