using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool collected;
    public Vector2 explosionForceAB;
    public Vector2 dragAB;
    public Animator animator;
    public PlaySound pickupSFX;

    public static CoinCounter coinCounterScript;
    private void Start()
    {
        try { if (coinCounterScript == null) { coinCounterScript = GameObject.Find("CoinCounter").GetComponent<CoinCounter>(); } }
        catch { }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.transform.name == "Player")
        {
            Collect();
        }
    }
    public void Collect(bool destroy = true)
    {
        if (!collected)
        {
            pickupSFX.Play();
            cameraShake.Shake(new Vector3(0.1f, 0.1f, 0), 10);
            HapticFeedbackPlayer.Play(1);
            animator.transform.eulerAngles = new Vector3(0, Mathf.Clamp(animator.transform.eulerAngles.y, 45, 135), 0);
            if (animator.transform.eulerAngles.y >= 45 && animator.transform.eulerAngles.y <= 90)
            {
                animator.transform.eulerAngles = new Vector3(0, 45, 0);
            }
            else if (animator.transform.eulerAngles.y <= 135 && animator.transform.eulerAngles.y >= 90)
            {
                animator.transform.eulerAngles = new Vector3(0, 135, 0);
            }
            animator.enabled = false;
            collected = true;
            if (coinCounterScript != null) {
                EncryptedPlayerPrefs.SetInt("Coins", EncryptedPlayerPrefs.GetInt("Coins") + 1);
                if (EncryptedPlayerPrefs.GetInt("Coins") >= CoinsSpawn.nextCost&&PlayerPrefs.GetInt("NextCostN")==0) { PlayerPrefs.SetInt("NextCostN", 1); print("Enough cash to purchase new skin!"); }
                coinCounterScript.AddCoins(1); 
            }
            PlayerPrefs.SetInt("CoinN",1);
            if (destroy) { Destroy(gameObject, 3); }
        }
    }
}
