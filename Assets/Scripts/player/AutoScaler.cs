using UnityEngine;
using System.Collections;

public class AutoScaler : MonoBehaviour
{
    [SerializeField] private float defaultHeight = 1.0f;
    [SerializeField] private new Camera camera;

    private void Resize() {
        float headHeight = camera.transform.localPosition.y;
        if (headHeight == 0) {
            Debug.Log("Autoscale disabled - no headset");
            return;
        }
        float scale = defaultHeight / headHeight;
        transform.localScale = Vector3.one * scale;
    }

    private void Start()
    {
        StartCoroutine(ResizeAfterHeadLoaded());
    }

    IEnumerator ResizeAfterHeadLoaded() {
        yield return null;
        Resize();
    }
}
