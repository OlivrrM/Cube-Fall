using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OudatedVersionScreen : MonoBehaviour
{
    public PlaySound startSFX;
    public void UpdateGame()
    {
        startSFX.Play(true, 1.5f);
        HapticFeedbackPlayer.Play(0);
        if (Application.platform == RuntimePlatform.Android) { Application.OpenURL("market://details?id=com.OlivrGames.CubeFall"); }
        else if (Application.platform == RuntimePlatform.IPhonePlayer) { Application.OpenURL("https://itunes.apple.com/app/id6456412050"); }
        else { Application.OpenURL("https://olivrr.itch.io/cubefall"); }
    }
    public void Cancel()
    {
        InstanceChange.LoadInstance("Main", 1);
        startSFX.Play(true,0.8f);
        HapticFeedbackPlayer.Play(0);
    }
}
