using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
public class MainMenu : MonoBehaviour
{
    public Animator instanceTransitionAnimator;

    public ParticleSystem[] startButtonParticles;
    public ButtonBob startButtonBob;
    public ParticleSystem[] exitButtonParticles;
    public ButtonBob exitButtonBob;
    public ParticleSystem[] settingsButtonParticles;
    public ButtonBob settingsButtonBob;
    public ParticleSystem[] leaderboardButtonParticles;
    public ButtonBob leaderboardButtonBob;
    public ParticleSystem[] noAdsButtonParticles;
    public ButtonBob noAdsButtonBob;

    int selectedButton;
    float selectTimeCooldown;

    bool usingMouse;

    public PlaySound startSFX;
    public PlaySound exitSFX;
    public PlaySound navigateSFX;
    int previousSelect;

    public TextMeshProUGUI highestScoreText;

    public RectTransform startButton;
    public RectTransform settingsButton;
    public Transform exitButton;

    public GameObject activatedDebugGraphic;
    public GameObject activatedPhoneResGraphic;
    public GameObject toggledInputType;

    public PlaySound letterClickSFX;

    public RectTransform buttons;

    bool noAdsScreenOpen;
    public RectTransform noAdsScreen;
    float noAdsScreenVelocity;
    float noAdsScreenStartVelocity;
    float noAdsScreenStartX;
    bool exitingNoAdsScreen;
    public PlaySound noAdsScreenOpenStartSFX;
    public PlaySound noAdsScreenOpenEndSFX;
    public PlaySound noAdsScreenCloseSFX;
    public RectTransform centrePoint;
    bool playedNoAdsScreenOpenEndSFX;
    public RectTransform trophiesTransform;

    public MainMenuCharacter mainMenuCharacterScript;

    public GameObject hiddenTitle;

