using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAllData : MonoBehaviour //Moved to ResetPlayerPrefs.cs
{
    float resetTime;
    private void Update()
    {
        if (Input.GetKey(KeyCode.F9)) {
            resetTime += Time.deltaTime; 
            if (resetTime >= 3)
            {
                PlayerPrefs.DeleteAll();
                InstanceChange.LoadInstance("MainMenu", 1);
            }
        }
        else
        {
            resetTime = 0;
        }
    }
}
