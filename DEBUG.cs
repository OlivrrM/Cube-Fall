using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG : MonoBehaviour
{
    public float capFps;
    private void Start() //Deprecated
    {
        //if (capFps != 0 && Application.isEditor) Application.targetFrameRate = (int)capFps;
    }
}
