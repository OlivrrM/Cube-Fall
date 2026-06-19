using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HellEvent : MonoBehaviour
{
    public bool active;
    public float transitionSpeed;
    bool playedSFX;
    public PlaySound transitionSFX;
    public PlaySound ambienceSFX;

    [Header("Assets")]
    public InvertColoursEvent invertColoursEventScript;
    public Color backgroundColor;
    public PlatformSpawn platformSpawnScript;
    public GameObject floor0HellPC;
    public GameObject floor0Hell;
    public Color floorsColourA;
    public Color floorsColourB;
    public float floorsColourLerpSpeed;
    float floorsColoursLerpBall;
    public Transform FxVolume;
    public TextMeshProUGUI scoreText;
    public ParticleSystem hellParticles;

    private void Start()
    {
        hellParticles.Stop();
    }
    private void Update()
    {
        if (active && !Level.death)
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, backgroundColor, Time.deltaTime * transitionSpeed);
            if (!playedSFX){
                transitionSFX.Play();
                ambienceSFX.Play();
                hellParticles.Play();
                /*
                if (Screen.width > Screen.height){
                    platformSpawnScript.platforms[0] = floor0HellPC;
                }
                else{
                    platformSpawnScript.platforms[0] = floor0Hell;
                }
                */
                platformSpawnScript.platforms[0] = floor0Hell;
                playedSFX = true;
            }
            invertColoursEventScript.activate = false;
            floorsColoursLerpBall = Mathf.PingPong(Time.time* floorsColourLerpSpeed, 1);
            GifMaterial.col = Color.Lerp(floorsColourA, floorsColourB, floorsColoursLerpBall);
            FxVolume.localPosition = Vector3.Lerp(FxVolume.localPosition, new Vector3(0, 0, 0), Time.deltaTime * transitionSpeed);
        }
        else if (active && Level.death)
        {
            scoreText.color = Color.Lerp(scoreText.color, Color.white, Time.deltaTime * 3);
        }
    }
}
