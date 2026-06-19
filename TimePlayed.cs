using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlayed : MonoBehaviour
{
    float timePlayed;
    private void Start()
    {
        timePlayed = EncryptedPlayerPrefs.GetFloat("TimePlayed");
    }
    private void Update()
    {
        if (!Level.death) timePlayed += Time.deltaTime;
    }
    private void OnDestroy()
    {
        EncryptedPlayerPrefs.SetFloat("TimePlayed", timePlayed);
    }
    private void OnApplicationQuit()
    {
        EncryptedPlayerPrefs.SetFloat("TimePlayed", timePlayed);
    }
}
