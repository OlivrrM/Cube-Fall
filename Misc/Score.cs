using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static int score;
    public static int scoreAntiCheatMultiplier;
    public static float sizeMultiplier;

    public RectTransform text;
    float defaultTextSize;
    private void Start()
    {
        scoreAntiCheatMultiplier = Random.Range(99, 9999);
        sizeMultiplier = 1;
        defaultTextSize = text.localScale.x;
        score = 0;
        AddScore(0);
    }
    private void Update()
    {
        text.localScale = new Vector3(defaultTextSize * sizeMultiplier, defaultTextSize * sizeMultiplier, defaultTextSize * sizeMultiplier);
        sizeMultiplier = Mathf.Lerp(sizeMultiplier, 1, Time.deltaTime*2);
    }
    public static void AddScore(int scoreIncrease)
    {
        score += scoreIncrease * scoreAntiCheatMultiplier;
        sizeMultiplier += 0.5f+Mathf.Clamp((((float)scoreIncrease / 10) - 0.1f),0,0.5f);
        ScoreText.text.text = (score/scoreAntiCheatMultiplier).ToString();
    }
}
