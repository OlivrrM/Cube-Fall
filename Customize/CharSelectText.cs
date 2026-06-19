using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharSelectText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public SpriteRenderer charSpriteRenderer;
    private void Update()
    {
        text.color = charSpriteRenderer.color;
    }
}
