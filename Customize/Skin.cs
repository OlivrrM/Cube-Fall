using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Skin : MonoBehaviour
{
    public static Skins skinsScript;
    public string skinString;

    public SpriteRenderer[] spriteRenderers;
    public MainMenuCharacter mainMenuCharacterScript;

    public float targetX;
    public float xMultiplier;

    public TargetSizeOverTime selectCharButton;

    public PlaySound selectSFX;

    string[] values = new string[2];
    private void Start()
    {
        if (skinsScript == null) { skinsScript = GameObject.Find("InstanceScripts").GetComponent<Skins>(); }

        values = skinString.Split('-');
        for (int i = 0; i < spriteRenderers.Length; i++){
            if (i > 0) { spriteRenderers[i].sprite = skinsScript.backGraphics[int.Parse(values[0])]; }
            else { spriteRenderers[i].sprite = skinsScript.graphics[int.Parse(values[0])]; }
            spriteRenderers[i].color = new Color(skinsScript.colours[int.Parse(values[1])].r, skinsScript.colours[int.Parse(values[1])].g, skinsScript.colours[int.Parse(values[1])].b,spriteRenderers[i].color.a);
        }
        //mainMenuCharacterScript.hitFloorSFX.SFX.clip = skinsScript.hitSfxs[int.Parse(values[2])];
        selectCharButton.gameObject.name = transform.name + "_SelectChar";
        selectCharButton.transform.parent = GameObject.Find("Canvas").transform;
        selectCharButton.GetComponent<CharacterSelect>().selectText.color = spriteRenderers[0].color;
    }
    private void Update()
    {
        transform.position = new Vector3(targetX*CustomizeInstance.skinXPosIndex*CustomizeInstance.xPosMultiplier, transform.position.y, transform.position.z);
        selectCharButton.transform.position = transform.position;

        if (values[2] == "61"){
            for (int i = 0; i < spriteRenderers.Length; i++){
                float ogA = spriteRenderers[i].color.a;
                spriteRenderers[i].color = Utilities.GetRainbowColor(0.05f);
                spriteRenderers[i].color = new Color(spriteRenderers[i].color.r, spriteRenderers[i].color.g, spriteRenderers[i].color.b, ogA);
            }
        }
    }
    public void Select()
    {
        EncryptedPlayerPrefs.SetInt("SelectedSkin", int.Parse(transform.name.Replace("Skin", "")));
        GameObject.Find("SelectSkinSFX").GetComponent<PlaySound>().Play();
    }
}
