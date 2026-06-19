using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectLifetime : MonoBehaviour
{
    public float time;
    float currentTime;
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= time) Destroy(gameObject);
    }
}
