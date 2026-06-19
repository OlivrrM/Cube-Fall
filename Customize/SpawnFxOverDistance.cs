using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFxOverDistance : MonoBehaviour
{
    public Transform cam;
    public float targetX;
    public Transform fx;
    private void Update()
    {
        if (cam.position.x > targetX)
        {
            targetX += 30;
            Instantiate(fx, new Vector3(targetX, 0, 0), Quaternion.identity);
        }
    }
}
