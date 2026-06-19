using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    Vector3 rotation;
    private void Start()
    {
        rotation = transform.eulerAngles;
    }
    private void Update()
    {
        transform.eulerAngles = rotation;
    }
}
