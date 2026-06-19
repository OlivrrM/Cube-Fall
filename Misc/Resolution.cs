using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resolution : MonoBehaviour
{
    public static bool setResolution;
    public CameraMove cameraMoveScript;
    public static bool setResMultiplier;
    public BaseMovement baseMovementScript;
    private void Awake()
    {
        if (!setResolution)
        {
            if (!Application.isMobilePlatform)
            {
                /*
                if (PlayerPrefs.GetInt("PhoneResolution", 0) == 1)
                {
                    Screen.SetResolution((int)(Display.main.systemHeight / 2.166f), (int)Display.main.systemHeight, true);
                }
                else
                {
                    if (Display.main.systemWidth > Display.main.systemHeight * 1.25f)
                    {
                        Screen.SetResolution((int)(Display.main.systemHeight * 1.25f), Display.main.systemHeight, true);
                    }
                }
                */
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    switch (PlayerPrefs.GetInt("AspectRatio", 0))
                    {
                        case 0:
                            Screen.SetResolution((int)((Display.displays[Utilities.GetCurrentDisplayNumber()].systemHeight * PlayerPrefs.GetFloat("ResMultiplier", 1)) * 0.461538f), (int)(Display.displays[Utilities.GetCurrentDisplayNumber()].systemHeight * PlayerPrefs.GetFloat("ResMultiplier", 1)), FullScreenMode.FullScreenWindow);
                            break;
                        case 1:
                            Screen.SetResolution((int)((Display.displays[Utilities.GetCurrentDisplayNumber()].systemHeight * PlayerPrefs.GetFloat("ResMultiplier", 1)) * 0.5625f), (int)(Display.displays[Utilities.GetCurrentDisplayNumber()].systemHeight * PlayerPrefs.GetFloat("ResMultiplier", 1)), FullScreenMode.FullScreenWindow);
                            break;
                        case 2:
                            Screen.SetResolution((int)((Display.displays[Utilities.GetCurrentDisplayNumber()].systemHeight * PlayerPrefs.GetFloat("ResMultiplier", 1)) * 0.75f), (int)(Display.displays[Utilities.GetCurrentDisplayNumber()].systemHeight * PlayerPrefs.GetFloat("ResMultiplier", 1)), FullScreenMode.FullScreenWindow);
                            break;
                        case 3:
                            Screen.SetResolution((int)((Display.displays[Utilities.GetCurrentDisplayNumber()].systemHeight * PlayerPrefs.GetFloat("ResMultiplier", 1))), (int)(Display.displays[Utilities.GetCurrentDisplayNumber()].systemHeight * PlayerPrefs.GetFloat("ResMultiplier", 1)), FullScreenMode.FullScreenWindow);
                            break;
                    }
                }
            }
            else 
            {
                Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true); 
                if (SceneManager.GetActiveScene().name == "Main") { Screen.sleepTimeout = SleepTimeout.NeverSleep; }
                else { Screen.sleepTimeout = SleepTimeout.SystemSetting; }
            }
            Application.targetFrameRate = PlayerPrefs.GetInt("FpsCap", Screen.currentResolution.refreshRate);
            QualitySettings.vSyncCount = 0;
            setResolution = true;
            if (Application.isMobilePlatform)
            {
                if (!setResMultiplier)
                {
                    float multiplier = 0.75f;
                    if (Application.platform == RuntimePlatform.IPhonePlayer) { multiplier = PlayerPrefs.GetFloat("ResMultiplier", 0.75f); }
                    else { multiplier = PlayerPrefs.GetFloat("ResMultiplier", 0.5f); }
                    Screen.SetResolution((int)(Display.main.systemWidth * multiplier), (int)(Display.main.systemHeight * multiplier), true);
                    setResMultiplier = true;
                }
            }
        }

        //Player speed up mechanism
        ///General idea:
        //Do a check to see how much bigger device aspect ratio is compared to target ratio (iPhone 11 and above)
        //Then with that % increase, convert that into a multiplier, which can then be applied to characters base movement speed.
        print("Camera aspect ratio: "+Camera.main.aspect);
        float targetRatio = 0.4620853f;
        float ratioMultiplier = Camera.main.aspect - targetRatio;
        ratioMultiplier = ratioMultiplier / Camera.main.aspect;
        ratioMultiplier+=1f;
        print("Speed multiplier: "+ratioMultiplier);
        if (baseMovementScript != null) { baseMovementScript.movementSpeed *= ratioMultiplier; }
        PlatformObjectManager.gapMultiplier = (ratioMultiplier - 1)/2;

        //Old code. This is when rather than the player going faster, it was the map itself. Much harder as many other variables had to be considered
        /*
        //SE - 0.5622189f
        //MODEL ABOVE SE - 0.462203f
        //Lower we ADD and higher we MINUS
        print("Camera aspect: " + Camera.main.aspect);
        //float screenSpeedMultiplier = 1-(((0.5384615f + Camera.main.aspect)-1)/2);
        float screenSpeedMultiplier = (0.5384615f+0.215f + Camera.main.aspect)-1;
        //0.752797f
        print("Camera aspect speed multiplier: " + screenSpeedMultiplier);
        if (cameraMoveScript != null) { cameraMoveScript.moveSpeed += screenSpeedMultiplier; }
        /* Old Code 
        if (Camera.main.aspect < 0.5f) //Legacy 3.215
        {
            if (cameraMoveScript != null) { cameraMoveScript.moveSpeed += 0.215f; print("(Resolution.cs) Increased camera move speed by 0.215f because tall screen was detected"); }
        }
        else if (Camera.main.aspect > 0.5f && Camera.main.aspect < 0.6f) //Legacy 2.85
        {
            if (cameraMoveScript != null) { cameraMoveScript.moveSpeed -= 0.15f; print("(Resolution.cs) Decreased camera move speed by 0.1f because iPhone SE screen was detected"); }
        }
        */
    }
}
