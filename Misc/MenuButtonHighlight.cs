using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonHighlight : MonoBehaviour
{
    public ButtonBob buttonBobScript;
    public ParticleSystem[] particles;
    public PlaySound sfx;
    public bool selectOnStart;
    private void Start()
    {
        if (selectOnStart){
            buttonBobScript.defaultScale = new Vector3(1.2f, 1.2f, 1.2f);
            for (int i = 0; i < particles.Length; i++){
                particles[i].Play();
            }
        }
        else { Deselect(); }
    }
    private void OnMouseEnter()
    {
        buttonBobScript.defaultScale = new Vector3(1.2f, 1.2f, 1.2f);
        for (int i = 0; i < particles.Length; i++){
            particles[i].Play();
        }
        if (sfx != null) { sfx.Play(); }
    }
    private void OnMouseExit()
    {
        Deselect();
    }
    void Deselect()
    {
        buttonBobScript.defaultScale = new Vector3(1, 1, 1);
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
        }
    }
}
