using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPixel : MonoBehaviour
{
    public Coin coinScript;
    public Rigidbody rb;
    bool collected;
    public float decaySpeed;
    private void Update()
    {
        if (coinScript.collected)
        {
            if (!collected)
            {
                rb.isKinematic = false;
                rb.drag = Random.RandomRange(coinScript.dragAB.x, coinScript.dragAB.y);
                rb.AddExplosionForce(Random.RandomRange(coinScript.explosionForceAB.x, coinScript.explosionForceAB.y), coinScript.animator.transform.position,2,0,ForceMode.VelocityChange);
            }
            collected = true;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * decaySpeed);
        }
    }
}
