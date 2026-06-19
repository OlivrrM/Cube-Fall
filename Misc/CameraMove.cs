using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed;
    public float moveSpeedPC;
    public float firstLevelMultiplier = 1;
    [HideInInspector] public float levelCompleteMultiplier;
    private void Awake()
    {
        /*
        if (Screen.width > Screen.height)
        {
            moveSpeed = moveSpeedPC;
            print("(CameraMove.cs) Set moveSpeed to moveSpeedPC because PC aspect ratio was detected");
        }
        */
    }
    private void Update()
    {
        transform.position += new Vector3(0, (moveSpeed * levelCompleteMultiplier) * firstLevelMultiplier * -Time.deltaTime, 0);
    }
}
