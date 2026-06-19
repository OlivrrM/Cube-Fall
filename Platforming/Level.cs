using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Cinemachine;
//using Interhaptics.Internal;

public class Level : MonoBehaviour
{
    public SpriteRenderer playerSpriteRenderer;
    public Transform passLevelY;
    public Transform GameOverY;
    public static bool gameOver;
    public static bool death;
    public static bool levelComplete;
    public Transform deathPostFX;
    public PlatformSpawn platformSpawnScript;
    public CameraMove cameraMoveScript;
    public TextMeshProUGUI gameOverText;

    public Color[] levelColours;

    [HideInInspector]public float levelStartTime;

    [HideInInspector]public int level;
    [HideInInspector]public int levelAntiCheatMultiplier;

    public Image rainbow;

    public float levelSpeedIncrease;
    public float pcLevelSpeedIncrease;
    public float rainbowSpeedIncreaseDecrease;
    float levelCompleteMultiplierOnLevelComplete;

    public CinemachineVirtualCamera cam1;
    CinemachineBasicMultiChannelPerlin cam1Noise;

    public PlaySound deathSFX;
    public PlaySound transitionSFX;
    float transitionSfxPitch;

    public static int easyPlatforms;
    public static int mediumPlatforms;

    public TextMeshProUGUI newHighestScoreText;
    bool newHighestScore;
    bool newHighestLevel;

    float timeAfterDeath;

    //public EventHapticSource deathHaptic;

    public Image homeButton;
    bool hideHomeButton;

    public Transform backButton;
    public Transform fpsCounter;

    bool moveRainbowText;
    public TextMeshProUGUI rainbowIndexText;

    public InvertColoursEvent invertColoursEventScript;
    public HellEvent hellEventScript;

    int invertLevel;
    int hellLevel;

    public LeaderboardManager_LL leaderboardManagerScript;

    public AdsPlayer adsPlayerScript;

