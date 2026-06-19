using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientVersionManager : MonoBehaviour
{
    public string setVersion;
    public int setNumber;

    public static string version;
    public static int versionNumber;
    private void Awake(){
        if (setVersion.Length > 0) { version = setVersion; }
        versionNumber = setNumber;
    }
}
