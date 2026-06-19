using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowSkin : MonoBehaviour
{
    public SetSkin setSkinScript;
    string[] values = new string[2];

    public SpriteRenderer[] spriteRenderers;
    private void Start()
    {
        values = setSkinScript.selectedSkin.Split('-');
    }
    private void Update()
    {
        if (values[2] == "61")
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                float ogA = spriteRenderers[i].color.a;
                spriteRenderers[i].color = Utilities.GetRainbowColor(0.05f);
                spriteRenderers[i].color = new Color(spriteRenderers[i].color.r, spriteRenderers[i].color.g, spriteRenderers[i].color.b, ogA);
            }
        }
    }
}
