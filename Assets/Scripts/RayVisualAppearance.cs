using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayVisualAppearance : MonoBehaviour
{
    public int RenderQueueNum;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = RenderQueueNum;
    }
}
