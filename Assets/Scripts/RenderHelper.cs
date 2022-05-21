using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderHelper : MonoBehaviour
{
    public int RenderQueueNum = 1998;
    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = RenderQueueNum;
    }
}
