using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMImageColInvert : MonoBehaviour
{
    Image image;
    public bool[] invert;
    Color defaultCol;
    public Color invertedCol;
    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        defaultCol = image.color;
        invertedCol = Utilities.InvertHue(defaultCol);
        if (invert[MainMenuScreenManager.selectedScreenID])
        {
            image.color = new Color(-image.color.r + 1, -image.color.g + 1, -image.color.b + 1, image.color.a);
            //image.color = invertedCol;
        }
        //sprite.color = new Color(colours[MainMenuScreenManager.selectedScreenID].r, colours[MainMenuScreenManager.selectedScreenID].g, colours[MainMenuScreenManager.selectedScreenID].b, sprite.color.a);
    }
    private void Update()
    {
        if (MainMenuScreenManager.changingScreen)
        {
            if (invert[MainMenuScreenManager.selectedScreenID])
            {
                image.color = new Color(
                    Mathf.Lerp(image.color.r, -defaultCol.r + 1, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                     Mathf.Lerp(image.color.g, -defaultCol.g + 1, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                      Mathf.Lerp(image.color.b, -defaultCol.b + 1, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2),
                      image.color.a);
                //image.color = Color.Lerp(image.color, invertedCol, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2);
            }
            else
            {
                image.color = Color.Lerp(image.color, defaultCol, Time.deltaTime * MainMenuScreenManager.changingScreenSpeed * 2);
            }
        }
    }
}
