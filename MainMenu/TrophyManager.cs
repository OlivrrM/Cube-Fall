using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrophyManager : MonoBehaviour
{
    public GameObject[] trophies;
    public Image[] trophiesImages;
    public Color[] trophiesColours;
    public Color[] trophiesDefaultColours;
    public TextMeshProUGUI trophyInfoText;
    public float textShowHideSpeed;
    public RectTransform buttonsTransform;
    public RectTransform trophyButtons;
    public PlaySound selectSFX;
    int targetTrophyID;
    Color defaultTrophyColour;

    public TextMeshProUGUI[] allText;
    public Color[] textColours0;
    public Color[] textColours1;
    public Color[] textColours2;
    public ParticleSystem[] allParticles;
    public Color[] particleColours0;
    public Color[] particleColours1;
    public Color[] particleColours2;
    public Camera camera;
    public Color[] skyboxColours;
    public PlaySound[] playSounds0;
    public PlaySound[] playSounds1;
    public PlaySound[] playSounds2;
    public Color[] trophyColours0;
    public Color[] trophyColours1;
    public Color[] trophyColours2;
    public ParticleSystem[] environmentParticles;
    public Color[] environmentDefaultTrophyColours;
    int selectedEnvironment;
    private void Start()
    {
        trophyInfoText.color = Utilities.Invisible(trophyInfoText.color);
        int trophiesUnlocked = 0;
        int savedHighestLevel = EncryptedPlayerPrefs.GetInt("HlMobile01");
        if (savedHighestLevel >= 56) { trophiesUnlocked = 3; }
        else if (savedHighestLevel >= 12) { trophiesUnlocked = 2; trophyButtons.anchoredPosition += new Vector2(50, 0); }
        else if (savedHighestLevel >= 7) { trophiesUnlocked = 1; trophyButtons.anchoredPosition += new Vector2(100, 0); }
        for (int i = 0; i < trophies.Length; i++) { trophies[i].SetActive(false); }
        for (int i = 0; i < trophiesUnlocked; i++) { trophies[i].SetActive(true); }
        targetTrophyID = 404;
        selectedEnvironment = PlayerPrefs.GetInt("mmEnvironment", 0);
        //SetScreenEnvironemnt(selectedEnvironment,true);
    }
    public void TrophyClick(int trophyID)
    {
        targetTrophyID = trophyID;
        selectSFX.Play(true,(float)(1f+targetTrophyID/10f));
        switch (trophyID)
        {
            case 0:
                trophyInfoText.text = "Awarded for reaching the rainbow zone";
                HapticFeedbackPlayer.Play(0);
                break;
            case 1:
                trophyInfoText.text = "Awarded for reaching the inverted zone";
                HapticFeedbackPlayer.Play(1);
                break;
            case 2:
                trophyInfoText.text = "Awarded for reaching the pits of hell";
                HapticFeedbackPlayer.Play(2);
                break;
        }
        selectedEnvironment = trophyID;
        //SetScreenEnvironemnt(trophyID,true);
    }
    void SetScreenEnvironemnt(int ID, bool onClick)
    {
        Color[] selectedTextColours = new Color[0];
        Color[] selectedParticleColours = new Color[0];
        PlaySound[] soundsToPlay = new PlaySound[0];
        switch (ID)
        {
            case 0:
                selectedTextColours = textColours0;
                selectedParticleColours = particleColours0;
                soundsToPlay = playSounds0;
                trophiesColours = trophyColours0;
                break;
            case 1:
                selectedTextColours = textColours1;
                selectedParticleColours = particleColours1;
                soundsToPlay = playSounds1;
                trophiesColours = trophyColours1;
                break;
            case 2:
                selectedTextColours = textColours2;
                selectedParticleColours = particleColours2;
                soundsToPlay = playSounds2;
                trophiesColours = trophyColours2;
                break;
        }
        for (int i = 0; i < allText.Length; i++){
            allText[i].color = Color.Lerp(allText[i].color, selectedTextColours[i],Time.deltaTime*5);
        }
        for (int i = 0; i < allParticles.Length; i++){
            allParticles[i].startColor = Color.Lerp(allParticles[i].startColor, selectedParticleColours[i],Time.deltaTime*5);
        }
        camera.backgroundColor = Color.Lerp(camera.backgroundColor, skyboxColours[ID], Time.deltaTime * 5);
        if (onClick)
        {
            PlayerPrefs.SetInt("mmEnvironment", ID);
            if (playSounds0 != soundsToPlay)
            {
                for (int i = 0; i < playSounds0.Length; i++)
                {
                    playSounds0[i].SFX.Stop();
                }
            }
            if (playSounds1 != soundsToPlay)
            {
                for (int i = 0; i < playSounds1.Length; i++)
                {
                    playSounds1[i].SFX.Stop();
                }
            }
            if (playSounds2 != soundsToPlay)
            {
                for (int i = 0; i < playSounds2.Length; i++)
                {
                    playSounds2[i].SFX.Stop();
                }
            }
            for (int i = 0; i < soundsToPlay.Length; i++)
            {
                soundsToPlay[i].Play();
            }
            for (int i = 0; i < environmentParticles.Length; i++)
            {
                environmentParticles[i].Stop();
            }
            environmentParticles[ID].Play();
            defaultTrophyColour = environmentDefaultTrophyColours[ID];
        }
    }
    private void OnDestroy()
    {
        //camera.backgroundColor = Color.black;
    }
    private void Update()
    {
        //SetScreenEnvironemnt(selectedEnvironment, false);
        if (EventSystem.current.currentSelectedGameObject == null){
            if (targetTrophyID != 404) { selectSFX.Play(true, 0.65f); }
            targetTrophyID = 404;
        }
        for (int i = 0; i < trophiesImages.Length; i++){
            //trophiesImages[i].color = defaultTrophyColour;
            trophiesImages[i].color = trophiesDefaultColours[MainMenuScreenManager.selectedScreenID];
        }
        if (targetTrophyID != 404){
            buttonsTransform.anchoredPosition = new Vector3(buttonsTransform.anchoredPosition.x, Mathf.Lerp(buttonsTransform.anchoredPosition.y, -25,Time.deltaTime*textShowHideSpeed));
            trophyInfoText.color = new Color(trophyInfoText.color.r, trophyInfoText.color.g, trophyInfoText.color.b, Mathf.Lerp(trophyInfoText.color.a, 1, Time.deltaTime * textShowHideSpeed));
            trophyInfoText.color = Color.Lerp(trophyInfoText.color, trophiesColours[targetTrophyID],Time.deltaTime*4);
            trophiesImages[targetTrophyID].color = trophiesColours[targetTrophyID];
            //trophyCountText.color = new Color(trophyCountText.color.r, trophyCountText.color.g, trophyCountText.color.b, Mathf.Lerp(trophyCountText.color.a,1, Time.deltaTime * textShowHideSpeed));
        }
        else{
            buttonsTransform.anchoredPosition = new Vector3(buttonsTransform.anchoredPosition.x, Mathf.Lerp(buttonsTransform.anchoredPosition.y, 35, Time.deltaTime * textShowHideSpeed));
            trophyInfoText.color = new Color(trophyInfoText.color.r, trophyInfoText.color.g, trophyInfoText.color.b, Mathf.Lerp(trophyInfoText.color.a, 0, Time.deltaTime * textShowHideSpeed*5));
            //trophyCountText.color = new Color(trophyCountText.color.r, trophyCountText.color.g, trophyCountText.color.b, Mathf.Lerp(trophyCountText.color.a, 0, Time.deltaTime * textShowHideSpeed));
        }
    }
}
