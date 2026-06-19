using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MMTextColorManager : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    Text text;
    public Color[] colours;
    private void Start()
    {
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        if (textMesh == null) { 
            text = gameObject.GetComponent<Text>();
            text.color = colours[MainMenuScreenManager.selectedScreenID];
        }
        else{
            textMesh.color = colours[MainMenuScreenManager.selectedScreenID];
        }
    }
    private void Update()
    {
        if (MainMenuScreenManager.changingScreen)
        {
            if (textMesh == null){
                text.color = Color.Lerp(text.color, colours[MainMenuScreenManager.selectedScreenID], Time.deltaTime * MainMenuScreenManager.changingScreenSpeed);
            }
            else{
                textMesh.color = Color.Lerp(textMesh.color, colours[MainMenuScreenManager.selectedScreenID], Time.deltaTime * MainMenuScreenManager.changingScreenSpeed);
            }
        }
    }
}
