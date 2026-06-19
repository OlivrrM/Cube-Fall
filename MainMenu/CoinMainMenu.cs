using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CoinMainMenu : MonoBehaviour
{
    public GameObject coinMM;
    public TextMeshProUGUI coinText;
    public Coin coinScript;
    public TargetSizeOverTime targetSizeOverTimeScript;
    bool clicked;
    private void Start()
    {
        if (EncryptedPlayerPrefs.GetInt("Coins") <=0&&SceneManager.GetActiveScene().name=="MainMenu") { coinText.transform.parent.parent.gameObject.SetActive(false); }
        targetSizeOverTimeScript = coinText.GetComponentInChildren<TargetSizeOverTime>();
        coinText.text = EncryptedPlayerPrefs.GetInt("Coins").ToString();
    }
    private void OnMouseDown()
    {
        if (!clicked && EncryptedPlayerPrefs.GetInt("Coins")>0)
        {
            coinScript.Collect();
            coinText.text = (float.Parse(coinText.text) - 1).ToString();
            Invoke("HideCoinText", 1);
            Invoke("NewCoin", 2.95f);
            clicked = true;
        }
    }
    void NewCoin()
    {
        GameObject newCoin = Instantiate(coinMM, transform.position, Quaternion.identity,transform);
        coinScript = newCoin.GetComponent<Coin>();
        coinText = newCoin.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        targetSizeOverTimeScript = coinText.GetComponentInChildren<TargetSizeOverTime>();
        coinText.text = EncryptedPlayerPrefs.GetInt("Coins").ToString();
        //coinText.text = (float.Parse(coinText.text) +1).ToString();
        clicked = false;
    }
    void HideCoinText()
    {
        targetSizeOverTimeScript.targetSize = Vector3.zero;
    }
}
