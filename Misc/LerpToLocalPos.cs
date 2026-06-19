using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToLocalPos : MonoBehaviour
{
    public Transform targetX;
    public float speed;
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetX.position.x, transform.position.y, transform.position.z),Time.deltaTime*speed);
    }
}
