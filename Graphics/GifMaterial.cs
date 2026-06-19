using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GifMaterial : MonoBehaviour
{
    public Texture2D[] frames;
    [SerializeField] private float fps = 10.0f;
    float currentFPS;
    public static float fpsMultiplier;

    public Material mat;

    public static Color col;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (frames.Length > 0)
        {
            currentFPS = fps * fpsMultiplier;
            int index = (int)(Time.time * currentFPS);
            index = index % frames.Length;
            mat.mainTexture = frames[index]; // usar en planeObjects
        }
        mat.color = col;
        //GetComponent<RawImage> ().texture = frames [index];
    }
}
