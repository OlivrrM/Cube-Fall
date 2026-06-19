using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostFxManager : MonoBehaviour
{
    PostProcessVolume postFxVolume;
    Grain grainEffect;
    private void Start()
    {
        if (!Application.isMobilePlatform){
            postFxVolume = GameObject.Find("GlobalPostFX").GetComponent<PostProcessVolume>();
            postFxVolume.profile.TryGetSettings(out grainEffect);
            grainEffect.size.value = 0.8f;
        }
    }
}
