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

    private IEnumerator Lean()
    {
        yield return new WaitForSeconds(1.5f);
        moverRef.position = new Vector3(-0.35f, moverRef.position.y, moverRef.position.z);
    }
}
