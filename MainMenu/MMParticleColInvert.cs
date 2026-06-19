using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMParticleColInvert : MonoBehaviour
{
    ParticleSystem particleSystem;
    public bool[] invert;
    Color defaultCol;
    Color invertedCol;
    private void Start()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        defaultCol = particleSystem.main.startColor.color;
        if (defaultCol != new Color(Color.white.r, Color.white.g, Color.white.b, defaultCol.a))
        {
            invertedCol = Utilities.DarkenColByPercent(Utilities.InvertHue(defaultCol), 20);
        }
        else
        {
            invertedCol = Utilities.InvertCol(defaultCol);
        }
        if (invert[MainMenuScreenManager.selectedScreenID])
        {
            //sprite.color = new Color(-sprite.color.r+1, -sprite.color.g + 1, -sprite.color.b + 1, sprite.color.a);
            var main = particleSystem.main;
            main.startColor = invertedCol;
        }
        //sprite.color = new Color(colours[MainMenuScreenManager.selectedScreenID].r, colours[MainMenuScreenManager.selectedScreenID].g, colours[MainMenuScreenManager.selectedScreenID].b, sprite.color.a);
    }
    /*
    private void Update()
    {
        if (MainMenuScreenManager.changingScreen)
        {
            if (invert[MainMenuScreenManager.selectedScreenID])
            {
                /*
                sprite.color = new Color(
                    Mathf.Lerp(sprite.color.r, -defaultCol.r+1, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                     Mathf.Lerp(sprite.color.g, -defaultCol.g + 1, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                      Mathf.Lerp(sprite.color.b, -defaultCol.b + 1, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                      sprite.color.a);
                var main = particleSystem.main;
                main.startColor = Color.Lerp(main.startColor.color, invertedCol, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2);
            }
            else
            {
                var main = particleSystem.main;
                main.startColor = Color.Lerp(main.startColor.color, defaultCol, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2);
            }
        }
    }
    */
}
