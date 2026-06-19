using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenSwap : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    int framesSinceLastSwap;

    public Level levelScript;
    private void Update()
    {
        framesSinceLastSwap++;
        if (StartTime.instanceTime > 1){
            if (!spriteRenderer.isVisible)
            {
                if (transform.position.x > 0 && framesSinceLastSwap > 3) { transform.position = new Vector3(-(transform.position.x - 0.25f), transform.position.y, transform.position.z); framesSinceLastSwap = 0; }
                else if (transform.position.x < 0 && framesSinceLastSwap > 3) { transform.position = new Vector3(-(transform.position.x + 0.25f), transform.position.y, transform.position.z); framesSinceLastSwap = 0; }
            }
        }
    }
}
