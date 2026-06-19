using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceSubmitName : MonoBehaviour
{
    public PlayerManager_LL playerManagerScript;
    public static bool success;

    public TextMeshProUGUI newHighestScoreText;
    public TextMeshProUGUI gameOverText;
    public RectTransform inputNameGraphics;
    public RectTransform nameInputFieldTransform;
    public TMP_InputField nameInputField;

    public PlaySound typeSFX;
    public PlaySound selectNameInputFieldSFX;

    public ObjectShake errorTextObjectShakeScript;

    float targetNameInputFieldScale;
    private void Start()
    {
        success = false;
        inputNameGraphics.localScale = new Vector3(0, 0, 0);
        if (PlayerPrefs.GetInt("ForceNameSubmitDone",0) == 1) { this.enabled = false; }
        errorTextObjectShakeScript.enabled = false;
    }
    private void Update()
    {
        if (Level.death)
        {
            int requiredScore = 25;
            if (Application.platform == RuntimePlatform.WebGLPlayer) { requiredScore = 150; }
            if ((Score.score/Score.scoreAntiCheatMultiplier) >= requiredScore && PlayerManager_LL.currentlyLoggedIn && !success)
            {
                Initiate();
            }
        }
        if (success)
        {
            inputNameGraphics.localScale = Vector3.Lerp(inputNameGraphics.localScale, new Vector3(1,1,1), Time.deltaTime * 2);

            nameInputFieldTransform.localScale = Vector3.Lerp(nameInputFieldTransform.localScale, new Vector3(targetNameInputFieldScale, targetNameInputFieldScale, targetNameInputFieldScale), Time.deltaTime * 15);
            if (nameInputFieldTransform.localScale.x >= 1.13f) { targetNameInputFieldScale = 1.1f; }
            else if (nameInputFieldTransform.localScale.x <= 0.97f) { targetNameInputFieldScale = 1f; }
        }
    }
    void Initiate()
    {
        success = true;
        newHighestScoreText.text = "<u>Great score!</u>\nSubmit your name\nbelow for\nthe global\nleaderboard";
        newHighestScoreText.fontSize = 0.5f;
        errorTextObjectShakeScript.enabled = true;
        errorTextObjectShakeScript.anchoredPos = errorTextObjectShakeScript.transform.position;
        newHighestScoreText.color = new Color(newHighestScoreText.color.r, newHighestScoreText.color.g, newHighestScoreText.color.b, 1);
        //-2.5Y
        gameOverText.text = "";
    }

    //Below is copy of code from other scripts. Jank
    public void ChangeNameTypeSFX()
    {
        if (nameInputField.text.Length > 0)
        {
            if (!Input.GetKey(KeyCode.Backspace))
            {
                typeSFX.Play();
            }
            else { typeSFX.Play(true, 0.75f); }
        }
        else { typeSFX.Play(true, 0.65f); }
    }
    public void ChangeNameFieldSelect()
    {
        targetNameInputFieldScale = 1.15f;
        selectNameInputFieldSFX.Play(true, 1f);
        HapticFeedbackPlayer.Play(0);
    }
    public void ChangeNameFieldDeselect()
    {
        targetNameInputFieldScale = 0.96f;
        selectNameInputFieldSFX.Play(true, 0.8f);
        HapticFeedbackPlayer.Play(0);
    }
}
