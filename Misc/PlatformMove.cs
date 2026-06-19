using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public float moveSpeed;
    private void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }
}
