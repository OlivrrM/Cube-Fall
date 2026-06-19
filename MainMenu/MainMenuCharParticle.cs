using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCharParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;
    float velocity;
    private void Start()
    {
        /*
        var main = particleSystem.main;
        velocity = transform.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        main.startSpeedMultiplier = velocity / 5;
        main.startSizeMultiplier = velocity / 5;
        */
        particleSystem.Play();
    }
}
