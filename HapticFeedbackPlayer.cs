using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID || UNITY_IPHONE
using CandyCoded.HapticFeedback;
#endif

public class HapticFeedbackPlayer : MonoBehaviour //UNUSED
{
    public static bool enabled = true;
    public static void Refresh()
    {
        enabled = !Utilities.IntToBool(PlayerPrefs.GetInt("HapticsDisabled"));
    }
    public static void Play(int intensity = 0)
    {
#if UNITY_ANDROID || UNITY_IPHONE
        if (enabled){
            switch (intensity)
            {
                case 1:
                    HapticFeedback.MediumFeedback();
                    break;
                case 2:
                    HapticFeedback.HeavyFeedback();
                    break;
                default:
                    HapticFeedback.LightFeedback();
                    break;
            }
        }
#endif
    }
}
