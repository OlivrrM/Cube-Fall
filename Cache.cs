using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cache : MonoBehaviour
{
    public static GameObject player;
    public static Transform groundCheck;
    public static bool isMobile;
    public bool forceMobileIdentity;
    public static GameObject postProcessor;
    private void Awake()
    {
        if (Application.isMobilePlatform || (Application.isEditor && forceMobileIdentity)) { isMobile = true; }
    }
}
