using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlatformPos : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(Random.Range(transform.position.x - (CameraBoundaries.boundaries.x + 2) / 2, transform.position.x + (CameraBoundaries.boundaries.x + 2) / 2),transform.position.y,transform.position.z);
    }
}
