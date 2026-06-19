using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHitGround : MonoBehaviour
{
    public GameObject thumpSFX;
    public static float lastCollisionForce;
    public GameObject hitGroundHeavyFX;
    public GameObject hitGroundMediumFX;
    Transform effectsDump;

    public float cameraShakeMultiplier;

    public SpriteRenderer characterSpriteRenderer;
    private void Start()
    {
        effectsDump = GameObject.Find("EffectsDump").transform;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        lastCollisionForce = collision.relativeVelocity.magnitude;
        Instantiate(thumpSFX, collision.transform.position,Quaternion.identity);
        GameObject fx = null;

        float force = collision.relativeVelocity.magnitude;
        if (force >= 3 && force < 5) { HapticFeedbackPlayer.Play(0); }
        else if (force >= 5 && force < 12) { HapticFeedbackPlayer.Play(1); if (Utilities.CoinFlip()) { fx = Instantiate(hitGroundMediumFX, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0), Quaternion.Euler(-90, 0, 0), effectsDump); } if (Utilities.CoinFlip()) { cameraShake.Shake(new Vector3(0, Random.Range(0.15f * cameraShakeMultiplier, 0.3f * cameraShakeMultiplier), 0), 15); } }
        else if (force >= 12) { HapticFeedbackPlayer.Play(2); fx = Instantiate(hitGroundHeavyFX, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, 0), Quaternion.Euler(-90, 0, 0), effectsDump); cameraShake.Shake(new Vector3(0, Random.Range(0.5f * cameraShakeMultiplier, 1 * cameraShakeMultiplier), 0), 10); }
        if (fx != null){
            var main = fx.GetComponent<ParticleSystem>().main;
            main.startColor = characterSpriteRenderer.color;
            //if (MainMenuScreenManager.selectedScreenID > 0) { main.startColor = new Color(-characterSpriteRenderer.color.r + 1, -characterSpriteRenderer.color.g + 1, -characterSpriteRenderer.color.b + 1); }
            //else { main.startColor = characterSpriteRenderer.color; }
        }
    }
}
