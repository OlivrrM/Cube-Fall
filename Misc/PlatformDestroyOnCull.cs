using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyOnCull : MonoBehaviour
{
    public static Transform cullY;
    public MeshRenderer[] platformRenderers;
    bool cleanedUpPlatforms;
    private void Start()
    {
        if (cullY == null) cullY = GameObject.Find("CullY").transform;
    }
    private void Update()
    {
        if (transform.position.y > cullY.position.y)
        {
            PlatformSpawn.platformsActive--;
            Destroy(gameObject);
        }

        if (!cleanedUpPlatforms)
        {
            if (platformRenderers[0].isVisible && !platformRenderers[1].isVisible) { Destroy(platformRenderers[1].gameObject); cleanedUpPlatforms = true; }
            else if (!platformRenderers[0].isVisible && platformRenderers[1].isVisible) { Destroy(platformRenderers[0].gameObject); cleanedUpPlatforms = true; }
        }
    }
}
