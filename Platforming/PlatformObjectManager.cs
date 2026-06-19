using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObjectManager : MonoBehaviour
{
    public static float gapMultiplier;
    public Transform leftPlatform;
    public Transform rightPlatform;

    public static bool shownHsMarker;
    public static int scoreThisPlatform;
    public GameObject hsMarker;
    private void Start()
    {
        leftPlatform.localPosition = new Vector3(leftPlatform.localPosition.x-gapMultiplier, leftPlatform.localPosition.y, leftPlatform.localPosition.z);
        rightPlatform.localPosition = new Vector3(rightPlatform.localPosition.x+gapMultiplier, rightPlatform.localPosition.y, rightPlatform.localPosition.z);
        scoreThisPlatform += 1 + PlatformPass.scoreIncreaseBonus;
        int hs = EncryptedPlayerPrefs.GetInt("HsMobile01");
        if (!shownHsMarker&&scoreThisPlatform>= hs&&hs>3){
            hsMarker.SetActive(true);
            shownHsMarker = true;
        }
        else { hsMarker.SetActive(false); }
    }
}
