using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSizeOverTime : MonoBehaviour
{
    public Vector3 targetSize;
    public float speed;
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetSize, Time.deltaTime * speed);
    }
}
