using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAfterTime : MonoBehaviour
{
    public Transform[] transforms;
    public float time;
    private void Start()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].position += new Vector3(0, 999, 0);
        }
        print("WAHDIUAWD");
        Invoke("Enable", time);
    }
    void Enable()
    {
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].position -= new Vector3(0, 999, 0);
        }
        print("SDSDASDSSS");
    }
}
