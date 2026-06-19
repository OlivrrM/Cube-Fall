using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvertColoursEvent : MonoBehaviour
{
    public bool activate;
    public float transitionSpeed;
    bool playedSFX;
    public PlaySound transitionSFX;

    [Header("Assets")]
    public SpriteRenderer[] playerSprites;
    public PlatformSpawn platformSpawnScript;
    public GameObject floor0Invert;
    public GameObject floor0InvertPC;
    public TextMeshProUGUI rainbowLevelIndex;
    public Color rainbowLevelIndexColor;
    public TextMeshProUGUI scoreText;
    public PlayerHitGround playerHitGroundScript;
    public GameObject hitGroundMediumInvert;
    public GameObject hitGroundHeavyInvert;
    public TextMeshProUGUI gameOverText;
    public Color gameOverTextColor;
    public TrailRenderer trail;
    public SwipeInputGraphics swipeInputGraphicsScript;
    public Color invertedSwipeInputColor;
    public PlaySound[] ambienceSFX;

    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI coinsAddedText;

    Color playerStartCol;
    private void Start()
    {
        Camera.main.backgroundColor = Color.black;
        playerStartCol = playerSprites[0].color;
    }
    private void Update()
    {
        if (activate && !Level.death)
        {
            if (!playedSFX){
                transitionSFX.Play();
                ambienceSFX[Random.Range(0, ambienceSFX.Length)].Play();
                /*
                if (Screen.width > Screen.height){
                    platformSpawnScript.platforms[0] = floor0InvertPC;
                }
                else{
                    platformSpawnScript.platforms[0] = floor0Invert;
                }
                */
                platformSpawnScript.platforms[0] = floor0Invert;
                playedSFX = true;
            }
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, Color.white, Time.deltaTime * transitionSpeed);
            for (int i = 0; i < playerSprites.Length; i++){
                //playerSprites[i].color = new Color(Mathf.Lerp(playerSprites[i].color.r, Color.black.r, Time.deltaTime * transitionSpeed), Mathf.Lerp(playerSprites[i].color.g, Color.black.g, Time.deltaTime * transitionSpeed), Mathf.Lerp(playerSprites[i].color.b, Color.black.b, Time.deltaTime * transitionSpeed), playerSprites[i].color.a);
                //playerSprites[i].color = new Color(Mathf.Lerp(playerSprites[i].color.r, -playerStartCol.r+1, Time.deltaTime * transitionSpeed), Mathf.Lerp(playerSprites[i].color.g, -playerStartCol.g+1, Time.deltaTime * transitionSpeed), Mathf.Lerp(playerSprites[i].color.b, -playerStartCol.b+1, Time.deltaTime * transitionSpeed), playerSprites[i].color.a);
                float ogA = playerSprites[i].color.a;
                if (playerStartCol == Color.white) { playerSprites[i].color = Color.Lerp(playerSprites[i].color, Utilities.InvertCol(playerStartCol), Time.deltaTime * transitionSpeed); }
                else { playerSprites[i].color = Color.Lerp(playerSprites[i].color, Utilities.DarkenColByPercent(Utilities.InvertHue(playerStartCol), 20), Time.deltaTime * transitionSpeed); }
                playerSprites[i].color = new Color(playerSprites[i].color.r, playerSprites[i].color.g, playerSprites[i].color.b, ogA);
            }
            
            rainbowLevelIndex.color = Color.Lerp(rainbowLevelIndex.color, rainbowLevelIndexColor, Time.deltaTime * transitionSpeed);
            scoreText.color = Color.Lerp(scoreText.color,Color.black,Time.deltaTime*transitionSpeed);
            coinsText.color = Color.Lerp(coinsText.color,new Color(Color.black.r, Color.black.g, Color.black.b,coinsText.color.a),Time.deltaTime*transitionSpeed);
            coinsAddedText.color = Color.Lerp(coinsAddedText.color,Color.black,Time.deltaTime*transitionSpeed);
            //playerHitGroundScript.hitGroundHeavyFX = hitGroundHeavyInvert;
            //playerHitGroundScript.hitGroundMediumFX = hitGroundMediumInvert;
            gameOverText.color = new Color(Mathf.Lerp(gameOverText.color.r, gameOverTextColor.r, Time.deltaTime * transitionSpeed), Mathf.Lerp(gameOverText.color.g, gameOverTextColor.g, Time.deltaTime * transitionSpeed), Mathf.Lerp(gameOverText.color.b, gameOverTextColor.b, Time.deltaTime * transitionSpeed), gameOverText.color.a);
            //trail.startColor = Color.Lerp(trail.startColor, new Color(-playerStartCol.r+1, -playerStartCol.g + 1, -playerStartCol.b + 1,trail.startColor.a), Time.deltaTime * transitionSpeed);
            swipeInputGraphicsScript.defaultColor = Color.Lerp(swipeInputGraphicsScript.defaultColor, invertedSwipeInputColor, Time.deltaTime * transitionSpeed);
        }
        trail.startColor = playerSprites[0].color;
        trail.endColor = new Color(playerSprites[0].color.r, playerSprites[0].color.g, playerSprites[0].color.b, 0);
    }
}
