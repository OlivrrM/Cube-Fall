using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LeaderboardInstance : MonoBehaviour
{
    public LeaderboardManager_LL leaderboardScript;
    public PlaySound backSFX;

    public int SelectedLeaderboard;

    public RectTransform selectedBoardGraphic;
    public float[] selectedBoardGraphicLerpPoses;

    public TextMeshProUGUI scoresTitle;
    public TMP_InputField nameInputField;
    public RectTransform nameInputFieldTransform;
    float targetNameInputFieldScale;

    public PlaySound toggleLeaderboardSFX;
    public PlaySound typeSFX;
    public PlaySound selectNameInputFieldSFX;

    public float nameInputFieldBobSpeed;
    private void Start()
    {
        leaderboardScript.RefreshLeaderboard();
        Application.targetFrameRate = 60;
    }
    private void OnDestroy()
    {
        Application.targetFrameRate = PlayerPrefs.GetInt("FpsCap", Screen.currentResolution.refreshRate);
    }
    private void Update()
    {
        selectedBoardGraphic.anchoredPosition = new Vector2(Mathf.Lerp(selectedBoardGraphic.anchoredPosition.x, selectedBoardGraphicLerpPoses[SelectedLeaderboard], Time.deltaTime*15),selectedBoardGraphic.anchoredPosition.y);
        nameInputFieldTransform.localScale = Vector3.Lerp(nameInputFieldTransform.localScale, new Vector3(targetNameInputFieldScale, targetNameInputFieldScale, targetNameInputFieldScale), Time.deltaTime*nameInputFieldBobSpeed);
        if (nameInputFieldTransform.localScale.x >= 1.13f) { targetNameInputFieldScale = 1.1f; }
        else if (nameInputFieldTransform.localScale.x <= 0.97f) { targetNameInputFieldScale = 1f; }
        if (Input.GetKeyDown(KeyCode.Return)) { EventSystem.current.SetSelectedGameObject(null); }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) { SelectLeaderboard(1); }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) { SelectLeaderboard(0); }
        else if (Input.GetKeyDown(KeyCode.Escape)) { MainMenu(); }
    }
    public void SelectLeaderboard(int ID)
    {
        SelectedLeaderboard = ID;
        switch (SelectedLeaderboard)
        {
            case 0:
                scoresTitle.text = "Scores";
                toggleLeaderboardSFX.Play(true, 0.85f);
                break;
            case 1:
                scoresTitle.text = "Levels";
                toggleLeaderboardSFX.Play(true, 1.15f);
                break;
            case 2:
                scoresTitle.text = "Scores";
                break;
        }
        leaderboardScript.page = 0;
        leaderboardScript.RefreshLeaderboard();
        HapticFeedbackPlayer.Play(2);
    }
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
    public void MainMenu()
    {
        InstanceChange.LoadInstance("MainMenu", 1);
        backSFX.Play();
        HapticFeedbackPlayer.Play(0);
    }
}
