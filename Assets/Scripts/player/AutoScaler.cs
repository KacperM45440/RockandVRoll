using UnityEngine;
using System.Collections;

public class AutoScaler : MonoBehaviour
{
    [SerializeField] private float defaultHeight = 1.0f;
    [SerializeField] private new Camera camera;
    [SerializeField] private Transform attachPointLeft;
    [SerializeField] private Transform attachPointRight;

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
        attachPointLeft.localScale = Vector3.one;
        attachPointRight.localScale = Vector3.one;
    }

    private void Start()
    {
        StartCoroutine(ResizeAfterHeadLoaded());
    }

    IEnumerator ResizeAfterHeadLoaded()
    {
        yield return null;
        Resize();
    }
}