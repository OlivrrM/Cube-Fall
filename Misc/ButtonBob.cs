using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonBob : MonoBehaviour, IPointerUpHandler
{
    RectTransform thisRectTransform;
    [HideInInspector] public Vector2 defaultScale;
    public float pressBob = 0.8f;
    public float bobSpeed = 3;
    void Start()
    {
        thisRectTransform = gameObject.GetComponent<RectTransform>();
        defaultScale = thisRectTransform.localScale;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Bob();
    }
    
    public void Bob()
    {
        thisRectTransform.localScale = new Vector3(defaultScale.x * pressBob, defaultScale.y * pressBob);
    }
    void Update()
    {
        thisRectTransform.localScale = Vector2.Lerp(thisRectTransform.localScale, defaultScale, Time.deltaTime * bobSpeed);
    }
}
