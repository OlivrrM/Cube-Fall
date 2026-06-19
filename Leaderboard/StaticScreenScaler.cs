using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticScreenScaler : MonoBehaviour
{
    public RectTransform canvas;
    public Transform staticScreen;
    public float scaleAmount;
    private void Update()
    {
        staticScreen.localScale = new Vector3(canvas.rect.width * scaleAmount, staticScreen.localScale.y, staticScreen.localScale.z);
    }
}
