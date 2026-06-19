using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMLeaderboard : MonoBehaviour
{
    public PlayerProfileManager playerProfileManagerScript;
    public LeaderboardManager_LL leaderboardManagerScript;
    public Color[] rankPosColors;
    public Color[] unknownTextColors;
    public Color[] knownTextColors;
    public string[] thisPlayerColoursHex;
    int selectedScreen;

    public Image[] playerHighlightImages;
    public LeaderboardPlayerSelect[] leaderboardPlayerSelectScripts;
    public Color[] selectPlayerColors;
    public Color[] selectThisPlayerColors;
    private void Start()
    {
        selectedScreen = EncryptedPlayerPrefs.GetInt("SelectedScreenID", 0);
        playerProfileManagerScript.thisPlayerColour = rankPosColors[selectedScreen];
        playerProfileManagerScript.unknownColour = unknownTextColors[selectedScreen];
        playerProfileManagerScript.knownColour = knownTextColors[selectedScreen];
        leaderboardManagerScript.thisPlayerColorHex = thisPlayerColoursHex[selectedScreen];
        for (int i = 0; i < playerHighlightImages.Length; i++){
            playerHighlightImages[i].color = selectPlayerColors[selectedScreen];
        }
        for (int i = 0; i < leaderboardPlayerSelectScripts.Length; i++){
            leaderboardPlayerSelectScripts[i].thisPlayerColour = selectThisPlayerColors[selectedScreen];
        }
    }
}
