using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeInputGraphics : MonoBehaviour
{
    public float speed;
    public float colorSpeed;
    public RectTransform swipeInputGraphic;
    public Image swipeInputFadeImage;
    [HideInInspector]public Color defaultColor;
    float currentYPos;
    bool fingerDownFirstFrame;

    public Image rightImage;
    public Image leftImage;

    bool disabled;

    //float screenWidthMultiplier;
    private void Start()
    {
        defaultColor = swipeInputFadeImage.color;
        swipeInputFadeImage.color = Utilities.Invisible(swipeInputFadeImage.color);
        leftImage.color = Utilities.Invisible(leftImage.color);
        rightImage.color = Utilities.Invisible(rightImage.color);
        //screenWidthMultiplier = PlayerPrefs.GetFloat("ResMultiplier", 0.75f);
    }
    private void Update()
    {
        //print("SCREEN WIDTH: " + Screen.currentResolution.width); //Size of canvas
        //print("SYSTEM WIDTH: " + Display.displays[Utilities.GetCurrentDisplayNumber()].systemWidth); //Size of device screen
        //print("swipeStartX: " + InputManager.swipeStartX);
        if (InputManager.mobileMoveSystem == 1)
        {
            disabled = false;
            if ((InputManager.movingRight || InputManager.movingLeft) && !Level.levelComplete && !Level.death)
            {
                if (!fingerDownFirstFrame)
                {
                    currentYPos = InputManager.fingerPoses[0].y;
                    fingerDownFirstFrame = true;
                }
                //swipeInputGraphic.anchoredPosition = Vector2.Lerp(swipeInputGraphic.anchoredPosition, new Vector2(InputManager.fingerPoses[0].x, swipeInputGraphic.anchoredPosition.y),Time.deltaTime*speed);
                currentYPos = Mathf.Lerp(currentYPos, InputManager.fingerPoses[0].y, Time.deltaTime * 10);

                //swipeInputGraphic.anchoredPosition = Utilities.ViewportPosToCanvasPos(Utilities.DisplayPosToViewportPos(new Vector2(InputManager.swipeStartX,currentYPos)));
                swipeInputGraphic.anchoredPosition = new Vector2(InputManager.swipeStartX*(1/PlayerPrefs.GetFloat("ResMultiplier", 0.75f)), currentYPos*(1/PlayerPrefs.GetFloat("ResMultiplier", 0.75f)));

                swipeInputFadeImage.color = Color.Lerp(swipeInputFadeImage.color, defaultColor, Time.deltaTime*colorSpeed);
                 
                float fingerPosTarget = InputManager.fingerPoses[0].x-(-(((Screen.width/2)-InputManager.fingerPoses[0].x)/5f));

                //InputManager.swipeStartX = Mathf.Lerp(InputManager.swipeStartX, InputManager.fingerPoses[0].x, Time.deltaTime * speed);
                InputManager.swipeStartX = Mathf.Lerp(InputManager.swipeStartX, fingerPosTarget, Time.deltaTime * speed);

                if (InputManager.movingRight) { rightImage.color = Color.Lerp(rightImage.color, defaultColor, Time.deltaTime * colorSpeed * 2); }
                else { rightImage.color = Color.Lerp(rightImage.color, Utilities.Invisible(rightImage.color), Time.deltaTime * colorSpeed * 4); }
                if (InputManager.movingLeft) { leftImage.color = Color.Lerp(leftImage.color, defaultColor, Time.deltaTime * colorSpeed * 2); }
                else { leftImage.color = Color.Lerp(leftImage.color, Utilities.Invisible(leftImage.color), Time.deltaTime * colorSpeed * 4); }
            }
            else
            {
                fingerDownFirstFrame = false;
                swipeInputFadeImage.color = Color.Lerp(swipeInputFadeImage.color, Utilities.Invisible(swipeInputFadeImage.color), Time.deltaTime * colorSpeed);
                leftImage.color = Color.Lerp(leftImage.color, Utilities.Invisible(leftImage.color), Time.deltaTime * colorSpeed * 2);
                rightImage.color = Color.Lerp(rightImage.color, Utilities.Invisible(rightImage.color), Time.deltaTime * colorSpeed * 2);
            }
        }
        else
        {
            if (!disabled)
            {
                swipeInputFadeImage.color = Utilities.Invisible(swipeInputFadeImage.color);
                leftImage.color = Utilities.Invisible(leftImage.color);
                rightImage.color = Utilities.Invisible(rightImage.color);
                disabled = true;
            }
        }
    }
}
