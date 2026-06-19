using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkin : MonoBehaviour
{
    public string selectedSkin;
    public Skins skinsScript;

    public SpriteRenderer[] spriteRenderers;
    private void Awake()
    {
        selectedSkin = EncryptedPlayerPrefs.GetString("skin" + EncryptedPlayerPrefs.GetInt("SelectedSkin"));
        if (selectedSkin == "" || string.IsNullOrEmpty(selectedSkin)) { selectedSkin = "0-0-0"; }
        string[] skinData = selectedSkin.Split('-');
        for (int i = 0; i < spriteRenderers.Length; i++){
            if (i > 0) { spriteRenderers[i].sprite = skinsScript.backGraphics[int.Parse(skinData[0])]; }
            else { spriteRenderers[i].sprite = skinsScript.graphics[int.Parse(skinData[0])]; }
            spriteRenderers[i].color = new Color(skinsScript.colours[int.Parse(skinData[1])].r, skinsScript.colours[int.Parse(skinData[1])].g, skinsScript.colours[int.Parse(skinData[1])].b, spriteRenderers[i].color.a);
        }
    }
}
