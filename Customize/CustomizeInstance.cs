using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomizeInstance : MonoBehaviour
{
    [HideInInspector] public float skinsUnlocked;
    public Transform canvasEdge;
    public static float skinXPosIndex;
    public GameObject skin;

    public Transform camera;
    public float navigateSpeed;
    public float navigateDecaySpeed;
    [HideInInspector]public float navigateVelocity;

    public Transform coinTransform;
    public float coinPositionSpeed;

    public Transform buyNewSkinTransform;
    public LockGraphic lockGraphicScript;

    public float cost;

    public static float xPosMultiplier;

    public PlaySound backSFX;

    public float keyboardNavigationSensitivity;

    public CustomizeMenuSelector customizeMenuSelectorScript;

    Transform selectedSkin;

    public CamViewEdgeCanvasScale camViewEdgeCanvasScaleScript;

    public GameObject purchaseSFX;

    public Transform rewardedAdTransform;

    public Skins skinsScript;

    public List<int> skinGraphicsUnlocked = new List<int>();

    public float xPosHardSet;
    private void Awake()
    {
        PlayerPrefs.SetInt("NextCostN", 2);
        xPosMultiplier = 0.65f;
        GifMaterial.col = Color.white;
        GifMaterial.fpsMultiplier = 1;

        skinsUnlocked = EncryptedPlayerPrefs.GetInt("skinsUnlocked", 0);
        string selectedSkinName = "Skin" + EncryptedPlayerPrefs.GetInt("SelectedSkin");
        for (int i = 0; i < skinsUnlocked; i++){
            GameObject newSkin = Instantiate(skin, new Vector3(-404, 8.5f, 0.25f), Quaternion.identity);
            newSkin.name = "Skin" + (i+1).ToString();
            if (newSkin.name == selectedSkinName) { selectedSkin = newSkin.transform; }
            Skin skinScript = newSkin.GetComponent<Skin>();
            skinScript.skinString = EncryptedPlayerPrefs.GetString("skin" + (i+1).ToString(),"0-0-0");
            skinGraphicsUnlocked.Add(int.Parse(skinScript.skinString.Split('-')[0]));
            skinScript.targetX = i+1;
        }
        RefreshCost();
        GameObject.Find("LockText").GetComponent<TextMeshProUGUI>().text = cost.ToString();
        camViewEdgeCanvasScaleScript.enabled = false;
        //camera.position = new Vector3(GameObject.Find("Skin" + EncryptedPlayerPrefs.GetInt("SelectedSkin")).transform.position.x, camera.position.y, camera.position.z);
    }
    public void MainMenu()
    {
        InstanceChange.LoadInstance("MainMenu", 1);
        backSFX.Play();
        HapticFeedbackPlayer.Play(0);
    }
    public void SilentMainMenu()
    {
        InstanceChange.LoadInstance("MainMenu", 1);
        HapticFeedbackPlayer.Play(0);
    }
    void RefreshCost()
    {
        switch (skinsUnlocked)
        {
            case 0:
                cost = 5;
                break;
            case 1:
                cost = 10;
                break;
            case 2:
                cost = 15;
                break;
            default:
                cost = 20;
                break;
        }
    }
    private void Update()
    {
        if (StartTime.instanceTime < 0.25f){
            if (selectedSkin != null){
                camera.transform.position = new Vector3(selectedSkin.position.x, camera.transform.position.y, camera.transform.position.z);
            }
        }
        else { camViewEdgeCanvasScaleScript.enabled = true; }
        //skinXPosIndex = canvasEdge.position.x;
        skinXPosIndex = xPosHardSet;
        float touchMagnitude = 0;
        if (Input.touchCount>0) { touchMagnitude = Input.GetTouch(0).deltaPosition.x/80; }

        navigateVelocity += Utilities.BoolToInt(Input.GetMouseButton(0)) * navigateSpeed * -(Input.GetAxis("Mouse X")+touchMagnitude);
        if (Input.GetKeyDown(KeyCode.RightArrow)) { navigateVelocity += keyboardNavigationSensitivity; }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { navigateVelocity -= keyboardNavigationSensitivity; }
        if (StartTime.instanceTime > 1f) { camera.position += new Vector3(navigateVelocity * Time.deltaTime, 0, 0); }
        //camera.position += new Vector3(navigateVelocity * Time.deltaTime, 0, 0);
        coinTransform.position = Vector3.Lerp(coinTransform.position, new Vector3(camera.transform.position.x, coinTransform.position.y, coinTransform.position.z),Time.deltaTime*coinPositionSpeed);
        buyNewSkinTransform.position = new Vector3(skinXPosIndex * (skinsUnlocked + 1) * xPosMultiplier, buyNewSkinTransform.position.y, buyNewSkinTransform.position.z);
        if (StartTime.instanceTime > 2f) 
        {
            if (camera.position.x < 0)
            {
                camera.position = Vector3.Lerp(camera.position, new Vector3(0, camera.position.y, camera.position.z), Time.deltaTime * 10);
            }
            else if (camera.position.x > rewardedAdTransform.position.x)
            {
                camera.position = Vector3.Lerp(camera.position, new Vector3(rewardedAdTransform.transform.position.x, camera.position.y, camera.position.z), Time.deltaTime * 10);
            }
        }
    }
    private void FixedUpdate()
    {
        navigateVelocity *= navigateDecaySpeed;
    }
    public void PurchaseNewSkin()
    {
        skinsUnlocked++;
        EncryptedPlayerPrefs.SetInt("Coins", EncryptedPlayerPrefs.GetInt("Coins") - (int)cost);
        TextMeshProUGUI text = GameObject.Find("CoinsText").GetComponent<TextMeshProUGUI>();
        text.text = EncryptedPlayerPrefs.GetInt("Coins").ToString();
        RefreshCost();
        int graphicID = 0;
        int loops = 0;
        while (true){
            graphicID = Random.RandomRange(0, skinsScript.graphics.Count);
            if (!skinGraphicsUnlocked.Contains(graphicID)){
                skinGraphicsUnlocked.Add(graphicID);
                break;
            }
            loops++;
            if (loops > skinsScript.graphics.Count) { break; }
        }
        //ERROR_TEXT.text += "1";
        EncryptedPlayerPrefs.SetString("skin" + skinsUnlocked, graphicID.ToString() + "-" + Random.RandomRange(0, skinsScript.colours.Count).ToString() + "-" + Random.Range(50,100).ToString());
        //ERROR_TEXT.text += "1";
        GameObject newSkin = Instantiate(skin, new Vector3(-404, 4f, 0.25f), Quaternion.identity);
        //ERROR_TEXT.text += "1";
        newSkin.name = "Skin" + skinsUnlocked.ToString();
        //ERROR_TEXT.text += "1";
        Skin skinScript = newSkin.GetComponent<Skin>();
        //ERROR_TEXT.text += "1";
        skinScript.skinString = EncryptedPlayerPrefs.GetString("skin" + skinsUnlocked.ToString(), "0-0-0");
        //ERROR_TEXT.text += "1";
        skinScript.targetX = skinsUnlocked;
        //ERROR_TEXT.text += "1";
        lockGraphicScript.Unlock();
        //ERROR_TEXT.text += "1";
        customizeMenuSelectorScript.previouslySelected = 404;
        //ERROR_TEXT.text += "1";
        PlayerPrefs.SetInt("NextCostN", 0);
        //ERROR_TEXT.text += "1";
        EncryptedPlayerPrefs.SetInt("skinsUnlocked", (int)skinsUnlocked);
        //ERROR_TEXT.text += "1";
        HapticFeedbackPlayer.Play(2);
        //ERROR_TEXT.text += "1";
        //Instantiate(purchaseSFX, Vector3.zero, Quaternion.identity);
    }
    private void OnDestroy()
    {
        EncryptedPlayerPrefs.SetInt("skinsUnlocked", (int)skinsUnlocked);
    }
}
