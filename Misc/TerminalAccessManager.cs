using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalAccessManager : MonoBehaviour
{
    public static bool access;
    public static Terminal terminalScript;
    public Terminal setTerminalScript;
    private void Awake()
    {
        access = Utilities.IntToBool(PlayerPrefs.GetInt("TerminalAccess", 0));
        if (Application.isEditor || !Application.isMobilePlatform) { access = true; }
        terminalScript = setTerminalScript;
        terminalScript.enabled = access;
    }
    public static void EnableDisableTerminalAccess()
    {
        access = !access;
        PlayerPrefs.SetInt("TerminalAccess", Utilities.BoolToInt(access));
        terminalScript.enabled = access;
    }
}
