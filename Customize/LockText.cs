using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public RectTransform textRect;
    public static CustomizeInstance customizeInstanceScript;
    bool collected;
    public TargetSizeOverTime targetSizeOverLifetimeScript;

    public Material whiteMat;
    public Color insufficientFundsCol;
    public float HELLO;
    public Color defaultCol;

    public GameObject notEnoughFundsSFX;
    private void Start()
    {
        if (MainMenuScreenManager.selectedScreenID == 1) { defaultCol = Color.black; }
        else { defaultCol = Color.white; }
        whiteMat.color = Color.white;
        if (customizeInstanceScript == null) { customizeInstanceScript = GameObject.Find("InstanceScripts").GetComponent<CustomizeInstance>(); }
    }
    private void OnDestroy()
    {
        whiteMat.color = Color.white;
    }
    private void Update()
    {
        if (!collected) { text.text = customizeInstanceScript.cost.ToString(); }

        if (collected)
        {
            targetSizeOverLifetimeScript.targetSize = Vector3.zero;
            targetSizeOverLifetimeScript.speed = 8;
        }
        whiteMat.color = Color.Lerp(whiteMat.color, Color.white, Time.deltaTime * 3);
        text.color = Color.Lerp(text.color, defaultCol, Time.deltaTime*1.5f);
        HELLO = Mathf.Lerp(HELLO, 0, Time.deltaTime * 5);
        try { textRect.localPosition = new Vector3(textRect.localPosition.x, textRect.localPosition.y, textRect.localPosition.z); }
        catch { }
    }
    private void FixedUpdate()
    {
        text.margin = new Vector4(HELLO * Utilities.CoinFlipn(), text.margin.y, text.margin.z, text.margin.w);
    }
    public void Collect()
    {
        collected = true;
        text.transform.parent = GameObject.Find("Canvas").transform;
        Destroy(text.gameObject, 2.9f);
    }
    public void InsufficientFunds()
    {
        HELLO = 30;
        whiteMat.color = insufficientFundsCol;
        text.color = insufficientFundsCol;
        HapticFeedbackPlayer.Play(0);
        Instantiate(notEnoughFundsSFX, transform.position, Quaternion.identity);
    }
}
