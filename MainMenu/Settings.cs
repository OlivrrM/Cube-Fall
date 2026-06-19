using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
public class Settings : MonoBehaviour
{
    [Header("Platform screens")]
    public GameObject mobileScreen;
    public GameObject desktopScreen;
    public RectTransform webGlSettings;
    public GameObject desktopOnlySettings;

    [Header("Interface\n")]
    public TMP_Dropdown inputTypeDropdown;
    public Toggle postProcessingToggle;
    public Toggle postProcessingToggleDesktop;
    public Toggle hapticFeedbackToggle;
    public Toggle cameraShakeToggle;
    public Toggle cameraShakeToggleDesktop;
    public Slider volumeSlider;
    public Slider volumeSliderDesktop;
    public Slider qualitySlider;
    public Slider qualitySliderDesktop;
    public Toggle terminalAccessToggle;
    public Slider aspectRatioSlider;

    bool backButtonHighlighted;
    [Header("Other vars\n")]
    public ParticleSystem[] backButtonParticles;
    public ButtonBob backButtonBob;
    public PlaySound backSFX;
    public TextMeshProUGUI selectedAspectRatioTextName;
    public TextMeshProUGUI selectedAspectRatioTextValue;
    public RectTransform aspectRatioImage;
    public Vector2[] aspectRatioImageScales;

    [Header("Button click vars\n")]
    public PlaySound clickSFX;
    public PlaySound navigateSFX;
    public PlaySound switchSFX;

