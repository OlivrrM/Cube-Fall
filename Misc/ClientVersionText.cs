using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientVersionText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Start(){
        string version = ClientVersionManager.version;
        if (!string.IsNullOrEmpty(version)) { text.text = "v" + ClientVersionManager.version; }
        else { text.text = "Version not detected"; }
    }
}
