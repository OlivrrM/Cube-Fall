using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    private void Awake()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume",1);
    }
}
