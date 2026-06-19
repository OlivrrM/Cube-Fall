using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawn : MonoBehaviour
{
    public bool disabled;
    public static Level levelScript;
    public List<float> platfromsRquiredPerLevel;
    public static float platformsSinceSpawn;

    public static float platformsRequiredForNextCoin; //ONLY USED FOR CONSOLE DEBUG

    public GameObject coin;

    public static int nextCost;
    private void Start()
    {
        switch(EncryptedPlayerPrefs.GetInt("skinsUnlocked", 0))
        {
            case 0:
                nextCost = 5;
                break;
            case 1:
                nextCost = 10;
                break;
            case 2:
                nextCost = 15;
                break;
            default:
                nextCost = 20;
                break;
        }
        if (!disabled)
        {
            if (levelScript == null) { levelScript = GameObject.Find("InstanceScripts").GetComponent<Level>(); }
            platformsSinceSpawn++;
            float platformsRequired = 0;
            if (((levelScript.level / levelScript.levelAntiCheatMultiplier) + 1) >= platfromsRquiredPerLevel.Count)
            {
                if ((levelScript.level / levelScript.levelAntiCheatMultiplier) >= 12) { platformsRequired = 15; }
                else if ((levelScript.level / levelScript.levelAntiCheatMultiplier) >= 56) { platformsRequired = 12; }
                else { platformsRequired = 18; }
            }
            else { platformsRequired = platfromsRquiredPerLevel[(levelScript.level / levelScript.levelAntiCheatMultiplier) + 1]; }
            if (platformsSinceSpawn >= platformsRequired)
            {
                coin.SetActive(true);
                platformsSinceSpawn = 0;
            }
            else
            {
                coin.SetActive(false);
            }
            platformsRequiredForNextCoin = platformsRequired;

            //print(platformsSinceSpawn + "    " + platformsRequired);
        }
        else { coin.SetActive(false); }
    }
}
