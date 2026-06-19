using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class fpsCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsCounterText;

    float secondCounter;
    public float FPS;

    int frame;

    string method;

    int refreshRate;

    public Color badFpsColor;
    public Color goodFpsColor;
    public Color amazingFpsColor;
    private float count;
    private void Start()
    {
        method = "rawFrames";
        refreshRate = Screen.currentResolution.refreshRate;
    }
    private void Update()
    {
        if (FPS < refreshRate - (refreshRate / 6))
        {
            fpsCounterText.color = badFpsColor;
        }
        else if (FPS > refreshRate + (refreshRate / 6))
        {
            fpsCounterText.color = amazingFpsColor;
        }
        else
        {
            fpsCounterText.color = goodFpsColor;
        }
        switch (method)
        {
            case "unscaledDeltaTime":
                secondCounter += Time.deltaTime;
                float current = 0;
                current = (int)(1f / Time.unscaledDeltaTime);
                FPS = (int)current;
                if (secondCounter > 0.25f)
                {
                    secondCounter = 0;
                }
                break;
            case "rawFrames":
                secondCounter += Time.deltaTime;
                frame++;
                if (secondCounter >= 0.5f)
                {
                    FPS = frame * 2;
                    frame = 0;
                    secondCounter = 0;
                }
                break;
            default:
                FPS = 404;
                break;
        }
        fpsCounterText.text = FPS.ToString();
    }
}
