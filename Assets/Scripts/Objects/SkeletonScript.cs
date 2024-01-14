using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : MonoBehaviour
{
    public HingeJoint joint;
    public void MoveSkeleton()
    {
        StartCoroutine(Lean());
    }

    // Uzyj niewidzialnych kostek do wypchniecia szkieleta z szafy, tak aby zaczal upadac w strone gracza
    // Przy naprawionych drzwiach szkielet powinien wypadac sam po otworzeniu drzwi, korzystajac jedynie z fizyki w grze
    private IEnumerator Lean()
    {
        yield return new WaitUntil(() => joint.angle >= 50f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        yield return new WaitForSeconds(2f);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
