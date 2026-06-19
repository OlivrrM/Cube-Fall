using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableIfInverted : MonoBehaviour
{
    public int screenID;
    public ParticleSystem particleSystem;
    private void Start()
    {
        if (MainMenuScreenManager.selectedScreenID == screenID)
        {
            particleSystem.Play();
        }
    }
}
