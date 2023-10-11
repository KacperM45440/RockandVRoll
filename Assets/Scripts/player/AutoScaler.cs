using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoScaler : MonoBehaviour
{
    [SerializeField] private float defaultHeight = 1.0f;
    [SerializeField] private new Camera camera;
    [SerializeField] private List<Transform> scaleList = new List<Transform>();
    [SerializeField] private List<Transform> rescaleList = new List<Transform>();
    [SerializeField] private Transform rightHandChild;
    [SerializeField] private GameObject rightHandGameObj;

    private void FindChild() {
        rightHandChild = rightHandGameObj.transform.Find("[RightHand] Attach");
        scaleList.Add(rightHandChild);
    }

    private void Resize()
    {
        float headHeight = camera.transform.localPosition.y;
        if (headHeight == 0)
        {
            Debug.Log("Autoscale disabled - no headset");
            return;
        }
        float scale = defaultHeight / headHeight;
        transform.localScale = Vector3.one * scale;
        foreach (Transform t in scaleList) {
            if (t == null) continue;
            t.localScale = Vector3.one / scale;
        }
        foreach (Transform t in rescaleList)
        {
            if (t == null) continue;
            t.localScale = Vector3.one * scale;
        }
    }

    private void Start()
    {
        StartCoroutine(ResizeAfterHeadLoaded());
    }

  /*  private void Update()
    {
        if (!addRightOnce) {
            if (rightHandGameObj != null) //chcialem sprawdzac, kiedy transform bedzie mial childa, ale null to co innego
            {
                addRightOnce = true;
            }
            //FindChild();
        }
    }*/

    IEnumerator ResizeAfterHeadLoaded()
    {
        yield return null;
        //FindChild();
        Resize();
    }
}