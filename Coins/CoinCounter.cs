using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI addCoinText;
    public Animator animator;
    float timeAfterCollect;

    public float counterTextInvisibleDecaySpeed;
    private void Start()
    {
        timeAfterCollect = 99;
        counterText.color = new Color(counterText.color.r, counterText.color.g, counterText.color.b, 0);
    }
    public void AddCoins(float amount)
    {
        timeAfterCollect = 0;
        counterText.color = new Color(counterText.color.r, counterText.color.g, counterText.color.b, 1);
        addCoinText.color = new Color(addCoinText.color.r, addCoinText.color.g, addCoinText.color.b, 1);
        addCoinText.text = "+" + amount.ToString();
        animator.Play("Plus");
    }
    private void Update()
    {
        timeAfterCollect += Time.deltaTime;
        if (timeAfterCollect > 0.166f){
            addCoinText.color = Utilities.Invisible(addCoinText.color);
            counterText.text = EncryptedPlayerPrefs.GetInt("Coins").ToString();
        }
        if (timeAfterCollect > 0.333f){
            counterText.color = Color.Lerp(counterText.color, Utilities.Invisible(counterText.color), Time.deltaTime * counterTextInvisibleDecaySpeed);
        }
        if (timeAfterCollect > 1)
        {
            animator.Play("Idle");
        }
    }
}
