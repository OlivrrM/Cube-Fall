using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LeaderboardManager_LL : MonoBehaviour
{
    int leaderboardID = 16320;
    int levelBoardID = 16321;

    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

    [HideInInspector] public int page;
    bool finalPage;

    public Image upArrow;
    public Image downArrow;

    bool cannotConnectToDB;

    public LeaderboardInstance leaderboardInstanceScript;

    public PlaySound navigateSFX;

    public TextMeshProUGUI yourRankText;

    public BaseMovement baseMovementScript;

    [HideInInspector] public int playersCurrentlyDisplayed;
    public int thisPlayerDisplayID;

    public LootLockerLeaderboardMember[] loadedMembers;

    public string thisPlayerColorHex;

    public GameObject noConnectionScreen;
    private void Start()
    {
        if (noConnectionScreen != null) { noConnectionScreen.SetActive(false); }
        thisPlayerDisplayID = 404;
        if (SceneManager.GetActiveScene().name == "Leaderboard"){
            if (Camera.main.aspect <= 0.5f){
                playerScores.rectTransform.anchoredPosition = new Vector2(playerScores.rectTransform.anchoredPosition.x, -107.5f);
                playerScores.lineSpacing = 18;
                playerScores.fontSize = 50;
            }
            else if (Camera.main.aspect >= 0.58f){
                playerScores.rectTransform.anchoredPosition = new Vector2(playerScores.rectTransform.anchoredPosition.x - 13, playerScores.rectTransform.anchoredPosition.y);
                playerNames.rectTransform.anchoredPosition = new Vector2(playerNames.rectTransform.anchoredPosition.x + 13, playerNames.rectTransform.anchoredPosition.y);
            }
        }
    }
    public IEnumerator SubmitScoreRoutine(int scoreToUpload,int level, bool levelSubmit = false)
    {
        bool done = false;
        string playerID = EncryptedPlayerPrefs.GetString("PlayerID");
        int submitBoardID = leaderboardID;
        string firstSeen = EncryptedPlayerPrefs.GetString("FirstSeen", "");
        if (firstSeen == "" || string.IsNullOrEmpty(firstSeen) || string.IsNullOrWhiteSpace(firstSeen)) { EncryptedPlayerPrefs.SetString("FirstSeen", System.DateTime.Today.ToString("dd/MM/yyyy")); firstSeen = System.DateTime.Today.ToString("dd/MM/yyyy"); }
        if (levelSubmit) { submitBoardID = levelBoardID; }
        float enjoyment = Random.Range(14, 812);
        float gameKey = scoreToUpload * baseMovementScript.movementSpeed / Camera.main.aspect + ((EncryptedPlayerPrefs.GetInt("withWifiGames", 0)+3.49f)*0.35259f*(enjoyment/13.7917f)) - System.DateTime.Parse(firstSeen).Day + 4 * 2.782353f;
        enjoyment *= 5672;
        enjoyment = (int)enjoyment;
        bool webGL = false;
        if (Application.platform == RuntimePlatform.WebGLPlayer) { webGL = true; }
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, submitBoardID, string.Format("version:{0},score:{1},level:{2},mobile:{3},aspectRatio:{4},deviceType:{5},deviceModel:{6},No-Ads:{7},movementSpeed:{8},adsPlayed:{9},noWifiGames:{10},withWifiGames:{11},firstSeen:{12},hs:{13},hl:{14},os:{15},gameKey:{16},enjoyment:{17},WebGL:{18},skin:{19},skins:{20},cash:{21};", ClientVersionManager.version,scoreToUpload.ToString(), level.ToString(), Application.isMobilePlatform.ToString(), Camera.main.aspect.ToString(),SystemInfo.deviceType.ToString(), SystemInfo.deviceModel.ToString(), Utilities.IntToBool(EncryptedPlayerPrefs.GetInt("NoAds", 0)).ToString(),baseMovementScript.movementSpeed.ToString(), EncryptedPlayerPrefs.GetInt("adsPlayed", 0).ToString(), EncryptedPlayerPrefs.GetInt("noWifiGames", 0).ToString(), EncryptedPlayerPrefs.GetInt("withWifiGames", 0).ToString(), firstSeen, EncryptedPlayerPrefs.GetInt("HsMobile01"), EncryptedPlayerPrefs.GetInt("HlMobile01"),SystemInfo.operatingSystem.ToString(), gameKey.ToString(),enjoyment.ToString(),webGL.ToString(), EncryptedPlayerPrefs.GetString("skin" + EncryptedPlayerPrefs.GetInt("SelectedSkin")),EncryptedPlayerPrefs.GetInt("skinsUnlocked").ToString(),EncryptedPlayerPrefs.GetInt("Coins").ToString()), (response) =>
         {
             if (response.success)
             {
                 if (!levelSubmit) { Debug.Log("Successfully uploaded score of " + scoreToUpload.ToString() + " to database (LL)"); }
                 else { Debug.Log("Successfully uploaded level of " + scoreToUpload.ToString() + " to database (LL)"); }
                 done = true;
             }
             else
             {
                 if (!levelSubmit) { Debug.Log("Failed to upload score of " + scoreToUpload.ToString() + " to database (LL): " + response.Error); }
                 else { Debug.Log("Failed to upload level of " + scoreToUpload.ToString() + " to database (LL): " + response.Error); }
                 done = true;
             }
         });
        yield return new WaitWhile(() => done == false);
        gameKey = Random.Range(37, 21311);
    }

    public IEnumerator FetchTopHighestScoresRoutine(int start, int finish,int leaderboardType)
    {
        bool done = false;
        int loadBoardID = 0;
        switch (leaderboardType)
        {
            case 0:
                loadBoardID = leaderboardID;
                break;
            case 1:
                loadBoardID = levelBoardID;
                break;
        }
        playerNames.text = "";
        playerScores.text = "";
        for (int i = 0; i < 10; i++)
        {
            playerNames.text += "Loading. . .\n";
            if (leaderboardType == 0) playerScores.text += "####\n";
            else playerScores.text += "##\n";
        }
        print("S"+start);
        print("F"+finish);
        LootLockerSDKManager.GetScoreListMain(loadBoardID, 10, start, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "";
                string tempPlayerScores = "";

                LootLockerLeaderboardMember[] members = response.items;
                loadedMembers = members;
                playersCurrentlyDisplayed = members.Length;
                if (members.Length < 10) { finalPage = true; }
                else { finalPage = false; }

                string thisPlayerID = EncryptedPlayerPrefs.GetString("PlayerID");
                for (int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (Camera.main.aspect <= 0.5f) { tempPlayerNames += "<size=50>"; }
                    string name = "";
                    if (members[i].member_id == thisPlayerID) { name = "<color="+thisPlayerColorHex+">"; thisPlayerDisplayID = i; }
                    string nameText = "";
                    if (members[i].player.name != ""){
                        nameText = members[i].player.name;
                    }
                    else{
                        nameText = "P" + members[i].player.id;
                    }

                    int chars = 0;
                    chars += members[i].rank.ToString().Length;
                    chars += members[i].score.ToString().Length;
                    int longChars = 0;
                    longChars += nameText.Count(x => x == 'w');
                    longChars += nameText.Count(x => x == 'W');
                    longChars += nameText.Count(x => x == 'm');
                    longChars += nameText.Count(x => x == 'M');
                    longChars += (int)(nameText.Count(x => x == 'k') / 2);
                    longChars += (int)(nameText.Count(x => x == 'K') / 2);
                    longChars += (int)(nameText.Count(x => x == 'x') / 2);
                    longChars += (int)(nameText.Count(x => x == 'X') / 2);
                    longChars += (int)(nameText.Count(x => x == 'y') / 2);
                    longChars += (int)(nameText.Count(x => x == 'Y') / 2);
                    //if (longChars >= 3) { chars++; }
                    int ha = longChars;
                    if (Camera.main.aspect<= 0.44) { longChars++; }
                    if ((((float)longChars) / 4) >= 0.45f) { longChars = Mathf.CeilToInt(((float)longChars) / 4); }
                    else if (longChars != 1) { longChars = 0; }
                    chars += longChars;
                    int ones = nameText.Count(x => x == '1');
                    if ((((float)ones) / 2) > 0.51f) { ones = Mathf.CeilToInt(((float)ones) / 2); }
                    else { ones = 0; }
                    chars -= ones;
                    if (Camera.main.aspect >=0.5 && Camera.main.aspect < 0.58f) { chars--; }
                    int nameCharLimit = 8;
                    if (Camera.main.aspect >= 0.58f) { nameCharLimit = 99; }
                    else { nameCharLimit -= (Mathf.Clamp(chars - 5, 0, 99)); }
                    //<0.49 - New phones
                    //<0.58 - Old phones
                    //>0.58 - iPads
                    /*
                    int nameCharLimit = 8;
                    if (members[i].score >= 100) nameCharLimit = 7;
                    if (members[i].score >= 1000) nameCharLimit = 6;
                    if (members[i].score >= 10000) nameCharLimit = 5;
                    if (members[i].score >= 100000) nameCharLimit = 4;
                    if (members[i].score >= 1000000) nameCharLimit = 3;
                    if (members[i].score >= 10000000) nameCharLimit = 2;
                    if (members[i].score >= 100000000) nameCharLimit = 1; //Fucking hacking what
                    */
                    nameText = Utilities.TruncateString(nameText, nameCharLimit);
                    //nameText += "<color=red>" + nameCharLimit.ToString() + ha.ToString() + "</color>"; Debug info
                    name += nameText;
                    if (members[i].member_id == thisPlayerID) { name += "</color>"; }
                    tempPlayerNames += name;
                    if (leaderboardType == 1) {tempPlayerScores += (members[i].score + 2) + "\n";}
                    else {tempPlayerScores += members[i].score + "\n";}
                    if (Camera.main.aspect <= 0.5f) { tempPlayerNames += "</size>"; }
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
                cannotConnectToDB = false;
                RefreshNavigationButtonGraphics();
                noConnectionScreen.SetActive(false);
            }
            else
            {
                Debug.Log("Failed to load leaderboard (LL): " + response.Error);
                //playerNames.text = "Unable to reach database. Please check your internet connection.";
                playerNames.text = "";
                noConnectionScreen.SetActive(true);
                cannotConnectToDB = true;
                playerScores.text = "";
                RefreshNavigationButtonGraphics();
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    public IEnumerator GetCurrentPosition()
    {
        bool finished = false;
        int boardToCheck = 0;
        if (leaderboardInstanceScript.SelectedLeaderboard == 0) { boardToCheck = leaderboardID; }
        else { boardToCheck = levelBoardID; }
            LootLockerSDKManager.GetMemberRank(boardToCheck, EncryptedPlayerPrefs.GetString("PlayerID"), (response) =>
        {
            if (response.success)
            {
                if (leaderboardInstanceScript.SelectedLeaderboard == 0){
                    if (response.rank != 0) { yourRankText.text = "Your Rank: #" + response.rank; }
                    else { 
                        if (Application.platform == RuntimePlatform.WebGLPlayer){
                            yourRankText.text = "Score over 150\nto rank!";
                        }
                        else{
                            yourRankText.text = "Play a game\nto rank!";
                        }
                    }
                }
                else if (leaderboardInstanceScript.SelectedLeaderboard == 1){
                    if (response.rank != 0 && response.score >= 3) { yourRankText.text = "Your Rank: #" + response.rank; }
                    else { yourRankText.text = "Reach level 5\nto rank!"; }
                }
                finished = true;
            }
            else
            {
                Debug.Log("Failed to get current leaderboard position (LL): " + response.Error);
                yourRankText.text = "";
                finished = true;
            }
        });
        yield return new WaitWhile(() => finished == false);
    }
    public void RefreshLeaderboard()
    {
        if (SceneManager.GetActiveScene().name != "Main")
        {
            StartCoroutine(FetchTopHighestScoresRoutine(page * 10, (page * 10) + 10, leaderboardInstanceScript.SelectedLeaderboard));
            StartCoroutine(GetCurrentPosition());
        }
    }
    public void NavigateNextPage()
    {
        if (!finalPage&&SceneManager.GetActiveScene().name== "Leaderboard"){
            navigateSFX.Play(true, 1.15f);
            page++;
            HapticFeedbackPlayer.Play(0);
            RefreshLeaderboard();
        }
    }
    public void NavigatePreviousPage()
    {
        if (page > 0){
            navigateSFX.Play(true, 0.85f);
            page--;
            HapticFeedbackPlayer.Play(0);
            RefreshLeaderboard();
        }
    }
    void RefreshNavigationButtonGraphics()
    {
        if (page > 0 && !cannotConnectToDB) { upArrow.color = new Color(upArrow.color.r, upArrow.color.g, upArrow.color.b, 1f); }
        else { upArrow.color = new Color(upArrow.color.r, upArrow.color.g, upArrow.color.b, 0.2f); }
        if (finalPage || cannotConnectToDB) { downArrow.color = new Color(downArrow.color.r, downArrow.color.g, downArrow.color.b, 0.2f); }
        else { downArrow.color = new Color(downArrow.color.r, downArrow.color.g, downArrow.color.b, 1f); }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            NavigateNextPage();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
            NavigatePreviousPage();
        }
        else if (Input.GetKeyDown(KeyCode.F5)){
            RefreshLeaderboard();
        }
    }
}
