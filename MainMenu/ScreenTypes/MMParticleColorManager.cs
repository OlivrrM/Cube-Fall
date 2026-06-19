using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMParticleColorManager : MonoBehaviour
{
    ParticleSystem particle;
    public Color[] colours;
    private void Start()
    {
        particle = gameObject.GetComponent<ParticleSystem>();
        particle.startColor = new Color(colours[MainMenuScreenManager.selectedScreenID].r, colours[MainMenuScreenManager.selectedScreenID].g, colours[MainMenuScreenManager.selectedScreenID].b, particle.startColor.a);
    }
    private void Update()
    {
        if (MainMenuScreenManager.changingScreen)
        {
            particle.startColor = new Color(
                Mathf.Lerp(particle.startColor.r, colours[MainMenuScreenManager.selectedScreenID].r, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                 Mathf.Lerp(particle.startColor.g, colours[MainMenuScreenManager.selectedScreenID].g, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                  Mathf.Lerp(particle.startColor.b, colours[MainMenuScreenManager.selectedScreenID].b, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                  particle.startColor.a);
        }
    }
}
