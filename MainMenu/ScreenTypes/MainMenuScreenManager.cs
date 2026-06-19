using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreenManager : MonoBehaviour
{
    public static int selectedScreenID;
    public static bool changingScreen;
    public static float changingScreenSpeed;
    public float setChangingScreenSpeed;

    public Color[] skyboxColours;
    public ParticleSystem hellParticles;
    public ParticleSystem[] defaultParticles;
    public ParticleSystem[] invertedParticles;

    public Transform hellFX;

    public GifMaterial[] gifMaterials;

    public PlaySound invertedSetSFX;
    public PlaySound hellSetSFX;

    public PlaySound[] invertedAmbienceSFX;
    public PlaySound[] hellAmbienceSFX;
    private void Awake()
    {
        changingScreenSpeed = setChangingScreenSpeed;
        changingScreen = false;
        selectedScreenID = EncryptedPlayerPrefs.GetInt("SelectedScreenID", 0);
        SetScreen();
    }
    void SetScreen()
    {
        Camera.main.backgroundColor = skyboxColours[selectedScreenID];
        switch (selectedScreenID)
        {
            case 0:
                for (int i = 0; i < defaultParticles.Length; i++){defaultParticles[i].Play();}
                break;
            case 1:
                for (int i = 0; i < invertedParticles.Length; i++) { invertedParticles[i].Play(); }
                invertedAmbienceSFX[Random.Range(0, invertedAmbienceSFX.Length)].Play();
                break;
            case 2:
                hellParticles.Play();
                hellFX.localPosition = new Vector3(0, 0, 0);
                hellAmbienceSFX[Random.Range(0, hellAmbienceSFX.Length)].Play();
                break;
        }
        //for (int i = 0; i < gifMaterials.Length; i++){gifMaterials[i].RefreshMat();}
    }
    public void ChangeToScreen(int screenID)
    {
        if (selectedScreenID != screenID)
        {
            selectedScreenID = screenID;
            changingScreen = true;
            EncryptedPlayerPrefs.SetInt("SelectedScreenID", selectedScreenID);

            for (int i = 0; i < defaultParticles.Length; i++) { defaultParticles[i].Stop(); }
            for (int i = 0; i < invertedParticles.Length; i++) { invertedParticles[i].Stop(); }
            hellParticles.Stop();
            for (int i = 0; i < invertedAmbienceSFX.Length; i++){invertedAmbienceSFX[i].SFX.Stop();}
            for (int i = 0; i < hellAmbienceSFX.Length; i++) { hellAmbienceSFX[i].SFX.Stop(); }
            switch (selectedScreenID){
                case 0:
                    for (int i = 0; i < defaultParticles.Length; i++) { defaultParticles[i].Play(); }
                    break;
                case 1:
                    for (int i = 0; i < invertedParticles.Length; i++) { invertedParticles[i].Play(); }
                    invertedSetSFX.Play();
                    invertedAmbienceSFX[Random.Range(0, invertedAmbienceSFX.Length)].Play();
                    break;
                case 2:
                    hellParticles.Play();
                    hellSetSFX.Play();
                    hellAmbienceSFX[Random.Range(0, hellAmbienceSFX.Length)].Play();
                    break;
            }
            MMMatColorManager.transitionAlpha = 1;
            //for (int i = 0; i < gifMaterials.Length; i++) { gifMaterials[i].RefreshMat(); }
        }
    }
    private void Update()
    {
        MMMatColorManager.transitionAlpha = Mathf.Lerp(MMMatColorManager.transitionAlpha, 0, Time.deltaTime * 3);
        if (changingScreen)
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, skyboxColours[selectedScreenID], Time.deltaTime * changingScreenSpeed);
            if (selectedScreenID == 2) { hellFX.localPosition = Vector3.Lerp(hellFX.localPosition, new Vector3(0, 0, 0), Time.deltaTime * changingScreenSpeed); }
            else { hellFX.localPosition = Vector3.Lerp(hellFX.localPosition, new Vector3(0, 0, -2.55f), Time.deltaTime * changingScreenSpeed); }
        }
    }
}
