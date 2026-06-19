using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BackText : MonoBehaviour
{
    public TextMeshProUGUI thisText;
    public TextMeshProUGUI targetText;
    private void Update(){
        thisText.text = targetText.text;
        thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, targetText.color.a);
    }
}