    bool hsCheckDone;
    bool hlCheckDone;
    private void Awake()
    {
        PlatformObjectManager.scoreThisPlatform = 0;
        PlatformObjectManager.shownHsMarker = false;
        PlatformPass.scoreIncreaseBonus = 0;
    }
    private void Start()
    {
        //CoinsSpawn.platformsSinceSpawn = 0;
        //CoinsSpawn.platformsRequiredForNextCoin = 404;
        CoinsSpawn.platformsSinceSpawn = EncryptedPlayerPrefs.GetInt("platformsSinceSpawn", 25);

        levelAntiCheatMultiplier = Random.Range(99, 9999);
        invertLevel = 12;
        hellLevel = 56;
        /*
        if (Screen.width > Screen.height){
            levelSpeedIncrease = pcLevelSpeedIncrease;
            print("(Level.cs) Set levelSpeedIncrease to pcLevelSpeedIncrease because PC aspect ratio was detected");
            invertLevel += 5;
        }
        */
        cam1Noise = cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        GifMaterial.col = Color.white;
        GifMaterial.fpsMultiplier = 1;
        level = -1*levelAntiCheatMultiplier;
        death = false;
        gameOver = false;
        cameraMoveScript.levelCompleteMultiplier = 1;
        transitionSfxPitch = 1.3f;
        easyPlatforms = 0;
        mediumPlatforms = 0;
        StartCoroutine(HideHomeButton());
        if (PlayerPrefs.GetInt("FpsCounter",0) == 1)
        {
            fpsCounter.gameObject.SetActive(true);
        }
        else
        {
            fpsCounter.gameObject.SetActive(false);
        }
        cameraMoveScript.firstLevelMultiplier = 0.85f;
    }
    private void Update()
    {
        backButton.position = new Vector3(-CameraBoundaries.boundaries.x + 2.5f, backButton.position.y, backButton.position.z);
        if (fpsCounter.gameObject.active) { fpsCounter.position = new Vector3(CameraBoundaries.boundaries.x - 3.5f, fpsCounter.position.y, fpsCounter.position.z); }

        levelStartTime += Time.deltaTime;
        if (!playerSpriteRenderer.isVisible && playerSpriteRenderer.transform.position.y < passLevelY.position.y && !gameOver)
        {
            PassLevel();
        }
        if (!playerSpriteRenderer.isVisible && playerSpriteRenderer.transform.position.y > GameOverY.position.y && !gameOver && levelStartTime > 3 && !levelComplete)
        {
            Gameover();
        }

        if (death)
        {
            timeAfterDeath += Time.deltaTime;
            cam1Noise.m_AmplitudeGain = Mathf.Lerp(cam1Noise.m_AmplitudeGain, 0, Time.deltaTime*3);
            deathPostFX.position = Vector3.Lerp(deathPostFX.position, Camera.main.transform.position, Time.deltaTime * 3);
            cameraMoveScript.moveSpeed = Mathf.Lerp(cameraMoveScript.moveSpeed, 0, Time.deltaTime * 1.5f);
            GifMaterial.col = Color.Lerp(GifMaterial.col, Color.red, Time.deltaTime * 1.5f);
            GifMaterial.fpsMultiplier = Mathf.Lerp(GifMaterial.fpsMultiplier, 0, Time.deltaTime * 1.5f);
            ScoreText.text.transform.localPosition = Vector3.Lerp(ScoreText.text.transform.localPosition, new Vector3(0, -5, -10),Time.deltaTime*3);
            gameOverText.color = Color.Lerp(gameOverText.color, new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 1), Time.deltaTime * 5);
            if ((Input.GetButtonUp("Jump") || (Input.GetMouseButtonUp(0) && timeAfterDeath >= 0.5f))&&!ForceSubmitName.success&&!AdsPlayer.clientAttempingToPlayAd)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                if (newHighestLevel || newHighestScore) { InstanceChange.LoadInstance(SceneManager.GetActiveScene().name, 2.5f); }
                else { InstanceChange.LoadInstance(SceneManager.GetActiveScene().name, 5); }
            }
        }

        if (levelComplete)
        {
            cameraMoveScript.firstLevelMultiplier = 1;
            levelStartTime = 0;
            cameraMoveScript.levelCompleteMultiplier = Mathf.Lerp(cameraMoveScript.levelCompleteMultiplier, levelCompleteMultiplierOnLevelComplete * 3, Time.deltaTime*2);
            if (PlatformSpawn.platformsActive <= 0)
            {
                StartCoroutine(RespawnPlayer());
                gameOver = false;
                levelComplete = false;
                cameraMoveScript.levelCompleteMultiplier = 1;
                if ((level/levelAntiCheatMultiplier) > 6){
                    levelSpeedIncrease *= rainbowSpeedIncreaseDecrease;
                    levelSpeedIncrease = Mathf.Clamp(levelSpeedIncrease, 0.015f, 1);
                    StartCoroutine(HideRainbowIndexTextDelay());
                }
                cameraMoveScript.moveSpeed += levelSpeedIncrease;
                platformSpawnScript.enabled = true;
                level+=levelAntiCheatMultiplier;
                if ((level/levelAntiCheatMultiplier) <= 6) { GifMaterial.col = levelColours[level/levelAntiCheatMultiplier]; }
                easyPlatforms = 2;
                if ((level/levelAntiCheatMultiplier) >= 12) { mediumPlatforms = 1; }
            }
        }
        if ((level/levelAntiCheatMultiplier) > 6 && (level/levelAntiCheatMultiplier) < invertLevel && !death)
        {
            GifMaterial.col = rainbow.color;
        }
        if ((level/levelAntiCheatMultiplier) >= hellLevel) { hellEventScript.active = true; }
        else if ((level/levelAntiCheatMultiplier) >= invertLevel) { invertColoursEventScript.activate = true; GifMaterial.col = Color.white; }
        if (newHighestScore)
        {
            newHighestScoreText.color = Color.Lerp(newHighestScoreText.color, new Color(newHighestScoreText.color.r, newHighestScoreText.color.g, newHighestScoreText.color.b, 1), Time.deltaTime * 4);
        }
        else if (newHighestLevel)
        {
            newHighestScoreText.text = "New Highest\nLevel Reached";
            newHighestScoreText.color = Color.Lerp(newHighestScoreText.color, new Color(newHighestScoreText.color.r, newHighestScoreText.color.g, newHighestScoreText.color.b, 1), Time.deltaTime * 4);
        }
        if (hideHomeButton)
        {
            homeButton.color = Color.Lerp(homeButton.color, new Color(homeButton.color.r, homeButton.color.g, homeButton.color.b, 0),Time.deltaTime*5);
            if (homeButton.color.a <= 0.05) homeButton.transform.parent.gameObject.SetActive(false);
        }
        if (moveRainbowText) { rainbowIndexText.transform.position += new Vector3(0, cameraMoveScript.moveSpeed * Time.deltaTime*3, 0); }
    }
    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(1.5f);
        Cache.player.transform.position = new Vector3(0, PlatformDestroyOnCull.cullY.position.y, Cache.player.transform.position.z);
        Cache.player.SetActive(true);
        levelStartTime = 0;
        transitionSFX.Play(true, transitionSfxPitch);
        if (transitionSfxPitch > 0.8f) { transitionSfxPitch -= 0.05f; }
    }
    IEnumerator HideHomeButton()
    {
        yield return new WaitForSeconds(3);
        hideHomeButton = true;
    }
    void Gameover()
    {
        gameOver = true;
        death = true;
        platformSpawnScript.enabled = false;
        deathSFX.Play();
        cam1Noise.m_AmplitudeGain = 10;
        SetHighestScore();
        SetHighestLevel();
        Cache.player.SetActive(false);
        if ((!Application.isEditor||1==1) && PlayerPrefs.GetInt("disableScoreSubmission",0)!=1){
            if (Application.platform == RuntimePlatform.WebGLPlayer){
                if (EncryptedPlayerPrefs.GetInt("HsMobile01") >= 150) { StartCoroutine(leaderboardManagerScript.SubmitScoreRoutine(EncryptedPlayerPrefs.GetInt("HsMobile01"), (level / levelAntiCheatMultiplier))); }
            }
            else{
                if (EncryptedPlayerPrefs.GetInt("HsMobile01") >= 25) { StartCoroutine(leaderboardManagerScript.SubmitScoreRoutine(EncryptedPlayerPrefs.GetInt("HsMobile01"), (level / levelAntiCheatMultiplier))); }
            }
            if ((level/levelAntiCheatMultiplier) >= 3) { StartCoroutine(leaderboardManagerScript.SubmitScoreRoutine(EncryptedPlayerPrefs.GetInt("HlMobile01"), (level/levelAntiCheatMultiplier), true)); }
            if ((level/levelAntiCheatMultiplier) >= 0 && !newHighestScore && !newHighestLevel && Application.isMobilePlatform) { StartCoroutine(QueueAd()); }
        }
#if UNITY_IPHONE || UNITY_EDTIOR
        if (Application.platform==RuntimePlatform.IPhonePlayer && PlayerPrefs.GetInt("hasReviewed", 0) == 0){
            PlayerPrefs.SetInt("reviewScore", PlayerPrefs.GetInt("reviewScore") + (Score.score / Score.scoreAntiCheatMultiplier));
            PlayerPrefs.SetInt("reviewGames", PlayerPrefs.GetInt("reviewGames") + 1);
            if (PlayerPrefs.GetInt("reviewScore")>=500 && PlayerPrefs.GetInt("reviewGames") >= 3){
                UnityEngine.iOS.Device.RequestStoreReview();
                PlayerPrefs.GetInt("hasReviewed", 1);
            }
        }
#endif
        if (EncryptedPlayerPrefs.GetInt("RewardedAdActive",1) == 0){
            EncryptedPlayerPrefs.SetInt("RewardedAdScoreReq", EncryptedPlayerPrefs.GetInt("RewardedAdScoreReq") + (int)(Score.score / Score.scoreAntiCheatMultiplier));
            if (EncryptedPlayerPrefs.GetInt("RewardedAdScoreReq") > 500)
            {
                EncryptedPlayerPrefs.SetInt("RewardedAdActive", 1);
                EncryptedPlayerPrefs.SetInt("RewardedAdScoreReq", 0);
            }
        }
        //deathHaptic.PlayEventVibration();
    }
    IEnumerator QueueAd()
    {
        yield return new WaitForSeconds(0.5f);
        if (EncryptedPlayerPrefs.GetInt("NoAds", 0) == 0) { adsPlayerScript.ShowAd(); }
        else { print("Ad did not play due to player having 'NoAds' purchased"); }
    }
    void PassLevel()
    {
        gameOver = true;
        levelComplete = true;
        PlatformPass.scoreIncreaseBonus++;
        levelCompleteMultiplierOnLevelComplete = cameraMoveScript.levelCompleteMultiplier;
        //platformSpawnScript.levelIncreasedTime -= 0.2f;
        //platformSpawnScript.levelIncreasedTime = Mathf.Clamp(platformSpawnScript.levelIncreasedTime, 0, 1);
        Cache.player.SetActive(false);
        platformSpawnScript.enabled = false;
        if ((level/levelAntiCheatMultiplier) > 6)
        {
            StartCoroutine(ShowRainbowIndexTextDelay());
            rainbowIndexText.text = ((level/levelAntiCheatMultiplier)-5).ToString();
        }
    }
    IEnumerator ShowRainbowIndexTextDelay()
    {
        yield return new WaitForSeconds(0.5f);
        moveRainbowText = true;
    }
    IEnumerator HideRainbowIndexTextDelay()
    {
        yield return new WaitForSeconds(1f);
        moveRainbowText = false;
        rainbowIndexText.transform.position = new Vector3(rainbowIndexText.transform.position.x, Camera.main.transform.position.y - 13, rainbowIndexText.transform.position.z);
    }
    private void OnApplicationQuit()
    {
        SetHighestScore();
        SetHighestLevel();
    }
    private void OnDestroy()
    {
        if (!hsCheckDone) { SetHighestScore(); }
        if (!hlCheckDone) { SetHighestLevel(); }
        EncryptedPlayerPrefs.SetInt("platformsSinceSpawn", (int)CoinsSpawn.platformsSinceSpawn);
    }
    void SetHighestScore()
    {
        if (Cache.isMobile||1==1) //Deprecated. Keep hard coded pass
        {
            if ((Score.score/Score.scoreAntiCheatMultiplier) > EncryptedPlayerPrefs.GetInt("HsMobile01"))
            {
                EncryptedPlayerPrefs.SetInt("HsMobile01", (Score.score/Score.scoreAntiCheatMultiplier));
                newHighestScore = true;
            }
        }
        /*
        else
        {
            if (Score.score > EncryptedPlayerPrefs.GetInt("HsPc01"))
            {
                EncryptedPlayerPrefs.SetInt("HsPc01", Score.score);
                newHighestScore = true;
            }
        }
        */
        hsCheckDone = true;
    }
    void SetHighestLevel()
    {
        if (Cache.isMobile||1==1) //Deprecated. Keep hard coded pass
        {
            if ((level/levelAntiCheatMultiplier) > EncryptedPlayerPrefs.GetInt("HlMobile01"))
            {
                EncryptedPlayerPrefs.SetInt("HlMobile01", level/levelAntiCheatMultiplier);
                newHighestLevel = true;
            }
        }
        /*
        else
        {
            if (level > EncryptedPlayerPrefs.GetInt("HlPc01"))
            {
                EncryptedPlayerPrefs.SetInt("HlPc01", level);
                newHighestLevel = true;
            }
        }
        */
        hlCheckDone = true;
    }
}
