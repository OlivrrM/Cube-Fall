using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedInfo : MonoBehaviour
{
    bool updatedThisInstance;
    private void Update()
    {
        if (Level.death && !updatedThisInstance)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable) { EncryptedPlayerPrefs.SetInt("noWifiGames", EncryptedPlayerPrefs.GetInt("noWifiGames", 0) + 1); }
            else { EncryptedPlayerPrefs.SetInt("withWifiGames", EncryptedPlayerPrefs.GetInt("withWifiGames", 0) + 1); }
            updatedThisInstance = true;
        }
    }
}