    [Header("VCams")]
    public CinemachineVirtualCamera cam1;
    private void Start()
    {
        if (Application.isMobilePlatform||Application.isEditor)
        {
            desktopScreen.SetActive(false);
        }
        else
        {
            mobileScreen.SetActive(false);
            if (Application.platform == RuntimePlatform.WebGLPlayer){
                desktopOnlySettings.SetActive(false);
                webGlSettings.anchoredPosition += new Vector2(0, 100);
            }
        }

        switch (PlayerPrefs.GetInt("InputType"))
        {
            case 0:
                inputTypeDropdown.value = 0;
                break;
            case 1:
                inputTypeDropdown.value = 1;
                break;
            case 2:
                inputTypeDropdown.value = 2;
                break;
            default:
                inputTypeDropdown.value = 0;
                PlayerPrefs.SetInt("InputType", 0);
                break;
        }
        aspectRatioSlider.value = PlayerPrefs.GetInt("AspectRatio", 0);
        switch (PlayerPrefs.GetInt("AspectRatio",0))
        {
            case 0:
                selectedAspectRatioTextName.text = "Mobile Phone";
                selectedAspectRatioTextValue.text = "9:19.5";
                break;
            case 1:
                selectedAspectRatioTextName.text = "Wide Mobile Phone";
                selectedAspectRatioTextValue.text = "9:16";
                break;
            case 2:
                selectedAspectRatioTextName.text = "Tablet";
                selectedAspectRatioTextValue.text = "3:4";
                break;
            case 3:
                selectedAspectRatioTextName.text = "Box";
                selectedAspectRatioTextValue.text = "1:1";
                break;
        }
        postProcessingToggle.isOn = !Utilities.IntToBool(PlayerPrefs.GetInt("PostFxDisabled"));
        postProcessingToggleDesktop.isOn = !Utilities.IntToBool(PlayerPrefs.GetInt("PostFxDisabled"));
        hapticFeedbackToggle.isOn = !Utilities.IntToBool(PlayerPrefs.GetInt("HapticsDisabled"));
        cameraShakeToggle.isOn = !Utilities.IntToBool(PlayerPrefs.GetInt("CameraShakeDisabled"));
        cameraShakeToggleDesktop.isOn = !Utilities.IntToBool(PlayerPrefs.GetInt("CameraShakeDisabled"));
        volumeSlider.value = PlayerPrefs.GetFloat("Volume",1);
        volumeSliderDesktop.value = PlayerPrefs.GetFloat("Volume",1);
        if (Application.platform == RuntimePlatform.IPhonePlayer) { qualitySlider.value = PlayerPrefs.GetFloat("ResMultiplier", 0.75f) * 100; }
        else { qualitySlider.value = PlayerPrefs.GetFloat("ResMultiplier", 0.5f) * 100; }
        qualitySliderDesktop.value = PlayerPrefs.GetFloat("ResMultiplier",0.75f)*100;
        terminalAccessToggle.isOn = Utilities.IntToBool(PlayerPrefs.GetInt("TerminalAccess", 0));
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "BackSelect") { backButtonHighlighted = true; backButtonBob.defaultScale = new Vector3(1.2f, 1.2f, 1.2f); }
        }
        else { backButtonHighlighted = false; backButtonBob.defaultScale = new Vector3(1f, 1f, 1f); }

        if (backButtonHighlighted)
        {
            for (int i = 0; i < backButtonParticles.Length; i++)
            {
                if (!backButtonParticles[i].isPlaying) {backButtonParticles[i].Play();}
            }
        }
        else
        {
            for (int i = 0; i < backButtonParticles.Length; i++)
            {
                backButtonParticles[i].Stop();
            }
        }
        aspectRatioImage.sizeDelta = Vector2.Lerp(aspectRatioImage.sizeDelta, aspectRatioImageScales[(int)aspectRatioSlider.value], Time.deltaTime * 10);
        if (aspectRatioSlider.value == 0) { selectedAspectRatioTextValue.rectTransform.rotation = Quaternion.Lerp(selectedAspectRatioTextValue.rectTransform.rotation, Quaternion.Euler(0, 0, 90), Time.deltaTime * 15); }
        else { selectedAspectRatioTextValue.rectTransform.rotation = Quaternion.Lerp(selectedAspectRatioTextValue.rectTransform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 10); }
        if (Input.GetKeyDown(KeyCode.Escape)) { Back(); }
    }
    public void OnInputTypeChange()
    {
        if (StartTime.instanceFrame > 10)
        {
            PlayerPrefs.SetInt("InputType", inputTypeDropdown.value);
            SettingsChange();
        }
    }
    public void OnPostProcessingChange(Toggle toggle)
    {
        print("postfx");
        PlayerPrefs.SetInt("PostFxDisabled", Utilities.BoolToInt(!toggle.isOn));
        PostProcessorController.Refresh();
        SettingsChange(Utilities.BoolToIntn(toggle.isOn));
    }
    public void OnHapticFeedbackChange()
    {
        print("Haptics");
        PlayerPrefs.SetInt("HapticsDisabled", Utilities.BoolToInt(!hapticFeedbackToggle.isOn));
        HapticFeedbackPlayer.Refresh();
        SettingsChange(Utilities.BoolToIntn(hapticFeedbackToggle.isOn));
    }
    public void OnCameraShakeChange(Toggle toggle)
    {
        print("CameraShake");
        PlayerPrefs.SetInt("CameraShakeDisabled", Utilities.BoolToInt(!toggle.isOn));
        HapticFeedbackPlayer.Refresh();
        SettingsChange(Utilities.BoolToIntn(toggle.isOn));
    }
    public void OnVolumeChange(Slider slider)
    {
        if (StartTime.instanceFrame > 10)
        {
            PlayerPrefs.SetFloat("Volume", slider.value);
            AudioListener.volume = slider.value;
            SettingsChange();
        }
    }
    public void OnQualityChange(Slider slider)
    {
        if (StartTime.instanceFrame > 10)
        {
            PlayerPrefs.SetFloat("ResMultiplier", (float)(slider.value / 100));
            Resolution.setResolution = false;
            Resolution.setResMultiplier = false;
            SettingsChange();
        }
    }
    public void Back()
    {
        InstanceChange.LoadInstance("MainMenu", 1);
        backSFX.Play();
        HapticFeedbackPlayer.Play(0);
    }
    public void SettingsChange(int on = 0)
    {
        if (on != 0) { clickSFX.Play(true,1+((float)on*0.2f)); }
        else { clickSFX.Play(); }
        HapticFeedbackPlayer.Play(0);
    }
    public void NextPage(bool forward)
    {
        navigateSFX.Play();
        cam1.Priority = Utilities.BoolToInt(forward)*25;
        HapticFeedbackPlayer.Play(0);
        //SettingsChange();
    }
    public void PrivacyPolicy()
    {
        clickSFX.Play();
        HapticFeedbackPlayer.Play(0);
        Application.OpenURL("https://olivrr.itch.io/downfall-privacy");
    }
    public void IapRefresh()
    {
        clickSFX.Play();
        HapticFeedbackPlayer.Play(0);
    }
    public void TerminalAccessToggle()
    {
        if (StartTime.instanceTime > 1) {
            TerminalAccessManager.EnableDisableTerminalAccess();
            HapticFeedbackPlayer.Play(0);
            switchSFX.Play(true, 1 + ((float)PlayerPrefs.GetInt("TerminalAccess") / 3));
        }
    }
    public void OnAspectRatioChange()
    {
        if (StartTime.instanceFrame > 10)
        {
            PlayerPrefs.SetInt("AspectRatio", (int)aspectRatioSlider.value);
            switch (aspectRatioSlider.value)
            {
                case 0:
                    selectedAspectRatioTextName.text = "Mobile Phone";
                    selectedAspectRatioTextValue.text = "9:19.5";
                    break;
                case 1:
                    selectedAspectRatioTextName.text = "Wide Mobile Phone";
                    selectedAspectRatioTextValue.text = "9:16";
                    break;
                case 2:
                    selectedAspectRatioTextName.text = "Tablet";
                    selectedAspectRatioTextValue.text = "3:4";
                    break;
                case 3:
                    selectedAspectRatioTextName.text = "Box";
                    selectedAspectRatioTextValue.text = "1:1";
                    break;
            }
            Resolution.setResolution = false;
            Resolution.setResMultiplier = false;
            SettingsChange();
        }
    }
    public void MoreGames()
    {
        clickSFX.Play();
        HapticFeedbackPlayer.Play(0);
        if (Application.platform == RuntimePlatform.IPhonePlayer) { Application.OpenURL("https://apps.apple.com/us/developer/robert-martin/id1676456011"); }
        //else if (Application.platform == RuntimePlatform.Android) { Application.OpenURL("market://developer?id=OlivrM"); }
        else if (Application.platform == RuntimePlatform.Android) { Application.OpenURL("https://play.google.com/store/apps/developer?id=OlivrM"); }
        else { Application.OpenURL("https://olivrr.itch.io/"); }
    }
}
