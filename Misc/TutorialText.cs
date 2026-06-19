using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    bool active;
    public float showTime;
    float currentTime;
    private void Start()
    {
        if (!Application.isMobilePlatform){
            tutorialText.text = "";
            tutorialText.color = Utilities.Invisible(tutorialText.color);
            this.enabled = false;
        }
        switch (PlayerPrefs.GetInt("InputType"))
        {
            case 0:
                if (PlayerPrefs.GetInt("InputTut0",0) == 0)
                {
                    tutorialText.text = "Hold the left or right side of the screen to move";
                    active = true;
                    PlayerPrefs.SetInt("InputTut0", 1);
                }
                break;
            case 1:
                if (PlayerPrefs.GetInt("InputTut1", 0) == 0)
                {
                    tutorialText.text = "Swipe and hold to the left or right side of the screen to move";
                    active = true;
                    PlayerPrefs.SetInt("InputTut1", 1);
                }
                break;
            case 2:
                if (PlayerPrefs.GetInt("InputTut2", 0) == 0)
                {
                    tutorialText.text = "Tilt your device to the left or right to move";
                    active = true;
                    PlayerPrefs.SetInt("InputTut2", 1);
                }
                break;
            default:
                tutorialText.text = "";
                active = true;
                break;
        }
        if (!active) { tutorialText.gameObject.SetActive(false); }
    }
    private void Update()
    {
        if (active)
        {
            tutorialText.rectTransform.anchoredPosition += new Vector2(0, Time.deltaTime/2.5f);
            currentTime += Time.deltaTime;
            if (currentTime >= showTime)
            {
                tutorialText.color = Color.Lerp(tutorialText.color, Utilities.Invisible(tutorialText.color), Time.deltaTime * 2);
                if (tutorialText.color.a < 0.01f)
                {
                    tutorialText.color = Utilities.Invisible(tutorialText.color);
                    this.active = false;
                }
            }
        }
        else
        {
            this.active = false;
        }
    }
}
