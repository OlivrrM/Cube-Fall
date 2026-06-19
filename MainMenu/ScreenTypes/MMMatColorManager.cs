using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMMatColorManager : MonoBehaviour
{
    MeshRenderer renderer;
    GifMaterial gifMaterialScript;
    public Material[] materials;

    public Color floorsColourA;
    public Color floorsColourB;
    public float floorsColourLerpSpeed;
    float floorsColoursLerpBall;

    public Renderer transitionRenderer;

    public static float transitionAlpha;

    public bool disableHellStuff;

    public Texture2D[] normalGifFrames;
    public Texture2D[] invertGifFrames;
    private void Start()
    {
        Enable();
    }
    private void OnEnable()
    {
        Enable();
    }
    void Enable()
    {
        gifMaterialScript = gameObject.GetComponent<GifMaterial>();
        transitionAlpha = 0;
        renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = materials[MainMenuScreenManager.selectedScreenID];
        if (gifMaterialScript != null) {
            gifMaterialScript.mat = materials[MainMenuScreenManager.selectedScreenID];
        }
        if (MainMenuScreenManager.selectedScreenID == 2&& gifMaterialScript!=null)// && !disableHellStuff) 
        { gifMaterialScript.mat.color = floorsColourA; gifMaterialScript.frames = normalGifFrames;print("Yay?"); }
        else if (MainMenuScreenManager.selectedScreenID == 1 && gifMaterialScript != null) { gifMaterialScript.frames = invertGifFrames; }
        else if (MainMenuScreenManager.selectedScreenID == 0 && gifMaterialScript != null) { gifMaterialScript.frames = normalGifFrames; }
    }
    private void Update()
    {
        if (transitionRenderer != null) { transitionRenderer.material.color = new Color(transitionRenderer.material.color.r, transitionRenderer.material.color.g, transitionRenderer.material.color.b, transitionAlpha); }
        if (MainMenuScreenManager.changingScreen)
        {
            if (renderer != null) { renderer.material = materials[MainMenuScreenManager.selectedScreenID]; }
            if (gifMaterialScript != null) { gifMaterialScript.mat = materials[MainMenuScreenManager.selectedScreenID]; }
        }
        if (MainMenuScreenManager.selectedScreenID == 2)//&&!disableHellStuff)
        {
            floorsColoursLerpBall = Mathf.PingPong(Time.time * floorsColourLerpSpeed, 1);
            if (gifMaterialScript != null)
            {
                gifMaterialScript.mat.color = Color.Lerp(floorsColourA, floorsColourB, floorsColoursLerpBall);
                gifMaterialScript.frames = normalGifFrames;
            }
        }
        else if (MainMenuScreenManager.selectedScreenID == 1 && gifMaterialScript!=null) { gifMaterialScript.frames = invertGifFrames; }
        else if (MainMenuScreenManager.selectedScreenID == 0 && gifMaterialScript != null) { gifMaterialScript.frames = normalGifFrames; }
    }
}
