using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class OutdatedClientCheck : MonoBehaviour
{
    int leaderboardID = 16556;
    int upToDateVersionNum;
    public string serverDataString;
    bool checkedClientVersion;
    public static bool outdatedClient;
    bool startedCheck;

    public TextMeshProUGUI outdatedClientText;
    private void Start()
    {
        outdatedClientText.color = Utilities.Invisible(outdatedClientText.color);
    }
    public IEnumerator GetServerData()
    {
        bool done = false;
        print(-1);
        LootLockerSDKManager.GetScoreListMain(leaderboardID, 10, 0, (response) =>
        {
            print(0);
            if (response.success)
            {
                print(1);
                LootLockerLeaderboardMember[] members = response.items;
                for (int i = 0; i < members.Length; i++)
                {
                    print(2);
                    if (members[i].metadata.Contains("data:Main"))
                    {
                        print(3);
                        serverDataString = members[i].metadata;
                        print("Obtained server data: " + serverDataString);
                    }
                }
                done = true;
            }
            else
            {
                Debug.Log("Failed to fetch server data (LL): " + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
    private void Update()
    {
        if (PlayerManager_LL.currentlyLoggedIn && !startedCheck) { startedCheck = true; StartCoroutine(GetServerData()); }
        if (!checkedClientVersion){
            if (serverDataString != ""){
                upToDateVersionNum = int.Parse(Utilities.StringBetweenTwoStrings(serverDataString, "versionNum:", ",announc"));
                if (ClientVersionManager.versionNumber < upToDateVersionNum){
                    outdatedClient = true;
                    Debug.LogError("Client outdated!");
                }
                else { print("Client up to date."); }
                checkedClientVersion = true;
            }
        }
        if (outdatedClient) { outdatedClientText.color = new Color(outdatedClientText.color.r, outdatedClientText.color.g, outdatedClientText.color.b, Mathf.Lerp(outdatedClientText.color.a, 1, Time.deltaTime * 3)); }
    }
}
