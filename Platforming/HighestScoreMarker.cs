using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighestScoreMarker : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    float coloursLerpBall;
    public float colourLerpSpeed;
    public Color colA;
    public Color colB;
    public static PlaySound reachedMarkerSFX;
    public static PlaySound smashSFX;
    public ParticleSystem particle;
    bool played;
    private void Start()
    {
        if (reachedMarkerSFX == null) { reachedMarkerSFX = GameObject.Find("HsMarkerReachSFX").GetComponent<PlaySound>(); }
        if (smashSFX == null) { smashSFX = GameObject.Find("smashSFX").GetComponent<PlaySound>(); }
    }
    private void Update()
    {
        if (Cache.groundCheck.transform.position.y < transform.position.y)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, Utilities.Invisible(spriteRenderer.color), Time.deltaTime * 15);
            if (!played)
            {
                reachedMarkerSFX.Play();
                smashSFX.Play();
                var main = particle.main;
                main.startColor = spriteRenderer.color;
                var shape = particle.shape;
                shape.radius = 1+(PlatformObjectManager.gapMultiplier * 2);
                particle.Play();
                HapticFeedbackPlayer.Play(2);
                played = true;
            }
        }
        else
        {
            coloursLerpBall = Mathf.PingPong(Time.time * colourLerpSpeed, 1);
            spriteRenderer.color = Color.Lerp(colA, colB, coloursLerpBall);
        }
    }
}
