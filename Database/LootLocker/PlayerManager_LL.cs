using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using System.Linq;
using System.Globalization;
using UnityEngine.SceneManagement;
public class PlayerManager_LL : MonoBehaviour
{
    [Header("Leave all vars null if instance isn't Leaderboard")]
    public TMP_InputField playerNameInputField;
    public List<string> blacklistedNames;

    public PlaySound errorSFX;
    public TextMeshProUGUI errorText;
    public ObjectShake errorObjectShake;

    public LeaderboardManager_LL leaderboardManagerScript;

    public PlaySound inputConfirmSFX;

    public static bool currentlyLoggedIn;

    public bool dontLoginOnStart;

    private void Start()
    {
        if (errorText != null) { errorText.color = Utilities.Invisible(errorText.color); }
        if (!dontLoginOnStart) { StartCoroutine(LoginRoutine()); }
    }
    public void Login(){
        StartCoroutine(LoginRoutine());
    }
    public void Logout(){
        StartCoroutine(LogoutRoutine());
    }

    public void SubmitName(string name)
    {
        string setName = name;
        if (setName == "") { setName = playerNameInputField.text; }

        if (setName != "")
        {
            bool passed = true;
            bool charPass = true;
            for (int i = 0; i < blacklistedNames.Count; i++){
                if (setName.IndexOf(blacklistedNames[i], System.StringComparison.OrdinalIgnoreCase) >= 0) { NameError("Name not allowed"); errorSFX.Play(); passed = false; print("Display name not allowed"); break; }
            }
            if (setName.Length > 8 || setName.Contains(" ") || setName.Any(ch => !char.IsLetterOrDigit(ch)) || setName == "") { passed = false; print("Display name not allowed"); errorSFX.Play();NameError("Invalid name"); }
            if (passed) 
            {
                SetPlayerName(setName);
            }
        }
        playerNameInputField.text = "";
    }
    void NameError(string error)
    {
        errorText.text = error;
        errorText.color = new Color(errorText.color.r, errorText.color.g, errorText.color.b, 1);
        errorSFX.Play();
        errorObjectShake.Shake(new Vector3(Utilities.CoinFlipn(), 0, 0),5f);
    }
    private void Update()
    {
        if (errorText != null){
            errorText.color = Color.Lerp(errorText.color, Utilities.Invisible(errorText.color), Time.deltaTime*0.75f);
        }
    }
    public void SetPlayerName(string setName)
    {
        name = setName.Replace('0', 'O'); //Specific to Cube Fall font
        LootLockerSDKManager.SetPlayerName(name, (response) => 
        {
            if (response.success)
            {
                Debug.Log("Succesfully set player name to: " + name);
                if (SceneManager.GetActiveScene().name != "Main") { leaderboardManagerScript.RefreshLeaderboard(); }
                else { PlayerPrefs.SetInt("ForceNameSubmitDone", 1); InstanceChange.LoadInstance(SceneManager.GetActiveScene().name, 2); }
                inputConfirmSFX.Play();
            }
            else
            {
                Debug.Log("Could not set player name due to connection error (LL): " + response.Error);
                NameError("Name already taken!");
            }
        });
    }
    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player logged into database (LL)");
                EncryptedPlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                currentlyLoggedIn = true;
                done = true;
            }
            else
            {
                Debug.Log("Could not start database login session (LL)");
                currentlyLoggedIn = false;
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    IEnumerator LogoutRoutine()
    {
        bool done = false;
        LootLockerSDKManager.EndSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player logged out of database (LL)");
                currentlyLoggedIn = false;
                done = true;
            }
            else
            {
                Debug.Log("Could not logout of database login session (LL)");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
