using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
{
    public Transform moverRef;

    public void MoveSkeleton()
    {
        StartCoroutine(Lean());
    }

    // Wydarzenie na potrzeby traileru
    // Uzyj niewidzialnych kostek do wypchniecia szkieleta z szafy, tak aby zaczal upadac w strone gracza
    // Przy naprawionych drzwiach szkielet powinien wypadac sam po otworzeniu drzwi, korzystajac jedynie z fizyki w grze
    private IEnumerator Lean()
    {
        yield return new WaitForSeconds(1.5f);
        moverRef.position = new Vector3(-0.35f, moverRef.position.y, moverRef.position.z);
    }
}
