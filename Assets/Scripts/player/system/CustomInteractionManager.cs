using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomInteractionManager : XRInteractionManager
{
    private void Start()
    {
        Application.targetFrameRate = 120;
    }
    // Poniewa¿ zwyk³y interaction manager jest read-only, tworzê swój w³asny dziedzicz¹c po nim dodaj¹c potrzebn¹ mi do kontrolera interakcji funkcjê
    // Reczne wylaczenie zaznaczenia przedmiotu pozwala pozniej na stworzenie naszej wlasnej logiki interaktowania z przedmiotem na dystans
    public void ForceDeselect(XRBaseInteractor interactor)
    {
        while (interactor.interactablesSelected.Count > 0)
        {
            SelectExit(interactor, interactor.interactablesSelected[0]);
        }
    }
}