    public GameObject customizeButton;
    private void Start()
    {
        HapticFeedbackPlayer.Refresh();

        GifMaterial.col = Color.white;
        GifMaterial.fpsMultiplier = 1;

        if (PlayerPrefs.GetInt("CoinN",0) == 0) { customizeButton.SetActive(false); }

        noAdsScreen.gameObject.SetActive(false);

        if (Application.isMobilePlatform||Application.isEditor)
        {
            if (EncryptedPlayerPrefs.GetInt("NoAds") == 0&&EncryptedPlayerPrefs.GetInt("HsMobile01", 0)>0) { 
                buttons.anchoredPosition += new Vector2(0, -40); 
            }
            else {
                buttons.anchoredPosition += new Vector2(0, -80);
                noAdsButtonBob.gameObject.SetActive(false); 
            }
            exitButton.gameObject.SetActive(false);
        }
        else
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer){
                buttons.anchoredPosition += new Vector2(0, -80);
                exitButton.gameObject.SetActive(false);
            }
            else{
                buttons.anchoredPosition += new Vector2(0, -40);
                exitButton.position = noAdsButtonBob.transform.position;
            }
            noAdsButtonBob.gameObject.SetActive(false);
        }

        previousSelect = -1;

        if (Utilities.IntToBool(PlayerPrefs.GetInt("FpsCounter", 0))){
            activatedDebugGraphic.SetActive(true);
        }
        else{
            activatedDebugGraphic.SetActive(false);
        }
        /*
        if (Utilities.IntToBool(PlayerPrefs.GetInt("PhoneResolution", 0))){
            if (!Application.isMobilePlatform)
            {
                //activatedPhoneResGraphic.SetActive(true);
                Resolution.setResolution = false;
                Cache.isMobile = true;
                RefreshHighestScore();
            }
        }
        else{
            activatedPhoneResGraphic.SetActive(false);
        }
        */
        activatedPhoneResGraphic.SetActive(false);
        /*
        if (PlayerPrefs.GetString("InputType", "Swipe") == "Hold")
        {
            toggledInputType.SetActive(true);
        }
        else
        {
            toggledInputType.SetActive(false);
        }
        InputManager.mobileMoveSystem = PlayerPrefs.GetString("InputType","Swipe");
        */
        //if (PlayerPrefs.GetString("InputType") == "" || !PlayerPrefs.HasKey("InputType")) { PlayerPrefs.SetString("InputType", "Swipe"); }

        RefreshHighestScore();
        selectedButton = -1;
        StartCoroutine(PlayButtonHighlightDelay());
    }
    IEnumerator PlayButtonHighlightDelay()
    {
        yield return new WaitForSeconds(1);
        selectedButton = 0;
    }
    private void Update()
    {
        if (!Terminal.terminalActive)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) { selectedButton = -1; usingMouse = true; }
            if (Input.anyKeyDown) usingMouse = false;

            if (usingMouse)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "StartSelect")
                    {
                        selectedButton = 0;
                    }
                    else if (hit.transform.name == "ExitSelect")
                    {
                        selectedButton = 3;
                    }
                    else if (hit.transform.name == "NoAdsSelect")
                    {
                        selectedButton = 4;
                    }
                    else if (hit.transform.name == "SettingsSelect")
                    {
                        selectedButton = 2;
                    }
                    else if (hit.transform.name == "LeaderboardSelect")
                    {
                        selectedButton = 1;
                    }
                    else
                    {
                        selectedButton = -1;
                    }
                }
                else
                {
                    selectedButton = -1;
                }
            }
            else
            {
                selectTimeCooldown -= Time.deltaTime;
                if (selectTimeCooldown <= 0)
                {
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        selectedButton--;
                        selectedButton = Mathf.Clamp(selectedButton, 0, 3);
                        selectTimeCooldown = 0.5f;
                    }
                    else if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        selectedButton++;
                        selectedButton = Mathf.Clamp(selectedButton, 0, 3);
                        selectTimeCooldown = 0.5f;
                    }
                }
                if (Input.GetAxisRaw("Vertical") == 0) selectTimeCooldown = 0;
            }

            switch (selectedButton)
            {
                case 0:
                    for (int i = 0; i < startButtonParticles.Length; i++)
                    {
                        if (!startButtonParticles[i].isPlaying) startButtonParticles[i].Play();
                        startButtonBob.defaultScale = new Vector2(1.2f, 1.2f);
                        DisableParticles(startButtonParticles, startButtonBob);
                        if (previousSelect != 0) { navigateSFX.Play(); previousSelect = 0; }
                        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return)) { Play(); startButtonBob.Bob(); }
                    }
                    break;
                case 3:
                    for (int i = 0; i < exitButtonParticles.Length; i++)
                    {
                        if (!exitButtonParticles[i].isPlaying) exitButtonParticles[i].Play();
                        exitButtonBob.defaultScale = new Vector2(1.2f, 1.2f);
                        DisableParticles(exitButtonParticles, exitButtonBob);
                        if (previousSelect != 2) { navigateSFX.Play(); previousSelect = 2; }
                        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return)) { Exit(); exitButtonBob.Bob(); }
                    }
                    break;
                case 4:
                    for (int i = 0; i < noAdsButtonParticles.Length; i++)
                    {
                        if (!noAdsButtonParticles[i].isPlaying) noAdsButtonParticles[i].Play();
                        noAdsButtonBob.defaultScale = new Vector2(1.2f, 1.2f);
                        DisableParticles(noAdsButtonParticles, noAdsButtonBob);
                        if (previousSelect != 2) { navigateSFX.Play(); previousSelect = 2; }
                        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return)) { Exit(); noAdsButtonBob.Bob(); }
                    }
                    break;
                case 2:
                    for (int i = 0; i < settingsButtonParticles.Length; i++)
                    {
                        if (!settingsButtonParticles[i].isPlaying) settingsButtonParticles[i].Play();
                        settingsButtonBob.defaultScale = new Vector2(1.2f, 1.2f);
                        DisableParticles(settingsButtonParticles, settingsButtonBob);
                        if (previousSelect != 1) { navigateSFX.Play(); previousSelect = 1; }
                        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return)) { Settings(); settingsButtonBob.Bob(); }
                    }
                    break;
                case 1:
                    for (int i = 0; i < leaderboardButtonParticles.Length; i++)
                    {
                        if (!leaderboardButtonParticles[i].isPlaying) leaderboardButtonParticles[i].Play();
                        leaderboardButtonBob.defaultScale = new Vector2(1.2f, 1.2f);
                        DisableParticles(leaderboardButtonParticles, leaderboardButtonBob);
                        if (previousSelect != 1) { navigateSFX.Play(); previousSelect = 1; }
                        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return)) { Leaderboard(); leaderboardButtonBob.Bob(); }
                    }
                    break;
                default:
                    previousSelect = -1;
                    DisableParticles(null, null);
                    break;
            }

            //No Ads Screen
            if (noAdsScreen.anchoredPosition.x < noAdsScreenStartX) { noAdsScreenStartX = noAdsScreen.anchoredPosition.x; }
            if (noAdsScreenOpen)
            {
                if (exitingNoAdsScreen)
                {
                    trophiesTransform.localScale = Vector3.Lerp(trophiesTransform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 3);
                    noAdsScreenVelocity = -1000;
                    noAdsScreen.anchoredPosition += new Vector2(noAdsScreenVelocity * Time.deltaTime, 0);
                    if (noAdsScreen.anchoredPosition.x < noAdsScreenStartX)
                    {
                        noAdsScreen.gameObject.SetActive(false);
                        exitingNoAdsScreen = false;
                        noAdsScreenOpen = false;
                    }
                }
                else
                {
                    trophiesTransform.localScale = Vector3.Lerp(trophiesTransform.localScale, new Vector3(0, 0, 0), Time.deltaTime * 5);
                    noAdsScreen.gameObject.SetActive(true);
                    noAdsScreen.anchoredPosition += new Vector2(noAdsScreenVelocity * Time.deltaTime, 0);
                    noAdsScreenVelocity += Time.deltaTime * 250;
                    if (noAdsScreen.position.x >= centrePoint.position.x + 2.675f)
                    {
                        if (!playedNoAdsScreenOpenEndSFX)
                        {
                            noAdsScreenOpenEndSFX.Play();
                            playedNoAdsScreenOpenEndSFX = true;
                        }
                        noAdsScreenStartVelocity *= 0.25f;
                        noAdsScreenVelocity = noAdsScreenStartVelocity;
                        noAdsScreen.position = new Vector2((centrePoint.position.x + 2.675f) - 0.001f, noAdsScreen.position.y);
                    }
                }
            }
            else
            {
                noAdsScreen.gameObject.SetActive(false);
                trophiesTransform.localScale = Vector3.Lerp(trophiesTransform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 3);
            }
        }
    }
    void DisableParticles(ParticleSystem[] selected, ButtonBob buttonBobScript)
    {
        if (selected != startButtonParticles) for (int i = 0; i < startButtonParticles.Length; i++) { startButtonParticles[i].Stop(); }
        if (buttonBobScript != startButtonBob) startButtonBob.defaultScale = new Vector2(1, 1);
        if (selected != exitButtonParticles) for (int i = 0; i < exitButtonParticles.Length; i++) { exitButtonParticles[i].Stop(); }
        if (buttonBobScript != exitButtonBob) exitButtonBob.defaultScale = new Vector2(1, 1);
        if (selected != settingsButtonParticles) for (int i = 0; i < settingsButtonParticles.Length; i++) { settingsButtonParticles[i].Stop(); }
        if (buttonBobScript != settingsButtonBob) settingsButtonBob.defaultScale = new Vector2(1, 1);
        if (selected != leaderboardButtonParticles) for (int i = 0; i < leaderboardButtonParticles.Length; i++) { leaderboardButtonParticles[i].Stop(); }
        if (buttonBobScript != leaderboardButtonBob) leaderboardButtonBob.defaultScale = new Vector2(1, 1);
        if (selected != noAdsButtonParticles) for (int i = 0; i < noAdsButtonParticles.Length; i++) { noAdsButtonParticles[i].Stop(); }
        if (buttonBobScript != noAdsButtonBob) noAdsButtonBob.defaultScale = new Vector2(1, 1);
    }
    public void Play()
    {
        if (!noAdsScreenOpen || exitingNoAdsScreen)
        {
            if (OutdatedClientCheck.outdatedClient&&Application.platform!=RuntimePlatform.WebGLPlayer) { InstanceChange.LoadInstance("OutdatedVersion", 1); }
            else { InstanceChange.LoadInstance("Main", 1); }
            mainMenuCharacterScript.Play();
            startSFX.Play();
            HapticFeedbackPlayer.Play(0);
        }
    }
    public void Settings()
    {
        if (!noAdsScreenOpen || exitingNoAdsScreen)
        {
            InstanceChange.LoadInstance("Settings", 1);
            startSFX.Play();
            HapticFeedbackPlayer.Play(0);
        }
    }
    public void Leaderboard()
    {
        if (!noAdsScreenOpen || exitingNoAdsScreen)
        {
            InstanceChange.LoadInstance("Leaderboard", 1);
            startSFX.Play();
            HapticFeedbackPlayer.Play(0);
        }
    }
    public void Customize()
    {
        if (!noAdsScreenOpen || exitingNoAdsScreen)
        {
            InstanceChange.LoadInstance("Customize", 1);
            startSFX.Play();
            HapticFeedbackPlayer.Play(0);
        }
    }
    public void NoAds()
    {
        if (!noAdsScreenOpen || exitingNoAdsScreen)
        {
            noAdsScreenOpen = true;
            noAdsScreenVelocity = 500;
            noAdsScreenStartVelocity = -noAdsScreenVelocity;
            playedNoAdsScreenOpenEndSFX = false;
            noAdsScreenOpenStartSFX.Play();
            HapticFeedbackPlayer.Play(0);
        }
    }
    public void ExitNoAdsScreen()
    {
        exitingNoAdsScreen = true;
        noAdsScreenCloseSFX.Play();
        HapticFeedbackPlayer.Play(0);
    }
    public void Exit()
    {
        InstanceChange.ExitApplication(1);
        exitSFX.Play();
        HapticFeedbackPlayer.Play(0);
    }
    public void ActivateDebug(bool haptics)
    {
        PlayerPrefs.SetInt("FpsCounter", Utilities.BoolToInt(!Utilities.IntToBool(PlayerPrefs.GetInt("FpsCounter"))));
        activatedDebugGraphic.SetActive(Utilities.IntToBool(PlayerPrefs.GetInt("FpsCounter", 0)));
        if (haptics) { StartCoroutine(DebugHaptics(Utilities.IntToBool(PlayerPrefs.GetInt("FpsCounter")))); }
        letterClickSFX.Play(true, 1 + ((float)PlayerPrefs.GetInt("FpsCounter") / 3));
    }
    public void ActivatePhoneResolution() //Deprecated
    {
        if (!Application.isMobilePlatform){
            PlayerPrefs.SetInt("PhoneResolution", Utilities.BoolToInt(!Utilities.IntToBool(PlayerPrefs.GetInt("PhoneResolution"))));
            activatedPhoneResGraphic.SetActive(Utilities.IntToBool(PlayerPrefs.GetInt("PhoneResolution", 0)));
            Resolution.setResolution = false;
            Cache.isMobile = Utilities.IntToBool(PlayerPrefs.GetInt("PhoneResolution"));
            RefreshHighestScore();
            letterClickSFX.Play(true, 1 + ((float)PlayerPrefs.GetInt("PhoneResolution") / 3));
        }
    }
    /*
    public void ToggleInputType()
    {
        if (Application.isMobilePlatform)
        {
            if (PlayerPrefs.GetString("InputType") == "Hold")
            {
                PlayerPrefs.SetString("InputType", "Swipe");
                toggledInputType.SetActive(false);
            }
            else if (PlayerPrefs.GetString("InputType") == "Swipe")
            {
                PlayerPrefs.SetString("InputType", "Hold");
                toggledInputType.SetActive(true);
            }
            InputManager.mobileMoveSystem = PlayerPrefs.GetString("InputType", "Swipe");
        }
    }
    */
    void RefreshHighestScore()
    {
        if (Cache.isMobile||1==1) //Deprecated. Keep hard coded pass
        {
            int highestScore = EncryptedPlayerPrefs.GetInt("HsMobile01", 0);
            print("Saved mobile highest score: " + highestScore);
            if (highestScore != 0){
                highestScoreText.transform.parent.gameObject.SetActive(true);
                highestScoreText.text = highestScore.ToString();
                hiddenTitle.SetActive(false);
            }
            else{
                highestScoreText.transform.parent.gameObject.SetActive(false);
                hiddenTitle.SetActive(true);
            }
        }
        else
        {
            int highestScore = EncryptedPlayerPrefs.GetInt("HsPc01", 0);
            print("Saved PC highest score: " + highestScore);
            if (highestScore != 0){
                highestScoreText.transform.parent.gameObject.SetActive(true);
                highestScoreText.text = highestScore.ToString();
            }
            else{
                highestScoreText.transform.parent.gameObject.SetActive(false);
            }
        }
    }
    IEnumerator DebugHaptics(bool activated)
    {
        if (activated)
        {
            HapticFeedbackPlayer.Play(0);
            yield return new WaitForSeconds(0.05f);
            HapticFeedbackPlayer.Play(1);
            yield return new WaitForSeconds(0.05f);
            HapticFeedbackPlayer.Play(2);
        }
        else
        {
            HapticFeedbackPlayer.Play(2);
            yield return new WaitForSeconds(0.05f);
            HapticFeedbackPlayer.Play(1);
            yield return new WaitForSeconds(0.05f);
            HapticFeedbackPlayer.Play(0);
        }
    }
}
