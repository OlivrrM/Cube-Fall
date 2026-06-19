using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class PlayerProfileManager : MonoBehaviour
{
    public CinemachineVirtualCamera leaderboardCamera;
    public LeaderboardManager_LL leaderboardScript;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI platformText;
    public TextMeshProUGUI firstSeenText;
    public TextMeshProUGUI highestScoreText;
    public TextMeshProUGUI highestLevelText;
    public Color thisPlayerColour;
    public Color unknownColour;
    public Color knownColour;
    public Image osImage;
    public Sprite[] osSprites;
    public Sprite noneImage;
    bool platformTextHighlighted;

    public Skins skinsScript;
    public TextMeshProUGUI skinsText;
    public TextMeshProUGUI coinsText;
    public SpriteRenderer[] characterSpriteRenderers;
    public Sprite unknownSkinSprite;
    public GameObject[] trophies;

    public PlaySound navigateSFX;

    public GameObject infoPage0;
    public GameObject infoPage1;

    public Material staticMaterial;
    public Image switchImageIcon;

    public AudioSource staticSfx;

    string[] skinStringValues = new string[2];
    private void Start()
    {
        ClearLoadedProfile();
    }
    public void SelectProfile(int selectedID)
    {
        navigateSFX.Play(true, 1);
        HapticFeedbackPlayer.Play(0);
        leaderboardCamera.Priority = 5;
        if (string.IsNullOrEmpty(leaderboardScript.loadedMembers[selectedID - 1].player.name)){
            playerName.text = Utilities.TruncateString("P" + leaderboardScript.loadedMembers[selectedID - 1].member_id,8);
        }
        else{
            playerName.text = leaderboardScript.loadedMembers[selectedID - 1].player.name;
        }
        try { platformText.text = Utilities.StringBetweenTwoStrings(leaderboardScript.loadedMembers[selectedID - 1].metadata, "deviceType:", ",deviceModel"); platformText.color = knownColour; }
        catch { platformText.text = "Unknown"; platformText.color = unknownColour; }
        try {
            string date = Utilities.StringBetweenTwoStrings(leaderboardScript.loadedMembers[selectedID - 1].metadata, "firstSeen:", ",hs");
            if (!string.IsNullOrEmpty(date) && !string.IsNullOrWhiteSpace(date) && date != ""){
                firstSeenText.text = date;
                firstSeenText.color = knownColour;
            }
            else{
                firstSeenText.text = "Unknown"; firstSeenText.color = unknownColour;
            }
        }
        catch { firstSeenText.text = "Unknown"; firstSeenText.color = unknownColour; }
        try { highestScoreText.text = Utilities.StringBetweenTwoStrings(leaderboardScript.loadedMembers[selectedID - 1].metadata, "hs:", ",hl:");highestScoreText.color = knownColour; }
        catch { highestScoreText.text = "Unknown"; highestScoreText.color = unknownColour; }
        try {
            string value = (int.Parse(Utilities.StringBetweenTwoStrings(leaderboardScript.loadedMembers[selectedID - 1].metadata, "hl:", ",os"))+2).ToString();
            if (int.Parse(value) <= 0) { highestLevelText.text = "---"; }
            else { highestLevelText.text = value; }
            highestLevelText.color = knownColour;
        }
        catch { highestLevelText.text = "Unknown"; highestLevelText.color = unknownColour; }
        try
        {
            string os = Utilities.StringBetweenTwoStrings(leaderboardScript.loadedMembers[selectedID - 1].metadata, "os:", ",gameKey");
            string webGlCheck = "";
            try{
                webGlCheck = Utilities.StringBetweenTwoStrings(leaderboardScript.loadedMembers[selectedID - 1].metadata, "WebGL:", ",skin");
            }
            catch{
                webGlCheck = "False";
            }
            if (os.Contains("Mozilla")|| os.Contains("HTML")|| os.Contains("Chrome")|| os.Contains("Safari") || os.Contains("AppleWebKit")|| webGlCheck == "True") { osImage.sprite = osSprites[6]; }
            else if (os.Contains("Windows")) { osImage.sprite = osSprites[0]; }
            else if (os.Contains("Mac")) { osImage.sprite = osSprites[1]; }
            else if (os.Contains("iPhone") || os.Contains("iOS") || os.Contains("iPad")) { osImage.sprite = osSprites[2]; }
            else if (os.Contains("Android")) { osImage.sprite = osSprites[3]; }
            else if (os.Contains("Steam")) { osImage.sprite = osSprites[4]; }
            else if (os.Contains("Linux")) { osImage.sprite = osSprites[5]; }
            else { osImage.sprite = noneImage; }
        }
        catch{ osImage.sprite = noneImage; }
        if ((selectedID - 1) == leaderboardScript.thisPlayerDisplayID) { playerName.color = thisPlayerColour; }
        else { playerName.color = knownColour; }
        try { skinsText.text = (int.Parse(Utilities.StringBetweenTwoStrings(leaderboardScript.loadedMembers[selectedID - 1].metadata, "skins:", ",cash"))+1).ToString(); skinsText.color = knownColour; }
        catch { skinsText.text = "Unknown"; skinsText.color = unknownColour; }
        try {
            print(leaderboardScript.loadedMembers[selectedID - 1].metadata); 
            coinsText.text = Utilities.StringBetweenTwoStrings(leaderboardScript.loadedMembers[selectedID - 1].metadata, "cash:", ";"); coinsText.color = knownColour; }
        catch { coinsText.text = "Unknown"; coinsText.color = unknownColour; }
        try {
            string skinString = Utilities.StringBetweenTwoStrings(leaderboardScript.loadedMembers[selectedID - 1].metadata, "skin:", ",skins");
            skinStringValues = skinString.Split('-');
            for (int i = 0; i < characterSpriteRenderers.Length; i++){
                if (i == 0) { characterSpriteRenderers[i].sprite = skinsScript.graphics[int.Parse(skinStringValues[0])]; }
                else { characterSpriteRenderers[i].sprite = skinsScript.backGraphics[int.Parse(skinStringValues[0])]; }
                characterSpriteRenderers[i].color = new Color(skinsScript.colours[int.Parse(skinStringValues[1])].r, skinsScript.colours[int.Parse(skinStringValues[1])].g, skinsScript.colours[int.Parse(skinStringValues[1])].b,characterSpriteRenderers[i].color.a);
                if (MainMenuScreenManager.selectedScreenID > 0)
                {
                    if (characterSpriteRenderers[i].color!=new Color(Color.white.r, Color.white.g, Color.white.b,characterSpriteRenderers[i].color.a)){
                        characterSpriteRenderers[i].color = Utilities.DarkenColByPercent(Utilities.InvertHue(characterSpriteRenderers[i].color), 20);
                    }
                    else{
                        characterSpriteRenderers[i].color = Utilities.InvertCol(characterSpriteRenderers[i].color);
                    }
                }
            }
        }
        catch { 
            if (MainMenuScreenManager.selectedScreenID > 0){
                for (int i = 0; i < characterSpriteRenderers.Length; i++){
                    if (i == 0) { characterSpriteRenderers[i].sprite = skinsScript.graphics[0]; }
                    else { characterSpriteRenderers[i].sprite = skinsScript.backGraphics[0]; }
                    characterSpriteRenderers[i].color = new Color(Color.black.r, Color.black.g, Color.black.b,characterSpriteRenderers[i].color.a);
                }
            }
            else{
                for (int i = 0; i < characterSpriteRenderers.Length; i++){
                    if (i == 0) { characterSpriteRenderers[i].sprite = skinsScript.graphics[0]; }
                    else { characterSpriteRenderers[i].sprite = skinsScript.backGraphics[0]; }
                    characterSpriteRenderers[i].color = new Color(Color.white.r, Color.white.g, Color.white.b, characterSpriteRenderers[i].color.a);
                }
            }
            for (int i = 0; i < skinStringValues.Length; i++){skinStringValues[i] = "0";}
        }
        try{
            for (int i = 0; i < trophies.Length; i++){trophies[i].SetActive(false); trophies[i].GetComponent<RectTransform>().anchoredPosition = new Vector2((i - 1) * 100,0); }
            int hl = int.Parse(Utilities.StringBetweenTwoStrings(leaderboardScript.loadedMembers[selectedID - 1].metadata, "hl:", ",os"));
            if (hl >= 56){for (int i = 0; i < trophies.Length; i++){trophies[i].SetActive(true);} }
            else if (hl >= 12) {
                trophies[1].SetActive(true); trophies[0].SetActive(true);
                trophies[0].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 0);
                trophies[1].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);
            }
            else if (hl >= 7) {
                trophies[0].SetActive(true);
                trophies[0].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }
        }
        catch{
            for (int i = 0; i < trophies.Length; i++) { trophies[i].SetActive(false); }
        }
    }
    public void SwitchPage()
    {
        infoPage0.SetActive(!infoPage0.active);
        infoPage1.SetActive(!infoPage1.active);
        staticMaterial.color = new Color(staticMaterial.color.r, staticMaterial.color.g, staticMaterial.color.b, 1);
        staticSfx.volume = 0.01f;
        HapticFeedbackPlayer.Play(0);
    }
    public void BackToLeaderboard()
    {
        navigateSFX.Play(true, 0.8f);
        leaderboardCamera.Priority = 15;
        StartCoroutine(ClearProfileDelay());
        HapticFeedbackPlayer.Play(0);
    }
    IEnumerator ClearProfileDelay()
    {
        yield return new WaitForSeconds(0.5f);
        ClearLoadedProfile();
    }
    void ClearLoadedProfile()
    {
        platformText.text = "Unknown";
        firstSeenText.text = "Unknown";
        highestScoreText.text = "Unknown";
        highestLevelText.text = "Unknown";
        infoPage0.SetActive(true);
        infoPage1.SetActive(false);
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){
            if (hit.transform.name == "playerPlatform"){
                platformTextHighlighted = true;
            }
            else{
                platformTextHighlighted = false;
            }
        }
        else
        {
            platformTextHighlighted = false;
        }
        if (platformTextHighlighted && osImage.sprite != noneImage) {
            osImage.color = Color.Lerp(osImage.color, new Color(osImage.color.r, osImage.color.g, osImage.color.b, 1), Time.deltaTime * 5);
            platformText.rectTransform.anchoredPosition = Vector2.Lerp(platformText.rectTransform.anchoredPosition, new Vector2(platformText.rectTransform.anchoredPosition.x, 230), Time.deltaTime * 5);
        }
        else{
            osImage.color = Color.Lerp(osImage.color, new Color(osImage.color.r, osImage.color.g, osImage.color.b, 0), Time.deltaTime * 15);
            platformText.rectTransform.anchoredPosition = Vector2.Lerp(platformText.rectTransform.anchoredPosition, new Vector2(platformText.rectTransform.anchoredPosition.x, 200), Time.deltaTime * 5);
        }

        staticMaterial.color = Color.Lerp(staticMaterial.color, Utilities.Invisible(staticMaterial.color), Time.deltaTime * 3);
        switchImageIcon.rectTransform.localRotation = Quaternion.Lerp(switchImageIcon.rectTransform.localRotation, Quaternion.Euler(new Vector3(0, 0, Utilities.BoolToInt(infoPage0.active) * -180)), Time.deltaTime * 10);
        staticSfx.volume = Mathf.Lerp(staticSfx.volume, 0, Time.deltaTime*4f);
        if (skinStringValues.Length >= 3)
        {
            if (skinStringValues[2] == "61")
            {
                for (int i = 0; i < characterSpriteRenderers.Length; i++)
                {
                    float ogA = characterSpriteRenderers[i].color.a;
                    characterSpriteRenderers[i].color = Utilities.GetRainbowColor(0.05f);
                    characterSpriteRenderers[i].color = new Color(characterSpriteRenderers[i].color.r, characterSpriteRenderers[i].color.g, characterSpriteRenderers[i].color.b, ogA);
                }
            }
        }
    }
}
