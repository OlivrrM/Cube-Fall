using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessorController : MonoBehaviour
{
    private void Awake()
    {
        if (Cache.postProcessor == null) Cache.postProcessor = GameObject.Find("GlobalPostFX");
        Refresh();
    }
    public static void Refresh()
    {
        Cache.postProcessor.SetActive(!Utilities.IntToBool(PlayerPrefs.GetInt("PostFxDisabled")));
    }
}
