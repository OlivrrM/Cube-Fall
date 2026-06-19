using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitGroundSFX : MonoBehaviour
{
    public PlaySound sfx;
    private void Start()
    {
        if (StartTime.instanceFrame > 5){
            float pitch = PlayerHitGround.lastCollisionForce;
            pitch = Mathf.Clamp(pitch, 1, 15f);
            pitch = 15 - pitch;
            pitch = Mathf.Clamp(pitch, 0.5f, 8f);
            sfx.SFX.volume = Mathf.Clamp(1 - (Mathf.InverseLerp(0.5f, 8, pitch)), 0.25f, 1);
            sfx.Play(true, pitch);
        }
    }
}
