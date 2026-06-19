using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMImageColorManager : MonoBehaviour
{
    Image image;
    public Color[] colours;
    public Sprite[] sprites;
    public bool overwriteA;
    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        if (overwriteA) { image.color = colours[MainMenuScreenManager.selectedScreenID]; }
        else { image.color = new Color(colours[MainMenuScreenManager.selectedScreenID].r, colours[MainMenuScreenManager.selectedScreenID].g, colours[MainMenuScreenManager.selectedScreenID].b, image.color.a); }
        if (sprites.Length>0) { image.sprite = sprites[MainMenuScreenManager.selectedScreenID]; }
    }
    private void Update()
    {
        if (MainMenuScreenManager.changingScreen){
            if (overwriteA) { image.color = Color.Lerp(image.color, colours[MainMenuScreenManager.selectedScreenID], Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2); }
            else{
                image.color = new Color(
                    Mathf.Lerp(image.color.r, colours[MainMenuScreenManager.selectedScreenID].r, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                        Mathf.Lerp(image.color.g, colours[MainMenuScreenManager.selectedScreenID].g, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                            Mathf.Lerp(image.color.b, colours[MainMenuScreenManager.selectedScreenID].b, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                                image.color.a);
            }
            if (sprites != null) { image.sprite = sprites[MainMenuScreenManager.selectedScreenID]; }
        }
    }
}
