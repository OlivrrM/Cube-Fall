using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageVisibilityChecker : MonoBehaviour
{
    public SpriteRenderer sr;
    public CamViewEdgeCanvasScale cvecs;
    private void Start()
    {
        //Application.targetFrameRate = 50;   
    }
    private void Update()
    {
        /*
        if (!sr.isVisible)
        {
            print(Random.RandomRange(1, 10000));
            cvecs.rectTransform.anchoredPosition = new Vector2(0, cvecs.rectTransform.anchoredPosition.y);
            //cvecs.forward = false;
            //cvecs.moveMultiplier = 2;
        }
        */  
    }
}
