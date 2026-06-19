using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageSpawn : MonoBehaviour
{
    public float spawnChance;
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    private void Start()
    {
        float dice = Random.Range(0, 100);
        if (dice > (spawnChance * 100)) { Destroy(gameObject); }
        else { spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            if (Random.Range(0, 2) == 1) { spriteRenderer.flipX = true; }
        }
    }
}
