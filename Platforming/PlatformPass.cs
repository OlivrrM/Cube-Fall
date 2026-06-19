using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPass : MonoBehaviour
{
    public int scoreIncrease;
    public static int scoreIncreaseBonus;
    private void Update()
    {
        if (Cache.player.transform.position.y < transform.position.y) { Score.AddScore(scoreIncrease + scoreIncreaseBonus); this.enabled = false; }
    }
}
