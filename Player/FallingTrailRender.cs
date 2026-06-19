using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FallingTrailRender : MonoBehaviour
{
    public TrailRenderer trail;
    public Rigidbody2D playerRB;

    public SpriteRenderer characterSpriteRenderer;
    private void Start()
    {
        trail.startColor = new Color(characterSpriteRenderer.color.r, characterSpriteRenderer.color.g, characterSpriteRenderer.color.b, trail.startColor.a);
    }
    private void Update()
    {
        if (playerRB.velocity.y < -12){
            trail.emitting = true;
        }
        else { 
            trail.emitting = false; 
        }
    }
}
