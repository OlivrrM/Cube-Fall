using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMainMenu : MonoBehaviour
{
    public PlaySound backSFX;
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6)) && !ForceSubmitName.success) { Back(); }
    }
    public void Back()
    {
        InstanceChange.LoadInstance("MainMenu", 0.8f); backSFX.Play();
        HapticFeedbackPlayer.Play(0);
    }
}
