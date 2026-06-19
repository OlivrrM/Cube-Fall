using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardedAd : MonoBehaviour
{
    public Transform lockTransform;
    public float xPos;

    public AdsPlayer adsPlayerScript;
    bool attemptingAd;

    public Coin coinScript;

    public int coinsAmountReward;
    public TextMeshProUGUI rewardAmountText;
    public Transform coinTransform;

    bool finishedAnimation;
    public Animator coinBobAnimator;
    float finishedAdTime;

    bool thanksForTheCoins;

    public GameObject rewardedAdGraphic;

    public PlaySound addedCoinsSFX;

    public Material whiteMat;
    Color defaultWhiteMatCol;
    public Color failureCol;
    public TextMeshProUGUI needInternetText;
    float timeAfterAdPlayFailure;
    public PlaySound errorSFX;

    float highlightedIndexOnTap;
    public CustomizeMenuSelector customizeMenuSelectorScript;
    public CustomizeInstance customizeInstanceScript;

    bool cubesGraphicPlayed;
    private void Start()
    {
        if (!Application.isMobilePlatform) { coinScript.gameObject.SetActive(false); }
        if (MainMenuScreenManager.selectedScreenID == 1) { defaultWhiteMatCol = Color.black; }
        else { defaultWhiteMatCol = Color.white; }
        whiteMat.color = defaultWhiteMatCol;
        if (EncryptedPlayerPrefs.GetInt("RewardedAdActive", 1) == 0) { rewardedAdGraphic.SetActive(false); }
        rewardAmountText.text = "+" + coinsAmountReward.ToString();
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, lockTransform.position + new Vector3(xPos, 0, 0), Time.deltaTime*2);
        if (attemptingAd)
        {
            if (AdsPlayer.adCurrentlyPlaying)
            {
                coinScript.Collect(false);
                cubesGraphicPlayed = true;
                attemptingAd = false;
            }
            if (adsPlayerScript.unsucessfulAdPlayedThisInstance)
            {
                timeAfterAdPlayFailure = 0;
                whiteMat.color = failureCol;
                needInternetText.color = new Color(needInternetText.color.r, needInternetText.color.g, needInternetText.color.b, 1);
                errorSFX.Play();
                attemptingAd = false;
            }
        }
        if (adsPlayerScript.successfulAdFinishedThisInstance){
            if (!thanksForTheCoins)
            {
                EncryptedPlayerPrefs.SetInt("Coins", EncryptedPlayerPrefs.GetInt("Coins") + coinsAmountReward);
                thanksForTheCoins = true;
            }
            finishedAdTime += Time.deltaTime;
            if (finishedAdTime > 1f){
                rewardAmountText.transform.position = Vector3.Lerp(rewardAmountText.transform.position, coinTransform.position, Time.deltaTime * 5);
                rewardAmountText.transform.localScale = Vector3.Lerp(rewardAmountText.transform.localScale, Vector3.zero, Time.deltaTime * 5);
                if (!finishedAnimation && rewardAmountText.transform.localScale.x < 0.025f)
                {
                    coinBobAnimator.Play("Bob");
                    GameObject CoinsText = GameObject.Find("CoinsText");
                    if (CoinsText != null) { CoinsText.GetComponent<TextMeshProUGUI>().text = EncryptedPlayerPrefs.GetInt("Coins").ToString(); }
                    rewardAmountText.gameObject.SetActive(false);
                    EncryptedPlayerPrefs.SetInt("RewardedAdActive", 0);
                    EncryptedPlayerPrefs.SetInt("RewardedAdScoreReq", 0);
                    addedCoinsSFX.Play();
                    finishedAnimation = true;
                }
            }
        }
        timeAfterAdPlayFailure += Time.deltaTime;
        if (timeAfterAdPlayFailure > 1) { needInternetText.color = Color.Lerp(needInternetText.color, new Color(needInternetText.color.r, needInternetText.color.g, needInternetText.color.b, 0), Time.deltaTime * 3); }
        whiteMat.color = Color.Lerp(whiteMat.color, defaultWhiteMatCol, Time.deltaTime * 2);

        if (Input.GetMouseButtonDown(0))
        {
            highlightedIndexOnTap = customizeMenuSelectorScript.currentlySelected;
        }

        if (adsPlayerScript.successfulAdFinishedThisInstance&& !cubesGraphicPlayed)
        {
            coinScript.Collect(false);
            cubesGraphicPlayed = true;
        }
    }
    private void OnMouseUp()
    {
        if (!adsPlayerScript.successfulAdPlayedThisInstance&& EncryptedPlayerPrefs.GetInt("RewardedAdActive", 1) == 1&& customizeMenuSelectorScript.currentlySelected == customizeInstanceScript.skinsUnlocked + 2 && highlightedIndexOnTap == customizeInstanceScript.skinsUnlocked + 2&& customizeInstanceScript.navigateVelocity < 1f && customizeInstanceScript.navigateVelocity > -1f)
        {
            if (Application.isMobilePlatform)
            {
                if (Input.GetTouch(0).deltaPosition.x == 0)
                {
                    attemptingAd = true;
                    adsPlayerScript.ShowAd();
                }
            }
            else
            {
                attemptingAd = true;
                adsPlayerScript.ShowAd();
            }
        }
    }
}
