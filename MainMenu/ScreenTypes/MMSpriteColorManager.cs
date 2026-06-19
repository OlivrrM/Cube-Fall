using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSpriteColorManager : MonoBehaviour
{
    SpriteRenderer sprite;
    public Color[] colours;
    private void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.color = new Color(colours[MainMenuScreenManager.selectedScreenID].r, colours[MainMenuScreenManager.selectedScreenID].g, colours[MainMenuScreenManager.selectedScreenID].b, sprite.color.a);
    }
    private void Update()
    {
        if (MainMenuScreenManager.changingScreen)
        {
            sprite.color = new Color(
                Mathf.Lerp(sprite.color.r, colours[MainMenuScreenManager.selectedScreenID].r, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed*2),
                 Mathf.Lerp(sprite.color.g, colours[MainMenuScreenManager.selectedScreenID].g, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed*2),
                  Mathf.Lerp(sprite.color.b, colours[MainMenuScreenManager.selectedScreenID].b, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed*2),
                  sprite.color.a);
        }
    }
}
