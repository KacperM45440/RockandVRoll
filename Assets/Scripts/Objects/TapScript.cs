using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapScript : MonoBehaviour
{
    private bool mugPlaced;
    public GameObject drinkCapsule;
    public Animator drinkAnimator;

    // Informacja o zmianie stanu kufla przypisana jest w inspektorze jako event
    public void MugPlaced()
    {
        mugPlaced = true;
    }

    public void MugExited()
    {
        mugPlaced = false;
    }

    // Gdy kufel jest polozony, oraz przekrecony zostanie kurek, wypelnij kufel napojem
    public void SwitchFlicked()
    {
        if (!mugPlaced)
        {
            return;
        }

        PourDrink();
    }

    // Wypelnienie kufla zrobione jest jako prosta animacja pojawiajacej sie kapsuly w kuflu
    // "Ciecz" nie posiada zadnej fizyki ani elementow sfx, jest pustym obiektem bez kolizji
    // W przyszlosci mozna dorobic funkcje odpowiedzialna za wylewanie napoju po obroceniu kuflem o 180 stopni
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
